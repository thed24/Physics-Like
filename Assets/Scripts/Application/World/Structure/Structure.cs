using System.Collections.Generic;
using UnityEngine;

public class Structure
{
    protected readonly GameObject structureObject;
    public Vector3 Size 
    { 
        get { return structureObject.transform.localScale; } 
        set { structureObject.transform.localScale = value; } 
    }
    public Vector3 Position
    {
        get { return structureObject.transform.position; }
        set { structureObject.transform.position = value; }
    }
    public T GetComponentFor<T>() => structureObject.GetComponent<T>();
    public Structure(Transform parent, Vector3? position = null, Material material = null)
    {
        structureObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        structureObject.transform.parent = parent;
        structureObject.tag = GetType().ToString();
        structureObject.GetComponent<MeshRenderer>().material = material is not null ? material : structureObject.GetComponent<MeshRenderer>().material;
        structureObject.transform.position = position ?? structureObject.transform.position;
    }
    public List<Structure> CreateWallsFor(IEnumerable<Direction> directions, int height)
    {
        var walls = new List<Structure>();
        var material = structureObject.GetComponent<MeshRenderer>().material;

        foreach (var direction in directions)
        {
            for (int i = 0; i < height; i++)
            {
                var position = direction switch
                {
                    Direction.Up => new Vector3(Position.x, i, Position.z + 1),
                    Direction.Down => new Vector3(Position.x, i, Position.z - 1),
                    Direction.Left => new Vector3(Position.x - 1, i, Position.z),
                    Direction.Right => new Vector3(Position.x + 1, i, Position.z),
                    Direction.None => new Vector3(0, 0, 0),
                    _ => new Vector3(0, 0, 0),
                };
                walls.Add(new Structure(structureObject.transform.parent, position, material));
            }
        }
        return walls;
    }
}