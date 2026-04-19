using System;
using UnityEngine;

public class TowerSelectButton : MonoBehaviour
{
    [SerializeField] TowerData towerData;
    [SerializeField] BuildTowerManager buildTowerManager;
    [SerializeField] MultiTouchEventTrigger eventTrigger;
    
    static Action onTowerSelected;
    private void Start()
    {
        eventTrigger.OnPointerDownEvent.AddListener(touch =>
        {
            onTowerSelected?.Invoke();
            
            if (buildTowerManager.SelectedTower == towerData)
                buildTowerManager.SelectedTower = null;
            else
            {
                buildTowerManager.SelectedTower = towerData;
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
