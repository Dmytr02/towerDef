using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TowerManagerSingletone : MonoBehaviour
{
    public static TowerManagerSingletone Instance;
    [SerializeField] private MultiTouchEventTrigger eventTrigger;
    private BaseTower _selectedTower;
    [SerializeField] private TMP_Text towerDescription;

    public BaseTower selectedTower
    {
        get => _selectedTower;
        set
        {
            _selectedTower = value;
            if (value != null)
            {
                visualPanel.SetActive(true);
                towerDescription.text = value.GetStats();
            }
            else visualPanel.SetActive(false);
        }
    }
    public Mesh visualizeMesh;
    public Material visualizeMaterial;

    public GameObject visualPanel;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        visualPanel.SetActive(false);
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

        TowerData nextData = selectedTower.data.nextLvl;

        if (nextData == null) {
            print("nextdata empty");
            return;
        }

        if (PlayerStats.Instance.coins >= nextData.cost) {
            PlayerStats.Instance.coins -= nextData.cost;
            
            selectedTower.data = nextData;
            selectedTower.TowerMeshFilter.mesh = selectedTower.data.mesh;
            
            /*Vector3 oldPosition = selectedTower.transform.localPosition;
            Quaternion oldRotation = selectedTower.transform.localRotation;
            Vector3 oldScale = selectedTower.transform.localScale;
            Transform parent = selectedTower.transform.parent;*/

            //Destroy(selectedTower.gameObject);
            
            //selectedTower = Instantiate(nextData.prefab, parent);
            //selectedTower.transform.localPosition = oldPosition;
            //selectedTower.transform.localRotation = oldRotation;
            //selectedTower.transform.localScale = oldScale;



            print("tower updated: ");
        } else {
            print("No money");
        }
    }

    public void RemoveSelectedTower()
    {
        if (!selectedTower) return;
        
        PlayerStats.Instance.coins += selectedTower.data.recoverCost;
        BuildTowerManager.DestroyTower(selectedTower);
        Destroy(selectedTower.gameObject);
        selectedTower = null;
        SceneGenerator.m_transform.parent.GetComponent<XRGrabInteractable>().trackPosition = true;
    }
}
