using System;
using UnityEditor;
using UnityEngine;

public class StructureBuilder
{
    public static Structure CreateStructureBasedOnType(StructureType structureType, Vector3 size, Vector3 location)
    {
        var structure = structureType switch 
        {
            StructureType.Room => new Structure(StructureType.Room, LoadPrefab("Assets/Resources/Prefabs/Floor.prefab")),
            StructureType.VerticalConnector => new Structure(StructureType.VerticalConnector, LoadPrefab("Assets/Resources/Prefabs/Vertical Connector.prefab")),
            StructureType.HorizontalConnector => new Structure(StructureType.HorizontalConnector, LoadPrefab("Assets/Resources/Prefabs/Horizontal Connector.prefab")),
            StructureType.Staircase => new Structure(StructureType.Staircase, LoadPrefab("Assets/Resources/Prefabs/Stairs.prefab")),
            _ => null
        };
        structure.SetStructureSize(size);
        structure.SetStructureOrigin(location);

        return structure;
    }

    private static GameObject LoadPrefab(String path){
        UnityEngine.Object originalPrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);
        return PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
    }
}