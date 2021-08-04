using System;
using UnityEditor;
using UnityEngine;

public class StructureBuilder
{
    public static Structure CreateStructureBasedOnType(StructureType structureType){
        return structureType switch 
        {
            StructureType.Room => new Structure(StructureType.Room, LoadPrefab("Assets/Resources/Prefabs/Floor.prefab")),
            StructureType.VerticalConnector => new Structure(StructureType.VerticalConnector, LoadPrefab("Assets/Resources/Prefabs/Vertical Connector.prefab")),
            StructureType.HorizontalConnector => new Structure(StructureType.HorizontalConnector, LoadPrefab("Assets/Resources/Prefabs/Horizontal Connector.prefab")),
            StructureType.CrossConnector => new Structure(StructureType.CrossConnector, LoadPrefab("Assets/Resources/Prefabs/Cross Connector.prefab")),
            _ => null
        };
    }

    private static GameObject LoadPrefab(String path){
        UnityEngine.Object originalPrefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);
        return PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;
    }
}