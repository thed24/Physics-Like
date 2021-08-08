using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private List<Structure> structureMap = new List<Structure>();
    public int structureAmount;
    public int floorAmount;
    
    void Start()
    {
        var currentStaircase = CreateFloorAndGetStaircase(new Vector3(0, 0, 0));

        for (var i = 0; i < floorAmount; i++)
        {
            var nextOrigin = currentStaircase.GetStructureOrigin();
            nextOrigin = DirectionExtensions.ModifyVectorBasedOnDirection(nextOrigin, Direction.Forward, currentStaircase.GetStructureSize());
            nextOrigin = DirectionExtensions.ModifyVectorBasedOnDirection(nextOrigin, Direction.Up, currentStaircase.GetStructureSize());

            currentStaircase = CreateFloorAndGetStaircase(nextOrigin);
        }
    }

    void Update()
    {

    }

    private Structure CreateFloorAndGetStaircase(Vector3 startingOrigin)
    {
        Structure currentRoom = StructureBuilder.CreateStructureBasedOnType(StructureType.Room);
        currentRoom.SetStructureOrigin(startingOrigin);

        for (var i = 1; i <= structureAmount; i++)
        {
            structureMap.Add(currentRoom);

            var nextDirection = getNextRandomDirection();
            var nextStructure = getNextRandomStructure(i, currentRoom, nextDirection);

            if (nextStructure is null)
                continue;

            if (i == structureAmount - 1) {
                nextDirection = Direction.Forward;
                nextStructure = StructureBuilder.CreateStructureBasedOnType(StructureType.Staircase);
            }

            nextStructure.SetStructureOrigin(DirectionExtensions.ModifyVectorBasedOnDirection(currentRoom.GetStructureOrigin(), nextDirection, currentRoom.GetStructureSize()));

            if (structureMap.Any(s => s.GetStructureOrigin() == nextStructure.GetStructureOrigin()) && nextStructure.GetStructureType() != StructureType.Staircase){
                nextStructure.DeleteStructure();
                continue;
            }

            currentRoom.ConnectStructure(nextDirection, nextStructure);
            currentRoom = currentRoom.GetConnectedStructure(nextDirection);
        }

        return currentRoom;
    }

    private Direction getNextRandomDirection()
    {
        return Random.Range(1, 5) switch
        {
            1 => Direction.Forward,
            2 => Direction.Back,
            3 => Direction.Left,
            4 => Direction.Right,
            _ => Direction.None
        };
    }

    private Structure getNextRandomStructure(int id, Structure oldStructure, Direction direction)
    {
        if (direction == Direction.Forward || direction == Direction.Back){
            if (oldStructure.GetStructureType() == StructureType.Room){
                return Random.Range(1, 3) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.HorizontalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
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
                return StructureBuilder.CreateStructureBasedOnType(StructureType.HorizontalConnector);
            }
        } 
        else {
            if (oldStructure.GetStructureType() == StructureType.Room){
                return Random.Range(1, 3) switch
                {
                    1 => StructureBuilder.CreateStructureBasedOnType(StructureType.VerticalConnector),
                    2 => StructureBuilder.CreateStructureBasedOnType(StructureType.Room),
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
                return StructureBuilder.CreateStructureBasedOnType(StructureType.VerticalConnector);
            }
        }

        return null;
    }
}
