using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BuildTowerManager : MonoBehaviour
{
    public static BitArr2D possibleValues;
    [SerializeReference] public Tower SelectedTower;
    public GameObject Prefab;
    public Vector2Int Size;
    public Mesh previewMesh;
    public Material previewMaterial;
    public Material previewMaterialBlocked;
    public MultiTouchEventTrigger MultiTouch;

    public void CastToTryBuild(PointerEventData touch)
    {
        if(SceneGenerator.m_transform == null) return;
        SceneGenerator.m_transform.parent.GetComponent<XRGrabInteractable>().trackPosition = true;
        if(SelectedTower == null) return;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        Plane plane = new Plane(SceneGenerator.m_transform.up, SceneGenerator.m_transform.position+new Vector3(0, SceneGenerator.m_transform.lossyScale.y,0));
        if (plane.Raycast(ray, out float enter))
        {
            TryBuild(SceneGenerator.m_transform.InverseTransformPoint(ray.GetPoint(enter)));
        }
    }
    public void TryBuild(Vector3 position) => TryBuild(new Vector2(position.x, position.z) * SceneGenerator._gridSize);
    public void TryBuild(Vector2 position)
    {
        if (!CanBuild(position, out Vector2 towerPosition, true)) return;
        GameObject tower = Instantiate(SelectedTower.Prefab, SceneGenerator.m_transform);
        tower.transform.localPosition = new Vector3(towerPosition.x-0.5f, 1, towerPosition.y-0.5f);
        tower.transform.localScale = new Vector3(1.0f /SceneGenerator._gridSize.x, 20.0f /SceneGenerator._gridSize.x, 1.0f /SceneGenerator._gridSize.x);
        tower.transform.localRotation = Quaternion.identity;
    }

    public bool CanBuild(Vector3 position, out Vector2 towerPosition, bool isBuilding = false)
    {
        return CanBuild(new Vector2(position.x, position.z) * SceneGenerator._gridSize, out towerPosition, isBuilding);
    }

    public bool CanBuild(Vector2 position, out Vector2 towerPosition, bool isBuilding = false)
    {
        towerPosition = position;
        List<Vector2Int> posList = new List<Vector2Int>();
        for (int x = 0; x < SelectedTower.Size.x; x++)
        {
            for (int y = 0; y < SelectedTower.Size.y; y++)
            {
                Vector2Int pos = Vector2Int.CeilToInt((Vector2)SceneGenerator._gridSize*0.5f+position+(Vector2)SelectedTower.Size*-0.5f+new Vector2(x, y));
                if (possibleValues[pos.x, pos.y])
                {
                    Debug.Log(pos);
                    return false;
                }
                posList.Add(pos);
            }
        }
        towerPosition = ((Vector2)posList.Aggregate((i, vector2Int) => i + vector2Int)) /posList.Count/SceneGenerator._gridSize;
        if(isBuilding) foreach (Vector2Int pos in posList) possibleValues[pos.x, pos.y] = true;
        
        Debug.Log("Completed");
        return true;
    }


    private void Start()
    {
        SelectedTower = new Tower(){Prefab = Prefab, Size = Size};
        possibleValues = new BitArr2D(50, 50);
        MultiTouch.OnPointerUpEvent.AddListener(CastToTryBuild);
        MultiTouch.OnDragEvent.AddListener(Drag);
        MultiTouch.OnPointerDownEvent.AddListener((arg0 =>
        {
            if(SceneGenerator.m_transform != null) SceneGenerator.m_transform.parent.GetComponent<XRGrabInteractable>().trackPosition = SelectedTower == null;
        }));
    }

    private void Drag(PointerEventData touch)
    {
        Debug.Log("Building tower0");
        if(SceneGenerator.m_transform == null || SelectedTower == null) return;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        Plane plane = new Plane(SceneGenerator.m_transform.up, SceneGenerator.m_transform.position+new Vector3(0, SceneGenerator.m_transform.lossyScale.y*1.5f,0));
        if (plane.Raycast(ray, out float enter))
        {
            if (CanBuild(SceneGenerator.m_transform.InverseTransformPoint(ray.GetPoint(enter)), out Vector2 position))
            {
                Graphics.DrawMeshInstanced(previewMesh, 0, previewMaterial, new[]
                {
                    Matrix4x4.TRS(
                        SceneGenerator.m_transform.localToWorldMatrix.MultiplyPoint(new Vector3(position.x - 0.5f, 1f, position.y - 0.5f)), SceneGenerator.m_transform.rotation,
                        new Vector3(SceneGenerator.m_transform.lossyScale.x/SceneGenerator._gridSize.x*SelectedTower.Size.x, SceneGenerator.m_transform.lossyScale.x/SceneGenerator._gridSize.x, SceneGenerator.m_transform.lossyScale.z/SceneGenerator._gridSize.y*SelectedTower.Size.y))
                });
            }
            else
            {
                Graphics.DrawMeshInstanced(previewMesh, 0, previewMaterialBlocked, new[]
                {
                    Matrix4x4.TRS(
                        SceneGenerator.m_transform.localToWorldMatrix.MultiplyPoint(new Vector3(position.x - 0.5f, 1f, position.y - 0.5f)), SceneGenerator.m_transform.rotation,
                        new Vector3(SceneGenerator.m_transform.lossyScale.x/SceneGenerator._gridSize.x*SelectedTower.Size.x, SceneGenerator.m_transform.lossyScale.x/SceneGenerator._gridSize.x, SceneGenerator.m_transform.lossyScale.z/SceneGenerator._gridSize.y*SelectedTower.Size.y))
                });
            }
        }

    }

    private void OnDestroy()
    {
        MultiTouch.OnPointerUpEvent.RemoveListener(CastToTryBuild);
        MultiTouch.OnDragEvent.RemoveListener(Drag);
    }
}

public class BitArr
{
    byte[] bytes;

    public bool this[int i]
    {
        get { return (bytes[i/8] & (1<<(i%8))) != 0; }
        set
        {
            if(value) bytes[i/8] = (byte)(bytes[i/8] | (1 << (i%8)));
            else bytes[i/8] = (byte)(bytes[i/8] & (~(1 << (i%8))));
        }
    }
    
    public BitArr(byte[] bytes)
    {
        this.bytes = bytes;
    }
    public BitArr(BitArr bitArr)
    {
        this.bytes = bitArr.bytes;
    }
    public BitArr(int size)
    {
        bytes = new byte[(size-1)/8+1];
    }
    public BitArr(int size, bool value)
    {
        bytes = new byte[(size-1)/8+1];
        Array.Fill(bytes, value? (byte)0xFF : (byte)0);
    }

    override public string ToString()
    {
        return string.Join("", bytes.Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));
    }
}

public class BitArr2D
{
    BitArr bitArr;
    int xsize;
    int ysize;

    public bool this[int x, int y]
    {
        get
        {
            if(x < 0 || x >= xsize || y < 0 || y >= ysize) return true;
            return bitArr[(y * xsize) + x];
        }
        set
        {
            if (!(x < 0 || x >= xsize || y < 0 || y >= ysize))
            {
                bitArr[(y * xsize) + x] = value;
            }
        }
    }

    public BitArr2D(int xsize, int ysize)
    {
        bitArr = new BitArr(xsize * ysize);
        this.xsize = xsize;
        this.ysize = ysize;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<mspace=12>");

        for (int y = 0; y < ysize; y++)
        {
            for (int x = 0; x < xsize; x++)
            {
                sb.Append(this[x, y] ? "1" : "0");
            }
            sb.Append("\n");
        }

        sb.Append("</mspace>");
        return sb.ToString();
    }
}

[Serializable]
public class Tower
{
    public GameObject Prefab;
    public Vector2Int Size;
}