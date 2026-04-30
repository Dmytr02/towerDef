using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menu;

    private void Start()
    {
        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }
}
