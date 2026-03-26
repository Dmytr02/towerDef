using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellOfGrid", menuName = "ScriptableObjects/CellOfGrid")]
public class CellOfGrid : ScriptableObject
{
    public GameObject cellPrefab;
    public Quaternion rotation;
    public int numRightSide;
    public int numLeftSide;
    public int numFrontSide;
    public int numBackSide;
}
