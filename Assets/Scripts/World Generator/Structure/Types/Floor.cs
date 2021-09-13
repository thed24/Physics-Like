using System.Collections.Generic;
using UnityEngine;

class Floor : Structure
{
    public Floor(Vector3 size, Transform parent, Vector3 position, Material material) : base(GameObject.CreatePrimitive(PrimitiveType.Cube), parent, size, position, material)
    {

    }

    public List<Structure> CreateWallsFor(IEnumerable<Direction> directions)
    {
        var walls = new List<Structure>();
        foreach (var direction in directions)
        {
            for (int i = 0; i < 8; i++)
            {
                var position = direction switch
                {
                    Direction.Up => new Vector3(Position.x, i, Position.z + Size.z / 2),
                    Direction.Down => new Vector3(Position.x, i, Position.z - Size.z / 2),
                    Direction.Left => new Vector3(Position.x - Size.x / 2, i, Position.z),
                    Direction.Right => new Vector3(Position.x + Size.x / 2, i, Position.z),
                    Direction.None => new Vector3(0, 0, 0),
                    _ => new Vector3(0, 0, 0),
                };
                var scale = direction switch
                {
                    Direction.Left => new Vector3(0.1F, 1, 1F),
                    Direction.Right => new Vector3(0.1F, 1, 1F),
                    Direction.Up => new Vector3(1, 1, 0.1F),
                    Direction.Down => new Vector3(1, 1, 0.1F),
                    Direction.None => new Vector3(0, 0, 0),
                    _ => new Vector3(0, 0, 0),
                };
                var material = structureObject.GetComponent<MeshRenderer>().material;
                walls.Add(new Wall(scale, structureObject.transform.parent, position, material));
            }
        }
        return walls;
    }
}

