using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public class SceneGenerator : MonoBehaviour
{
    
    [SerializeField] private int xSize = 50;
    [SerializeField] private int zSize = 50;
    private int[,] _grid;
    
    [SerializeField] private List<CellOfGrid> possibleCells = new();
    [SerializeField] private List<CellOfGrid> PathCells = new();
    private void Awake()
    {
        _grid = new int[xSize, zSize];
        for (int x = 0; x < _grid.GetLength(0); x++)
        {
            for (int z = 0; z < _grid.GetLength(1); z++)
            {
                _grid[x, z] = Random.Range(1, 10);
            }
        }
        for (int x = -4; x < 4; x++)
        {
            for (int z = -4; z < 4; z++)
            {
                _grid[_grid.GetLength(0)/2+x, _grid.GetLength(1)/2+z] = -1;
            }
        }
        _grid[0, 0] = -1;
    }

    private void Start()
    {
        Vector2Int[] way = WayGenerator.GenerateWay(_grid, new Pair<int, int>(5, 5), new Pair<int, int>(46, 46), new List<Pair<int, int>>() { new Pair<int, int>(0, 1), new Pair<int, int>(1, 0), new Pair<int, int>(0, -1), new Pair<int, int>(-1, 0) }, 1).Select(n => new Vector2Int(n.First, n.Second)).ToArray();
        
        HashSet<Vector2Int>[] buckets = new HashSet<Vector2Int>[33].Select(_ => new HashSet<Vector2Int>()).ToArray();
        Dictionary<Vector2Int, uint> possibleValue = new Dictionary<Vector2Int, uint>();
        
        for (int x = 0; x < xSize; x++)
        for (int z = 0; z < zSize; z++)
        {
            buckets[32].Add(new Vector2Int(x, z));
        }
        
        for(int i = 1; i < way.Length-1; i++)
        {
            int mask = 0;
            if(way[i] + Vector2.down == way[i+1]) mask |= 1<<0;
            if(way[i] + Vector2.up == way[i+1]) mask |= 1<<1;
            if(way[i] + Vector2.left == way[i+1]) mask |= 1<<2;
            if(way[i] + Vector2.right == way[i+1]) mask |= 1<<3;
            
            if(way[i] + Vector2.down == way[i-1]) mask |= 1<<0;
            if(way[i] + Vector2.up == way[i-1]) mask |= 1<<1;
            if(way[i] + Vector2.left == way[i-1]) mask |= 1<<2;
            if(way[i] + Vector2.right == way[i-1]) mask |= 1<<3;

            Transform cell = Instantiate(PathCells[mask].cellPrefab, new Vector3(way[i].x/(float)xSize, 0, way[i].y/(float)zSize), PathCells[mask].rotation, transform).transform;
            cell.localPosition = new Vector3(way[i].x/(float)xSize-0.5f, 1, way[i].y/(float)zSize-0.5f);
            cell.localRotation = PathCells[mask].rotation;
            cell.localScale = new Vector3(1/(float)xSize, 0.02f, 1/(float)zSize);
            SetCell(ref buckets, ref possibleValue, PathCells[mask], way[i].x, way[i].y);
        }
       
        StartCoroutine(Corutine(buckets, possibleValue));
    }

    IEnumerator Corutine(HashSet<Vector2Int>[] buckets, Dictionary<Vector2Int, uint> possibleValue)
    {
        while (true)
        {
            (Vector2Int pos, uint mask) current = PopCell(ref buckets, ref possibleValue);
            if(current.pos == new Vector2Int(-1, -1)) break;
            
            (CellOfGrid cell, Quaternion rotation) currentIndex = GetRandom(current.mask);
            
            Transform cell = Instantiate(currentIndex.cell.cellPrefab, new Vector3(current.pos.x/(float)xSize, 0, current.pos.y/(float)zSize), currentIndex.cell.rotation, transform).transform;
            cell.localPosition = new Vector3(current.pos.x/(float)xSize-0.5f, 1, current.pos.y/(float)zSize-0.5f);
            cell.localRotation = currentIndex.cell.rotation;
            cell.localScale = new Vector3(1/(float)xSize, 0.02f, 1/(float)zSize);
            SetCell(ref buckets, ref possibleValue, currentIndex.cell, current.pos.x, current.pos.y);
            yield return null;
        }
    }
    
    

    private void SetCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue, CellOfGrid cell, int x, int y)
    {
        SetCell(ref buckets, ref possibleValue, cell.numUpSide, new Vector2Int(x, y+1));
        SetCell(ref buckets, ref possibleValue, cell.numDownSide, new Vector2Int(x, y-1));
        SetCell(ref buckets, ref possibleValue, cell.numRightSide, new Vector2Int(x+1, y));
        SetCell(ref buckets, ref possibleValue, cell.numLeftSide, new Vector2Int(x-1, y));
        //_grid[x, y] = 0;
        /*_grid[x, y+1] &= cell.numFrontSide;
        _grid[x, y-1] &= cell.numBackSide;
        _grid[x+1, y] &= cell.numRightSide;
        _grid[x-1, y] &= cell.numLeftSide;*/
    }

    private Vector2Int GetCell(HashSet<Vector2Int>[] buckets)
    {
        for (int i = 0; i < buckets.Length; i++)
        {
            if (buckets[i].Count != 0)
            {
                return buckets[i].First();
            }
        }
        return new Vector2Int(-1, -1);
    }

    private (Vector2Int pos, uint mask) PopCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue)
    {
        (Vector2Int pos, uint mask) result = (new Vector2Int(-1, -1), 0);
        for (int i = 0; i < buckets.Length; i++)
        {
            if (buckets[i].Count != 0)
            {
                result.pos = buckets[i].First();
                buckets[i].Remove(result.pos);
            }
        }
        result.mask = possibleValue.ContainsKey(result.pos) ? possibleValue[result.pos] : uint.MaxValue;
        possibleValue.Remove(result.pos);
        return result;
    }
    private void SetCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue, uint num, Vector2Int cell)
    {
        for (int i = 0; i < buckets.Length; i++)
            buckets[i].Remove(cell);

        if(!possibleValue.TryAdd(cell, num))possibleValue[cell] &= num;

        buckets[PopCount(num)].Add(cell);
    }
    
    public static int PopCount(uint i)
    {
        i = i - ((i >> 1) & 0x55555555);
        i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
        return (int)((((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
    }

    private (CellOfGrid, Quaternion) GetRandom(uint mask)
    {
        List<(CellOfGrid, Quaternion)> possibleValues = new List<(CellOfGrid, Quaternion)>();

        foreach (var possibleCell in possibleCells)
        {
            /*for (int i = 0; i < 4; i++)
            {
                uint moved = (mask << 8*i) | (mask >> 32-8*i);*/
                Debug.Log(possibleCell.num + " | " +mask);
                if ((possibleCell.num & mask) == possibleCell.num)
                {
                    possibleValues.Add((possibleCell, Quaternion.Euler(0f, possibleCell.rotation.eulerAngles.y /*90*i*/, 0f)));
                }
            //}
        }
        
        
        return possibleValues[Random.Range(0, possibleValues.Count-1)];
    }
}
