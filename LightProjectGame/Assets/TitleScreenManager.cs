using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int i)
    {
        StartCoroutine(LoadLevelAsyncCoroutine(i));
    }

    public void ExitGame()
    {
        Application.Quit();
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

    }
}
