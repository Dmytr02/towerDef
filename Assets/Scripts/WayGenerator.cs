using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayGenerator : MonoBehaviour
{
    public static List<Vector2Int> GenerateWay(int[,] map, Vector2Int start, Vector2Int target, List<Vector2Int> neighbors, int distanceCost = 1)
    {
        int[,] weight = new int[map.GetLength(0), map.GetLength(1)];
        Vector2Int[,] directions = new Vector2Int[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < weight.GetLength(0); x++)
        {
            for (int y = 0; y < weight.GetLength(1); y++)
            {
                weight[x, y] = 10000;
            }
        }

        weight[start.x, start.y] = map[start.x, start.y];
        List<Vector2Int> checkList = new List<Vector2Int>() { start };

        while (checkList.Count > 0)
        {
            Vector2Int check = checkList[0];
            checkList.RemoveAt(0);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (check.x + neighbor.x < 0 || check.y + neighbor.y < 0 || check.x + neighbor.x > map.GetLength(0) - 1 || check.y + neighbor.y > map.GetLength(1) - 1)
                    continue;
                if (weight[check.x + neighbor.x, check.y + neighbor.y] <= weight[check.x, check.y] + map[check.x + neighbor.x, check.y + neighbor.y])
                    continue;
                if (map[check.x + neighbor.x, check.y + neighbor.y] == -1)
                    continue;
                directions[check.x + neighbor.x, check.y + neighbor.y] = new Vector2Int(-neighbor.x, -neighbor.y);
                if (check.x + neighbor.x == target.x && check.y + neighbor.y == target.y)
                {
                    List<Vector2Int> way = new List<Vector2Int>();
                    Vector2Int pos = target;
                    while (pos.x != start.x || pos.y != start.y)
                    {
                        way.Add(pos);
                        pos = new Vector2Int(directions[pos.x, pos.y].x + pos.x, directions[pos.x, pos.y].y + pos.y);
                    }
                    way.Add(pos);
                    return way;
                }



                weight[check.x + neighbor.x, check.y + neighbor.y] = weight[check.x, check.y] + map[check.x + neighbor.x, check.y + neighbor.y];

                int Count = checkList.Count;
                for (int i = 0; i < Count + 1; i++)
                {
                    if (i == Count)
                    {
                        checkList.Add(new Vector2Int(check.x + neighbor.x, check.y + neighbor.y));
                    }
                    else if (weight[check.x + neighbor.x, check.y + neighbor.y] + (Mathf.Abs(target.x - check.x - neighbor.x) + Mathf.Abs(target.y - check.y - neighbor.y)) * distanceCost < weight[checkList[i].x, checkList[i].y] + (Mathf.Abs(target.x - checkList[i].x) + Mathf.Abs(target.y - checkList[i].y)) * distanceCost)
                    {
                        checkList.Insert(i,
                            new Vector2Int(check.x + neighbor.x, check.y + neighbor.y));
                        break;
                    }
                }
            }
        }

        return new List<Vector2Int>();
    }
}
