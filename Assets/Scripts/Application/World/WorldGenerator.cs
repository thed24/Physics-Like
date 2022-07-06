using Assets.Scripts.World_Generator;
using System;
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
    public int roomAmount;
    public Material wallMaterial;
    public Material floorMaterial;
    public Material roofMaterial;
    public float enemySpawnChance;

    void Start()
    {
        var roomPoints = GenerateRooms();

        var triangles = GraphUtilities.Triangulate(roomPoints.Select(room => new Vertex(room.x, room.z)).ToList());
        var edges = GraphUtilities.GetEdges(triangles);
        var vertices = edges.Select(edge => edge.v0).Concat(edges.Select(edge => edge.v1)).Distinct().ToList();
        var minimumSpanningTree = GraphUtilities.BuildMinimumSpanningTreeFrom(edges, vertices);
        var minimumSpanningTreeEnriched = minimumSpanningTree.Concat(edges.ToList().GetRange(3, (int)(edges.Count() * 0.04))).Where(e => e.v0.x < worldSize.x && e.v0.y < worldSize.z && e.v1.x < worldSize.x && e.v1.y < worldSize.z && e.v0.x > 0 && e.v0.y > 0 && e.v1.x > 0 && e.v1.y > 0).ToList();

        minimumSpanningTreeEnriched.ForEach(edge => GenerateHallway(edge));
        worldGrid.GetAll().Where(s => s.Position.y == 0).ForEach(s => GenerateWalls(s));

        CombineStructures();
    }

    private List<Vector3> GenerateRooms()
    {
        var listOfPoints = new List<Vector3>();
        var playerSpawned = false;

        worldGrid = new Grid<Structure>(worldSize);
        var enemies = new GameObject("Enemies");
        var items = new GameObject("Items");

        for (var i = 0; i < roomAmount; i++)
        {
            var roomBlueprint = new RoomPlanner(transform, enemies.transform, items.transform)
                .SetPosition(new Vector3(UnityEngine.Random.Range(0, (int)worldSize.x), 0, UnityEngine.Random.Range(0, (int)worldSize.z)))
                .SetSize(new Vector2(UnityEngine.Random.Range(5, 15) * roomSizeModifier, UnityEngine.Random.Range(5, 15) * roomSizeModifier))
                .AddTorches();

            if (playerSpawned)
            {
                roomBlueprint.AddEnemy(enemySpawnChance);
            }

            if (!playerSpawned)
            {
                roomBlueprint.AddPlayerSpawn();
                playerSpawned = true;
            }

            listOfPoints.Add(roomBlueprint.Position);
            MirrorStructure(roomBlueprint.Position, roomBlueprint.Scale);
        }

        return listOfPoints;
    }

    private void GenerateHallway(Edge edge) => PathFinder
            .FindPath(new Vector3((float)edge.v0.x, 0, (float)edge.v0.y), new Vector3((float)edge.v1.x, 0, (float)edge.v1.y), worldGrid)
            .ForEach(node =>
            {
                MirrorStructure(new Vector3(node.Position.x, 0, node.Position.z), new Vector2(1, 1));
            });

    private void GenerateWalls(Structure structure)
    {
        var unoccupiedSpots = worldGrid
            .GetPointsSurrounding(structure.Position)
            .Where(kv => worldGrid[kv.Value] is null)
            .Select(kv => kv.Key).ToList();

        CreateWalls(structure, unoccupiedSpots, (int) worldSize.y, wallMaterial)
            .ForEach(wall => worldGrid[wall.Position] = wall);
    }

    private void MirrorStructure(Vector3 position, Vector2 size)
    {
        var startPosition = new Vector2((float)System.Math.Floor(position.x - (size.x / 2)), (float)System.Math.Floor(position.z - (size.y / 2)));
        var endPosition = new Vector2((float)System.Math.Ceiling(position.x + (size.x / 2)), (float)System.Math.Ceiling(position.z + (size.y / 2)));

        for (var i = (int) startPosition.x; i < endPosition.x; i++)
        {
            for (var j = (int) startPosition.y; j < endPosition.y; j++)
            {
                if (i > 0 && j > 0 && i < worldSize.x && j < worldSize.z)
                {
                    worldGrid[new Vector3(i, 0, j)] = new Structure(transform, new Vector3(i, 0, j), floorMaterial, StructureType.Floor);
                    worldGrid[new Vector3(i, worldSize.y, j)] = new Structure(transform, new Vector3(i, worldSize.y, j), roofMaterial, StructureType.Roof);
                }
            }
        }
    }

    private List<Structure> CreateWalls(Structure structure, IEnumerable<Direction> directions, int height, Material material)
    {
        var walls = new List<Structure>();

        foreach (var direction in directions)
        {
            for (int i = 0; i < height; i++)
            {
                var position = direction switch
                {
                    Direction.Up => new Vector3(structure.Position.x, i, structure.Position.z + 1),
                    Direction.Down => new Vector3(structure.Position.x, i, structure.Position.z - 1),
                    Direction.Left => new Vector3(structure.Position.x - 1, i, structure.Position.z),
                    Direction.Right => new Vector3(structure.Position.x + 1, i, structure.Position.z),
                    Direction.None => new Vector3(0, 0, 0),
                    _ => new Vector3(0, 0, 0),
                };

                walls.Add(new Structure(gameObject.transform, position, material, StructureType.Wall));
            }
        }

        return walls;
    }

    private void CombineStructures()
    {
        var structureTypes = Enum.GetNames(typeof(StructureType));

        foreach(var structureType in structureTypes)
        {
            var parent = new GameObject(structureType);
            parent.AddComponent<MeshFilter>();
            parent.AddComponent<MeshCollider>();
            parent.AddComponent<MeshRenderer>();

            var position = parent.transform.position;
            parent.transform.position = Vector3.zero;

            var objects = GameObject.FindGameObjectsWithTag(structureType);
            var meshFilters = objects.Select(go => go.GetComponent<MeshFilter>()).ToList();
            var combine = new CombineInstance[meshFilters.Count];

            for (int i = 0; i < meshFilters.Count; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
            }

            parent.GetComponent<MeshFilter>().mesh = new Mesh() { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
            parent.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
            parent.GetComponent<MeshCollider>().sharedMesh = parent.transform.GetComponent<MeshFilter>().mesh;
            parent.GetComponent<MeshRenderer>().material = objects[0].GetComponent<MeshRenderer>().material;
            parent.gameObject.SetActive(true);
            parent.transform.position = position;
        }
    }
}
