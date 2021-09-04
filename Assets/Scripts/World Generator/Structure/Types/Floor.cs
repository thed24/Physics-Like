using System.Collections.Generic;
using UnityEngine;

class Floor : Structure
{
    public Floor(Vector3 size, Vector3 position, Material material) : base(GameObject.CreatePrimitive(PrimitiveType.Cube), size, position, material)
    {

    }

    public void CreateWallsFor(IEnumerable<Direction> directions)
    {
        foreach (var direction in directions)
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.position = direction switch
            {
                Direction.Up => new Vector3(Position.x, 3, Position.z + Size.z / 2),
                Direction.Down => new Vector3(Position.x, 3, Position.z - Size.z / 2),
                Direction.Left => new Vector3(Position.x - Size.x / 2, 3, Position.z),
                Direction.Right => new Vector3(Position.x + Size.x / 2, 3, Position.z),
                Direction.None => new Vector3(0, 0, 0),
                _ => new Vector3(0, 0, 0),
            };
            wall.transform.localScale = direction switch
            {
                Direction.Left => new Vector3(0.1F, 7, 1.1F),
                Direction.Right => new Vector3(0.1F, 7, 1.1F),
                Direction.Up => new Vector3(1, 7, 0.1F),
                Direction.Down => new Vector3(1, 7, 0.1F),
                Direction.None => new Vector3(0, 0, 0),
                _ => new Vector3(0, 0, 0),
            };
            wall.GetComponent<MeshRenderer>().material = structureObject.GetComponent<MeshRenderer>().material;
        }
    }
}

