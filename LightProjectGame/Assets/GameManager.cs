using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Linq;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public CinemachineFreeLook[] cams;
    public static GameManager instance;
    private float hitPauseTime = 0.2f;
    public static TargetInfo[] enemyTargetsInScene;
    public List<StatusManager> enemysInScene = new List<StatusManager>();
    public Transform lastSavePoint;
    [Header("Interface")]
    public GameObject canvas;
    public GameObject deathMessageUI;
    public GameObject savingUI;
    public GameObject pauseMenu;
    public Animator loadingScreenUI;
    public TextMeshProUGUI locationText;
    public Animator locationTextAnimator;
    public TextMeshProUGUI subtitle;
    public GameObject fpsCounter;

    public GameObject[] keyboardUIElements;
    public GameObject[] gamepadUIElements;

    public PlayableDirector currentCutszene;

    public Transform interactText;
    public Animator[] interactTextAnims;

    private bool controllerUsed = false;


    public bool isLoadingLevel = false;

    private GameObject player;






    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            player = GameObject.Find("Player");
            instance = this;
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        }
        else
        {
            
            instance.player.transform.position = gameObject.transform.parent.GetComponentInChildren<PlayerController>(true).transform.position;
            Destroy(gameObject.transform.parent.gameObject);
            //this.enabled = false;
        }
        if(currentCutszene != null)
        {
            player.GetComponent<CutszeneState>().startCutszene = true;
            player.GetComponent<StateMachine>().ForceState(player.GetComponent<CutszeneState>());
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ReloadThisLevel();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.F3))
        {
            fpsCounter.SetActive(!fpsCounter.activeSelf);
        }
        if(Input.GetButtonDown("Pause"))
        {
            if(Time.timeScale == 0)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
            
        }

        ControllerUsed = Gamepad.current != null || Joystick.current != null;


    }

    private static void Init()
    {
        Time.timeScale = 1;
    }

    public static void AddToEnemyTargetList(TargetInfo targetInfo)
    {
        List<TargetInfo> list = new List<TargetInfo>();
        list.Add(targetInfo);
        enemyTargetsInScene = list.Where(e => e != null).ToArray();
    }

    public void LoadLevel(int i)
    {
        Init();

        SceneManager.LoadScene(i);
    }

    public void LoadLevelAsync(int level)
    {
        isLoadingLevel = true;
        loadingScreenUI.SetBool("animate", true);
        StartCoroutine(LoadLevelAsyncCoroutine(level,false));
        player.GetComponent<StateMachine>().ForceState(player.GetComponent<TransitioningLevel>());
    }

    public void LoadSceneAndDestroyPlayer(int level)
    {
        isLoadingLevel = true;
        loadingScreenUI.SetBool("animate", true); 
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(LoadLevelAsyncCoroutine(level,true));
        player.GetComponent<StateMachine>().ForceState(player.GetComponent<TransitioningLevel>());
    }

    private IEnumerator LoadLevelAsyncCoroutine(int level,bool destroyPlayer)
    {
        float time = 0;
       
        while (time < 2)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        // Begin loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(level);

        // Don't allow the scene to be shown while it's still loading
        asyncOperation.allowSceneActivation = false;
        

        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone || time < 3)
        {
            // The progress property is a value between 0 and 1, indicating the loading progress
            float progress = asyncOperation.progress;

            // Here you can display a loading progress bar or update a loading screen UI

            // If the loading is almost complete, allow the scene to be activated
            if (progress >=0.9f)
            {
                if(destroyPlayer)
                {
                    Destroy(gameObject.transform.parent.gameObject);
                }
                asyncOperation.allowSceneActivation = true;
            }
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        loadingScreenUI.SetBool("animate", false);
        isLoadingLevel = false;
    }



    public void LoadLevel(string name)
    {
        Init();

        SceneManager.LoadScene(name);
    }

    public static void ReloadThisLevel()
    {
        Init();
        instance.cams[0].m_YAxis.m_MaxSpeed = 1;
        instance.cams[0].m_XAxis.m_MaxSpeed = 300;
        if (instance.lastSavePoint != null)
        {
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerController>().RevivePlayer();
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = instance.lastSavePoint.position;
            player.GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameObject player = GameObject.Find("Player");
            player.GetComponent<PlayerController>().RevivePlayer();
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<CharacterController>().enabled = true;
        }

    }

    public static void ExitApplication()
    {
        Application.Quit(0);
    }

    public static void CallHitPause()
    {
        return;
    }

    public IEnumerator HitPause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(hitPauseTime);
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        cams[0].m_YAxis.m_MaxSpeed = 0;
        cams[0].m_XAxis.m_MaxSpeed = 0;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        cams[0].m_YAxis.m_MaxSpeed = 1;
        cams[0].m_XAxis.m_MaxSpeed = 300;
        Cursor.lockState = CursorLockMode.Locked;
        cams[0].m_StandbyUpdate = CinemachineVirtualCameraBase.StandbyUpdateMode.RoundRobin;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }





    public static void SpawnInteractText(Vector3 pos)
    {
        instance.interactText.parent.position = pos;
        instance.interactText.transform.localEulerAngles = Vector3.zero;
        foreach (Animator anim in instance.interactTextAnims)
        {
            anim.SetBool("animate", true);
        }
    }

    public static void DespawnInteractText() 
    {
        instance.interactText.parent.position = new Vector3(0,-1000,0);
        foreach (Animator anim in instance.interactTextAnims)
        {
            anim.SetBool("animate", false);
        }
    }

    public bool ControllerUsed
    {
        get => controllerUsed; set
        {
            if (value != controllerUsed)
            {
                foreach(GameObject go in gamepadUIElements)
                {
                    go.SetActive(value);
                }

                foreach (GameObject go in keyboardUIElements)
                {
                    go.SetActive(!value);
                }
            }
            controllerUsed = value;
        }
    }
}
