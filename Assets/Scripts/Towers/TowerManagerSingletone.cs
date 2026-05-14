using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TowerManagerSingletone : MonoBehaviour
{
    public static TowerManagerSingletone Instance;
    [SerializeField] private MultiTouchEventTrigger eventTrigger;
    public BaseTower selectedTower;
    public Mesh visualizeMesh;
    public Material visualizeMaterial;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        eventTrigger.OnPointerDownEvent.AddListener(TrySelect);
    }

    void Update()
    {
        if (selectedTower)
        {
            Graphics.DrawMeshInstanced(visualizeMesh, 0, visualizeMaterial, new List<Matrix4x4>()
            {
                Matrix4x4.TRS(selectedTower.transform.position, Quaternion.identity, selectedTower.data.range*Vector3.one)
            });
        }
    }


    public void TrySelect(PointerEventData eventData)
    {
        if (BuildTowerManager.SelectedTower || !SceneGenerator.m_transform) return;
        
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, LayerMask.GetMask("Tower")))
        {
            
            if (hit.transform.TryGetComponent(out BaseTower tower))
            {
                SceneGenerator.m_transform.parent.GetComponent<XRGrabInteractable>().trackPosition = false;
                selectedTower = tower;
                return;
            }
        }

        selectedTower = null;
        SceneGenerator.m_transform.parent.GetComponent<XRGrabInteractable>().trackPosition = true;
    }

    public void UpgradeSelectedTower()
    {
        if (!selectedTower) return;

        if (selectedTower.data.nextLvl && selectedTower.data.nextLvl.cost >= PlayerStats.Instance.coins)
        {
            selectedTower.data = selectedTower.data.nextLvl;
            PlayerStats.Instance.coins -= selectedTower.data.nextLvl.cost;
        }
    }
}
