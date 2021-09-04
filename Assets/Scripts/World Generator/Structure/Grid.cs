using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    readonly T[] data;
    public Vector2Int GridSize { get; private set; }

    public Grid(Vector2Int size)
    {
        GridSize = size;

        data = new T[size.x * size.y];
    }

    public int GetIndex(Vector2Int pos)
    {
        return pos.x + (GridSize.x * pos.y);
    }

    public Dictionary<Direction, T> GetNodesSurrounding(Vector2 position)
    {
        var mapOfNodes = new Dictionary<Direction, T>
        {
            [Direction.Up] = this[new Vector2(position.x, position.y + 1)],
            [Direction.Down] = this[new Vector2(position.x, position.y - 1)],
            [Direction.Right] = this[new Vector2(position.x + 1, position.y)],
            [Direction.Left] = this[new Vector2(position.x - 1, position.y)],
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

    public T this[int x, int y]
    {
        get
        {
            return this[new Vector2Int(x, y)];
        }
        set
        {
            this[new Vector2Int(x, y)] = value;
        }
    }

    public T this[Vector2 pos]
    {
        get
        {
            if (GetIndex(Vector2Int.RoundToInt(pos)) < data.Length && GetIndex(Vector2Int.RoundToInt(pos)) > 0)
                return data[GetIndex(Vector2Int.RoundToInt(pos))];
            else
                return default;
        }
        set
        {
            data[GetIndex(Vector2Int.RoundToInt(pos))] = value;
        }
    }

    public T this[Vector3 pos]
    {
        get
        {
            if (GetIndex(new Vector2Int((int)pos.x, (int)pos.y)) < data.Length && GetIndex(new Vector2Int((int)pos.x, (int)pos.y)) > 0)
                return data[GetIndex(new Vector2Int((int)pos.x, (int)pos.y))];
            else
                return default;
        }
        set
        {
            data[GetIndex(new Vector2Int((int)pos.x, (int)pos.y))] = value;
        }
    }

    public T this[Vector2Int pos]
    {
        get
        {
            if (GetIndex(pos) < data.Length && GetIndex(pos) > 0)
                return data[GetIndex(pos)];
            else
                return default;
        }
        set
        {
            data[GetIndex(pos)] = value;
        }
    }
}