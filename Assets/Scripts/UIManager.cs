using System;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject pauseMenu;

    private void Start()
    {
        ShowMenu();
        SetTimeScale(0);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    } 
    public void HideMenu()
    {
        menu.SetActive(false);
    }
    
    public void ShowMenu()
    {
        menu.SetActive(true);
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
