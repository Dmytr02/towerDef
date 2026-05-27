using System;
using System.Collections;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Management;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;

    private void Update()
    {
        #if UNITY_EDITOR
            if(!pauseMenu) return;
            if (Input.GetKey(KeyCode.P)) Time.timeScale = 10;
            else if(pauseMenu.activeSelf) Time.timeScale = 0;
            else Time.timeScale = 1;
        #endif
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
    private IEnumerator ExecuteReload(int name)
    {
        
        // 1. Całkowicie zatrzymaj podsystemy XR i skaner środowiska symulacji
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }

        // 2. Poczekaj dwie klatki, aby Unity przetworzyło usunięcie obiektów z pamięci
        yield return null;
        yield return null;

        // 3. Ładowanie nowej sceny w trybie Single (czyści starą scenę)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 4. Poczekaj klatkę na zainicjalizowanie obiektów nowej sceny
        yield return null;

        
        
        
        /*// 6. Znajdujemy NOWĄ sesję AR, włączamy ją i wymuszamy twardy reset podsystemu kamery
        ARSession newSession = FindFirstObjectByType<ARSession>();
        if (newSession != null)
        {
            newSession.enabled = true;
            newSession.Reset(); // To budzi kamerę w Edytorze (Symulacji) oraz na Telefonie
        }*/
    }

    private void Start()
    {
        StartCoroutine(StartCorutine());
    }

    IEnumerator StartCorutine()
    {
        // 5. Uruchom podsystemy XR na nowo dla nowej sceny
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }
    public void ClearDontDestroy(int name)
    {
        StartCoroutine(ExecuteReload(name));
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
