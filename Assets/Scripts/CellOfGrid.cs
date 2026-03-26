using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CellOfGrid", menuName = "ScriptableObjects/CellOfGrid")]
public class CellOfGrid : ScriptableObject
{
    public GameObject cellPrefab;
    public Quaternion rotation;
    [UintBytesAttribute] public uint num;
    
    
    
    [UintBytesAttribute]  public uint numRightSide;
    [UintBytesAttribute]  public uint numLeftSide;
    [UintBytesAttribute]  public uint numUpSide;
    [UintBytesAttribute]  public uint numDownSide;
}
