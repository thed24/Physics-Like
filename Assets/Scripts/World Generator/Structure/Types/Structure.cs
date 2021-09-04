using System;
using UnityEngine;

public abstract class Structure
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
    public Structure(GameObject gameObject, Vector3? size = null, Vector3? position = null, Material material = null)
    {
        structureObject = gameObject;
        structureObject.GetComponent<MeshRenderer>().material = material is not null ? material : structureObject.GetComponent<MeshRenderer>().material;
        structureObject.transform.position = position.HasValue ? position.Value : structureObject.transform.position;
        structureObject.transform.localScale = size.HasValue ? size.Value : structureObject.transform.localScale;
    }
    public Structure()
    {
        structureObject = null;
    }
}