using System;
using UnityEngine;

public class TowerSelectButton : MonoBehaviour
{
    [SerializeField] BaseTower towerData;
    [SerializeField] BuildTowerManager buildTowerManager;
    [SerializeField] MultiTouchEventTrigger eventTrigger;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;
    
    static Action onTowerSelected;
    private void Start()
    {
        eventTrigger.OnPointerDownEvent.AddListener(touch =>
        {
            audioSource.PlayOneShot(audioClip);
            onTowerSelected?.Invoke();
            
            if (BuildTowerManager.SelectedTower == towerData)
                BuildTowerManager.SelectedTower = null;
            else
            {
                BuildTowerManager.SelectedTower = towerData;
                Select();
            }
        });
        onTowerSelected += () =>
        {
            Deselect();
        };
    }

    void Select()
    {
        transform.localScale = Vector3.one*1.2f;
    }

    void Deselect()
    {
        transform.localScale = Vector3.one;
    }
}
