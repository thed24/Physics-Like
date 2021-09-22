using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    readonly T[,,] data;
    public Vector3 GridSize { get; private set; }

    public Grid(Vector3 size)
    {
        GridSize = size;

        data = new T[(int)size.x, (int)size.y, (int)size.z];
    }

    public Dictionary<Direction, Vector3> GetPointsSurrounding(Vector3 position)
    {
        var mapOfNodes = new Dictionary<Direction, Vector3>
        {
            [Direction.Up] = new Vector3(position.x, position.y, position.z + 1),
            [Direction.Down] = new Vector3(position.x, position.y, position.z - 1),
            [Direction.Right] = new Vector3(position.x + 1, position.y, position.z),
            [Direction.Left] = new Vector3(position.x - 1, position.y, position.z),
        };
        return mapOfNodes;
    }

    public List<T> GetAll(){
        var all = new List<T>();
        foreach (var item in data)
        {
            if (item is not null)
                all.Add(item);
        }
        return all;
    }
    public T this[Vector3 pos]
    {
        get
        {
            if (pos.x < 0 || pos.y < 0 || pos.z < 0 || pos.x >= GridSize.x || pos.y >= GridSize.y || pos.z >= GridSize.z){
                return default;
            }
            return data[(int)pos.x, (int)pos.y, (int)pos.z];
        }
        set
        {
            if (pos.x < 0 || pos.y < 0 || pos.z < 0 || pos.x >= GridSize.x || pos.y >= GridSize.y || pos.z >= GridSize.z){
                return;
            }
            data[(int)pos.x, (int)pos.y, (int)pos.z] = value;
        }
    }
}