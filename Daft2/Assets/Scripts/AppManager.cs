using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    //
    AndroidJavaObject activity;

    public IEnumerator LoadGameScene()
    {
        MenuScreenController menuController = GameObject.FindGameObjectWithTag("MenuController").GetComponent<MenuScreenController>();

        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            menuController.SetLoadingProgress(async.progress);

            // Scena się załadowała
            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Start()
    {
        #if UNITY_ANDROID

                activity =
                   new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                   .GetStatic<AndroidJavaObject>("currentActivity");

        #endif
    }

    private void Update()
    {
        // Zminimalizuj aplikację
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MinimizeWindow();
        }
    }

    public void LoadMenuScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void MinimizeWindow()
    {
        if (activity != null)
            activity.Call<bool>("moveTaskToBack", true);
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("focus");
        if (focus)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("pause");
        if (!pause)
            Time.timeScale = 1.0f;
        else
            Time.timeScale = 0.0f;
    }
}
