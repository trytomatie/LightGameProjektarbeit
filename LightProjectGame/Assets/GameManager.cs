using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private float hitPauseTime = 0.05f;
    public static List<TargetInfo> enemyTargetsInScene = new List<TargetInfo>();
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            this.enabled = false;
        }
    }

    public static void AddToEnemyTargetList(TargetInfo targetInfo)
    {
        enemyTargetsInScene.Add(targetInfo);
    }

    public void LoadLevel(int i)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(i);
    }

    public void LoadLevel(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }

    public static void ReloadThisLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void ExitApplication()
    {
        Application.Quit(0);
    }

    public static void CallHitPause()
    {
        instance.StartCoroutine(instance.HitPause());
    }

    public IEnumerator HitPause()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(hitPauseTime);
        Time.timeScale = 1;
    }

}
