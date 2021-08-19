using UnityEngine;

public enum StructureType
{
    Room, HorizontalConnector, VerticalConnector, Staircase, None
}

public class Structure
{
    private StructureType structureType;
    private GameObject structureObject;
    public Structure(StructureType structureType, GameObject gameObject)
    {
        this.structureType = structureType;
        this.structureObject = gameObject;
    }

    public Vector3 GetStructureOrigin(){
        return structureObject.transform.localPosition;
    }

    public Vector3 GetStructureSize(){
        return structureObject.transform.localScale;
    }

    public void SetStructureOrigin(Vector3 position){
        structureObject.transform.localPosition = position;
    }

    internal void SetStructureSize(Vector3 scale)
    {
        structureObject.transform.localScale = scale;
    }

    public void DeleteStructure(){
        GameObject.Destroy(structureObject);
    }

    public StructureType GetStructureType(){
        return structureType;
    }
}