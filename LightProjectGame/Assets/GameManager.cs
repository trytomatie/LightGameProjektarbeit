using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cams;
    public static GameManager instance;
    private float hitPauseTime = 0.2f;
    public static TargetInfo[] enemyTargetsInScene;
    public List<StatusManager> enemysInScene = new List<StatusManager>();
    public Transform lastSavePoint;
    [Header("Interface")]
    public GameObject canvas;
    public GameObject deathMessageUI;
    public GameObject savingUI;
    public Animator loadingScreenUI;
    public TextMeshProUGUI locationText;
    public Animator locationTextAnimator;


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
        StartCoroutine(LoadLevelAsyncCoroutine(level));
        player.GetComponent<StateMachine>().ForceState(player.GetComponent<TransitioningLevel>());
    }

    private IEnumerator LoadLevelAsyncCoroutine(int level)
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
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // Here you can display a loading progress bar or update a loading screen UI

            // If the loading is almost complete, allow the scene to be activated
            if (progress >= 0.9f)
            {
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
        if(instance.lastSavePoint != null)
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
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(hitPauseTime);
        Time.timeScale = 1;
    }
}
