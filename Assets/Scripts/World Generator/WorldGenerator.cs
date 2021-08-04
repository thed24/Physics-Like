using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private List<Structure> structureMap = new List<Structure>();
    public int structureAmount;
    
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {

    }

    private void GenerateMap()
    {
        Structure currentRoom = StructureBuilder.CreateStructureBasedOnType(StructureType.Room);

        for (var i = 1; i < structureAmount; i++)
        {
            structureMap.Add(currentRoom);

            var nextDirection = getNextRandomDirection();
            var nextStructure = getNextRandomStructure(i, currentRoom, nextDirection);
            nextStructure.SetStructureOrigin(DirectionExtensions.ModifyVectorBasedOnDirection(currentRoom.GetStructureOrigin(), nextDirection));

            if (structureMap.Any(s => s.GetStructureOrigin() == nextStructure.GetStructureOrigin())){
                nextStructure.DeleteStructure();
                continue;
            }

            currentRoom.ConnectStructure(nextDirection, nextStructure);
            currentRoom = currentRoom.GetConnectedStructure(nextDirection);
        }
    }

    private Direction getNextRandomDirection()
    {
        return Random.Range(1, 5) switch
        {
            1 => Direction.Up,
            2 => Direction.Down,
            3 => Direction.Left,
            4 => Direction.Right,
            _ => Direction.None
        };
    }

    private Structure getNextRandomStructure(int id, Structure oldStructure, Direction direction)
    {
        if (direction == Direction.Up || direction == Direction.Down){
            if (oldStructure.GetStructureType() == StructureType.Room){
                return Random.Range(1, 4) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.HorizontalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector),
                    3 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
                    _ => null
                };
            }

            if (oldStructure.GetStructureType() == StructureType.HorizontalConnector){
                return Random.Range(1, 4) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.HorizontalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector),
                    3 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
                    _ => null
                };
            }

            if (oldStructure.GetStructureType() == StructureType.CrossConnector){
                return Random.Range(1, 4) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.HorizontalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector),
                    3 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
                    _ => null
                };
            }
        } 
        else {
            if (oldStructure.GetStructureType() == StructureType.Room){
                return Random.Range(1, 4) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.VerticalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector),
                    3 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
                    _ => null
                };
            }

            if (oldStructure.GetStructureType() == StructureType.VerticalConnector){
                return Random.Range(1, 4) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.VerticalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector),
                    3 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
                    _ => null
                };
            }

            if (oldStructure.GetStructureType() == StructureType.CrossConnector){
                return Random.Range(1, 4) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.VerticalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector),
                    3 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
                    _ => null
                };
            }
        }

        return StructureBuilder.CreateStructureBasedOnType(StructureType.CrossConnector);
    }
}
