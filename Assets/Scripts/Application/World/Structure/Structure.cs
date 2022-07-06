using UnityEngine;

public class Structure
{
    public StructureType Type { get; set; }

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

    public Structure(Transform parent, Vector3 position, Material material, StructureType type)
    {
        structureObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        structureObject.transform.parent = parent;
        structureObject.GetComponent<MeshRenderer>().material = material;
        structureObject.transform.position = position;
        structureObject.tag = type.ToTag();

        Type = type;
    }
}
