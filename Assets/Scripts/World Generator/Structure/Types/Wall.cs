using System.Collections.Generic;
using UnityEngine;

class Wall : Structure
{
    public Wall(Vector3 size, Transform parent, Vector3 position, Material material) : base(GameObject.CreatePrimitive(PrimitiveType.Cube), parent, size, position, material)
    {

    }
}

