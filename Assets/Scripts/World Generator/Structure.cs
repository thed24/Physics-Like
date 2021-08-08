using System.Collections.Generic;
using UnityEngine;

public enum StructureType
{
    Room, HorizontalConnector, VerticalConnector, CrossConnector, Staircase
}

public class Structure
{
    private StructureType structureType;
    private Dictionary<Direction, Structure> connectedStructures = new Dictionary<Direction, Structure>();
    private GameObject structureObject;
    public Structure(StructureType structureType, GameObject gameObject)
    {
        this.structureType = structureType;
        this.structureObject = gameObject;
    }

    public void ConnectStructure(Direction direction, Structure structure)
    {
        connectedStructures.Add(direction, structure);
    }

    public Structure GetConnectedStructure(Direction direction)
    {
        return connectedStructures[direction];
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

    public void DeleteStructure(){
        GameObject.Destroy(structureObject);
    }

    public StructureType GetStructureType(){
        return structureType;
    }
}