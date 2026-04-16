using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildTowerManager : MonoBehaviour
{
    public static BitArr2D possibleValues;
    [SerializeReference] public Tower SelectedTower;
    public GameObject Prefab;
    public Vector2Int Size;
    public MultiTouchEventTrigger MultiTouch;

    public void CastToTryBuild(PointerEventData touch)
    {
        //Debug.Log(touch.position);
        if(SceneGenerator.m_transform == null || SelectedTower == null) return;
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
        Debug.Log(position);
        List<Vector2Int> posList = new List<Vector2Int>();
        for (int x = 0; x < SelectedTower.Size.x; x++)
        {
            for (int y = 0; y < SelectedTower.Size.y; y++)
            {
                Vector2Int pos = Vector2Int.CeilToInt((Vector2)SceneGenerator._gridSize*0.5f+position+(Vector2)SelectedTower.Size*-0.5f+new Vector2(x, y));
                if (possibleValues[pos.x, pos.y])
                {
                    Debug.Log(pos);
                    return;
                }
                posList.Add(pos);
            }
        }
        Vector2 towerPosition = ((Vector2)posList.Aggregate((i, vector2Int) => i + vector2Int)) /posList.Count/SceneGenerator._gridSize;
        GameObject tower = Instantiate(SelectedTower.Prefab, SceneGenerator.m_transform);
        tower.transform.localPosition = new Vector3(towerPosition.x-0.5f, 1, towerPosition.y-0.5f);
        tower.transform.localScale = new Vector3(1.0f /SceneGenerator._gridSize.x, 1.0f /SceneGenerator._gridSize.x, 1.0f /SceneGenerator._gridSize.x);
        tower.transform.localRotation = Quaternion.identity;
        foreach (Vector2Int pos in posList) possibleValues[pos.x, pos.y] = true;
    }

    
    
    private void Start()
    {
        possibleValues = new BitArr2D(50, 50);
        MultiTouch.OnPointerDownEvent.AddListener(CastToTryBuild);
    }

    private void OnDestroy()
    {
        MultiTouch.OnDragEvent.RemoveListener(CastToTryBuild);
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
        get { return bitArr[(y * xsize) + x]; }
        set { bitArr[(y * xsize) + x] = value; }
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