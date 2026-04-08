using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};
public class WayGenerator : MonoBehaviour
{
    public static List<Pair<int, int>> GenerateWay(int[,] map, Pair<int, int> start, Pair<int, int> target, List<Pair<int, int>> neighbors, int distanceCost = 1)
    {
        int[,] weight = new int[map.GetLength(0), map.GetLength(1)];
        Pair<int, int>[,] directions = new Pair<int, int>[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < weight.GetLength(0); x++)
        {
            for (int y = 0; y < weight.GetLength(1); y++)
            {
                weight[x, y] = 10000;
            }
        }

        weight[start.First, start.Second] = map[start.First, start.Second];
        List<Pair<int, int>> checkList = new List<Pair<int, int>>() { start };

        while (checkList.Count > 0)
        {
            Pair<int, int> check = checkList[0];
            checkList.RemoveAt(0);
            foreach (Pair<int, int> neighbor in neighbors)
            {
                if (check.First + neighbor.First < 0 || check.Second + neighbor.Second < 0 || check.First + neighbor.First > map.GetLength(0) - 1 || check.Second + neighbor.Second > map.GetLength(1) - 1)
                    continue;
                if (weight[check.First + neighbor.First, check.Second + neighbor.Second] <= weight[check.First, check.Second] + map[check.First + neighbor.First, check.Second + neighbor.Second])
                    continue;
                if (map[check.First + neighbor.First, check.Second + neighbor.Second] == -1)
                    continue;
                directions[check.First + neighbor.First, check.Second + neighbor.Second] = new Pair<int, int>(-neighbor.First, -neighbor.Second);
                if (check.First + neighbor.First == target.First && check.Second + neighbor.Second == target.Second)
                {
                    List<Pair<int, int>> way = new List<Pair<int, int>>();
                    Pair<int, int> pos = target;
                    while (pos.First != start.First || pos.Second != start.Second)
                    {
                        way.Add(pos);
                        pos = new Pair<int, int>(directions[pos.First, pos.Second].First + pos.First, directions[pos.First, pos.Second].Second + pos.Second);
                    }
                    way.Add(pos);
                    return way;
                }



                weight[check.First + neighbor.First, check.Second + neighbor.Second] = weight[check.First, check.Second] + map[check.First + neighbor.First, check.Second + neighbor.Second];

                int Count = checkList.Count;
                for (int i = 0; i < Count + 1; i++)
                {
                    if (i == Count)
                    {
                        checkList.Add(new Pair<int, int>(check.First + neighbor.First, check.Second + neighbor.Second));
                    }
                    else if (weight[check.First + neighbor.First, check.Second + neighbor.Second] + (Mathf.Abs(target.First - check.First - neighbor.First) + Mathf.Abs(target.Second - check.Second - neighbor.Second)) * distanceCost < weight[checkList[i].First, checkList[i].Second] + (Mathf.Abs(target.First - checkList[i].First) + Mathf.Abs(target.Second - checkList[i].Second)) * distanceCost)
                    {
                        checkList.Insert(i,
                            new Pair<int, int>(check.First + neighbor.First, check.Second + neighbor.Second));
                        break;
                    }
                }
            }
        }

        return new List<Pair<int, int>>();
    }
}
