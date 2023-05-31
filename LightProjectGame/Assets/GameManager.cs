using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cams;
    public static GameManager instance;
    private float hitPauseTime = 0.2f;
    public static TargetInfo[] enemyTargetsInScene;
    public List<StatusManager> enemysInScene = new List<StatusManager>();
    public Transform lastSavePoint;
    [Header("Interface")]
    public GameObject deathMessageUI;
    public GameObject savingUI;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ReloadThisLevel();
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
