using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CellOfGrid", menuName = "ScriptableObjects/CellOfGrid")]
public class CellOfGrid : ScriptableObject
{
    public GameObject cellPrefab;
    public Material mat;
    public Mesh mesh;

    public Quaternion rotation; 
    [UintBytesAttribute(Name1 = "right", Name2 = "Up", Name3 = "Left", Name4 = "Down")]
    public uint num;
    
    [UintBytesAttribute(Name1 = "right", Name2 = "Up", Name3 = "Left", Name4 = "Down")]
    public uint sides;

    public int chance = 1;
}
