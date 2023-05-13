using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void ReloadThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void ExitApplication()
    {
        Application.Quit(0);
    }
}
