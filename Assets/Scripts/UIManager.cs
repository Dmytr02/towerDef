using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;


    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    } public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;     
        #endif
    }
}
