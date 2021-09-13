using Assets.Scripts.World_Generator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class WorldGenerator : MonoBehaviour
{
    private Grid<Structure> worldGrid;
    public Vector2Int worldSize;
    public int amountOfStructuresInWorld;
    public Material structureMaterial;

    void Start()
    {
        var roomPoints = GenerateRooms();

        var triangles = GraphUtilities.Triangulate(roomPoints.Select(room => new Vertex(room.x, room.z)).ToList());
        var edges = GraphUtilities.GetEdgesFrom(triangles);
        var vertices = edges.Select(edge => edge.v0).Concat(edges.Select(edge => edge.v1)).Distinct().ToList();
        var minimumSpanningTree = GraphUtilities.BuildMinimumSpanningTreeFrom(edges, vertices);
        var minimumSpanningTreeEnriched = minimumSpanningTree.Concat(edges.GetRange(3, (int)(edges.Count() * 0.04))).Where(e => e.v0.x < worldSize.x && e.v0.y < worldSize.y && e.v1.x < worldSize.x && e.v1.y < worldSize.y && e.v0.x > 0 && e.v0.y > 0 && e.v1.x > 0 && e.v1.y > 0).ToList();
 
        minimumSpanningTreeEnriched.ForEach(edge => GenerateHallwayFrom(edge));
        worldGrid.GetAll().ForEach(f => GenerateWalls(f as Floor));

        UnityExtensions.CombineChildMeshesOf(gameObject, structureMaterial);
    }

    void Update()
    {

    }

    private List<Vector3> GenerateRooms()
    {
        var listOfPoints = new List<Vector3>();
        var playerSpawned = false;

        worldGrid = new Grid<Structure>(worldSize);

        for (var i = 0; i < amountOfStructuresInWorld; i++)
        {
            var roomBlueprint = new RoomPlanner()
                .SetPosition(new Vector3(Random.Range(0, worldSize.x), 0, Random.Range(0, worldSize.y)))
                .SetSize(new Vector3(Random.Range(5, 15), 0, Random.Range(5, 15)));

            if (playerSpawned)
            {
                roomBlueprint.AddEnemySpawn();
            }
            else
            {
                roomBlueprint.AddPlayerSpawn();
                playerSpawned = true;
            }

            listOfPoints.Add(roomBlueprint.Position);
            ProjectFloorOntoGridWithCeiling(roomBlueprint.Position, roomBlueprint.Scale);
        }

        return listOfPoints;
    }

    private void GenerateHallwayFrom(Edge edge)
    {
        var paths = PathFinder.FindPath(new Vector2((float)edge.v0.x, (float)edge.v0.y), new Vector2((float)edge.v1.x, (float)edge.v1.y), worldGrid);
        foreach (var path in paths)
        {
            var scale = new Vector3(1, 1, 1);
            var position = new Vector3(path.Position.x, 0, path.Position.y);

            ProjectFloorOntoGridWithCeiling(position, scale);
        }
    }

    private void GenerateWalls(Floor floor)
    {
        var structureMap = worldGrid.GetNodesSurrounding(new Vector2(floor.Position.x, floor.Position.z));
        var walls = floor.CreateWallsFor(structureMap.Where(kv => kv.Value is null).Select(kv => kv.Key).ToList());
        walls.ForEach(wall => worldGrid[wall.Position] = wall);
    }

    private void ProjectFloorOntoGridWithCeiling(Vector3 position, Vector3 size)
    {
        var startPosition = new Vector2((float)System.Math.Floor(position.x - (size.x / 2)), (float)System.Math.Floor(position.z - (size.z / 2)));
        var endPosition = new Vector2((float)System.Math.Ceiling(position.x + (size.x / 2)), (float)System.Math.Ceiling(position.z + (size.z / 2)));

        for (var i = (int)startPosition.x; i < endPosition.x; i++)
        {
            for (var j = (int)startPosition.y; j < endPosition.y; j++)
            {
                if (i > 0 && j > 0 && i < worldSize.x && j < worldSize.y)
                {
                    worldGrid[i, j] = new Floor(new Vector3(1, 1, 1), transform, new Vector3(i, 0, j), structureMaterial);
                    worldGrid[i, j] = new Floor(new Vector3(1, 1, 1), transform, new Vector3(i, 7, j), structureMaterial);
                }
            }
        }
    }
}
