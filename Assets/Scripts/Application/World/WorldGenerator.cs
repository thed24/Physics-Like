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
    public Vector3 worldSize;
    public float roomSizeModifier;
    public int amountOfStructuresInWorld;
    public Material structureMaterial;
    public float enemySpawnChancePerRoom;

    void Start()
    {
        var roomPoints = GenerateRooms();

        var triangles = GraphUtilities.Triangulate(roomPoints.Select(room => new Vertex(room.x, room.z)).ToList());
        var edges = GraphUtilities.GetEdgesFrom(triangles);
        var vertices = edges.Select(edge => edge.v0).Concat(edges.Select(edge => edge.v1)).Distinct().ToList();
        var minimumSpanningTree = GraphUtilities.BuildMinimumSpanningTreeFrom(edges, vertices);
        var minimumSpanningTreeEnriched = minimumSpanningTree.Concat(edges.GetRange(3, (int)(edges.Count() * 0.04))).Where(e => e.v0.x < worldSize.x && e.v0.y < worldSize.z && e.v1.x < worldSize.x && e.v1.y < worldSize.z && e.v0.x > 0 && e.v0.y > 0 && e.v1.x > 0 && e.v1.y > 0).ToList();

        minimumSpanningTreeEnriched
            .ForEach(edge => GenerateHallwayFrom(edge));

        worldGrid
            .GetAll()
            .Where(s => s.Position.y == 0)
            .ForEach(s => GenerateWalls(s));

        UnityExtensions.CombineChildMeshesOf(gameObject, structureMaterial);
    }

    private List<Vector3> GenerateRooms()
    {
        var listOfPoints = new List<Vector3>();
        var playerSpawned = false;

        worldGrid = new Grid<Structure>(worldSize);

        for (var i = 0; i < amountOfStructuresInWorld; i++)
        {
            var roomBlueprint = new RoomPlanner(transform)
                .SetPosition(new Vector3(Random.Range(0, (int)worldSize.x), 0, Random.Range(0, (int)worldSize.z)))
                .SetSize(new Vector2(Random.Range(5, 15) * roomSizeModifier, Random.Range(5, 15) * roomSizeModifier))
                .AddTorches();

            if (playerSpawned)
            {
                roomBlueprint.AddEnemy(enemySpawnChancePerRoom);
            }

            if (!playerSpawned)
            {
                roomBlueprint.AddPlayerSpawn();
                playerSpawned = true;
            }

            listOfPoints.Add(roomBlueprint.Position);
            MirrorStructureBasedOn(roomBlueprint);
        }

        return listOfPoints;
    }

    private void GenerateHallwayFrom(Edge edge) => PathFinder
            .FindPath(new Vector3((float)edge.v0.x, 0, (float)edge.v0.y), new Vector3((float)edge.v1.x, 0, (float)edge.v1.y), worldGrid)
            .ForEach(node => MirrorStructureBasedOn(new Vector3(node.Position.x, 0, node.Position.z), new Vector2(1, 1)));

    private void GenerateWalls(Structure structure)
    {
        var unoccupiedSpots = worldGrid
            .GetPointsSurrounding(structure.Position)
            .Where(kv => worldGrid[kv.Value] is null)
            .Select(kv => kv.Key).ToList();

        structure
            .CreateWallsFor(unoccupiedSpots, (int)worldSize.y)
            .ForEach(wall => worldGrid[wall.Position] = wall);
    }

    private void MirrorStructureBasedOn(RoomPlanner roomDesign) =>
        MirrorStructureBasedOn(roomDesign.Position, roomDesign.Scale);

    private void MirrorStructureBasedOn(Vector3 position, Vector2 size)
    {
        var startPosition = new Vector2((float)System.Math.Floor(position.x - (size.x / 2)), (float)System.Math.Floor(position.z - (size.y / 2)));
        var endPosition = new Vector2((float)System.Math.Ceiling(position.x + (size.x / 2)), (float)System.Math.Ceiling(position.z + (size.y / 2)));

        for (var i = (int)startPosition.x; i < endPosition.x; i++)
        {
            for (var j = (int)startPosition.y; j < endPosition.y; j++)
            {
                if (i > 0 && j > 0 && i < worldSize.x && j < worldSize.z)
                {
                    worldGrid[new Vector3(i, 0, j)] = new Structure(transform, new Vector3(i, 0, j), structureMaterial);
                    worldGrid[new Vector3(i, worldSize.y, j)] = new Structure(transform, new Vector3(i, worldSize.y, j), structureMaterial);
                }
            }
        }
    }
}
