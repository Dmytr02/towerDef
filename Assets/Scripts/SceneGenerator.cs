using System;
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
        var way = WayGenerator.GenerateWay(_grid, new Pair<int, int>(5, 5), new Pair<int, int>(46, 46), new List<Pair<int, int>>() { new Pair<int, int>(0, 1), new Pair<int, int>(1, 0), new Pair<int, int>(0, -1), new Pair<int, int>(-1, 0) }, 1).Select(n => new Vector2Int(n.First, n.Second)).ToArray();

        for (var index0 = 0; index0 < _grid.GetLength(0); index0++)
            for (var index1 = 0; index1 < _grid.GetLength(1); index1++) {
                _grid[index0, index1] = -1;
                
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
            SetCell(PathCells[mask], way[i].x, way[i].y);
        }
    }

    private void SetCell(CellOfGrid cell, int x, int y)
    {
        _grid[x, y] = 0;
        _grid[x, y+1] &= cell.numFrontSide;
        _grid[x, y-1] &= cell.numBackSide;
        _grid[x+1, y] &= cell.numRightSide;
        _grid[x-1, y] &= cell.numLeftSide;
    }
}
