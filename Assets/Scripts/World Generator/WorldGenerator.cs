using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    private Grid<Structure> worldGrid;
    public Vector2Int worldSize;
    public int amountOfStructuresInWorld;
    void Start()
    {
        GenerateRoomsWithPlayer();

        var rooms = worldGrid.GetAll().Where(r => r.GetStructureType() == StructureType.Room).ToList();
        var triangles = GraphUtilities.Triangulate(rooms.Select(room => new Vertex(room.GetStructureOrigin().x, room.GetStructureOrigin().z)).ToList());
        var edges = GraphUtilities.GetEdgesFrom(triangles);
        var vertices = edges.Select(edge => edge.v0).Concat(edges.Select(edge => edge.v1)).Distinct().ToList();
        var minimumSpanningTree = GraphUtilities.BuildMinimumSpanningTreeFrom(edges, vertices);
        var minimumSpanningTreeEnriched = minimumSpanningTree.Concat(edges.GetRange(3, (int)(edges.Count() * 0.04))).Where(e => e.v0.x < worldSize.x && e.v0.y < worldSize.y && e.v1.x < worldSize.x && e.v1.y < worldSize.y && e.v0.x > 0 && e.v0.y > 0 && e.v1.x > 0 && e.v1.y > 0).ToList();

        minimumSpanningTreeEnriched.ForEach(edge => GenerateHallwayFrom(edge));
    }

    void Update()
    {

    }

    private void GenerateRoomsWithPlayer()
    {
        var playerSpawned = false;
        worldGrid = new Grid<Structure>(worldSize, Vector2Int.zero);

        for (var i = 0; i < amountOfStructuresInWorld; i++)
        {
            var structure = StructureBuilder.CreateStructureBasedOnType(StructureType.Room, new Vector3(1, 1, 1), new Vector3(Random.Range(0, worldSize.x), 0, Random.Range(0, worldSize.y)));

            if (OtherObjectExistsInArea(structure.GetStructureOrigin(), structure.GetStructureSize()))
                structure.DeleteStructure();
            else {
                if (playerSpawned == false){
                    var player = GameObject.FindGameObjectWithTag("Player");
                    player.transform.position = structure.GetStructureOrigin();
                    playerSpawned = true;
                }
                worldGrid[(int)structure.GetStructureOrigin().x, (int)structure.GetStructureOrigin().z] = structure;
            }
        }
    }

    private void GenerateHallwayFrom(Edge edge)
    {
        var paths = PathFinder.FindPath(new Vector2((float)edge.v0.x, (float)edge.v0.y), new Vector2((float)edge.v1.x, (float)edge.v1.y), worldGrid);
        foreach (var path in paths)
        {
            var structure = StructureBuilder.CreateStructureBasedOnType(StructureType.HorizontalConnector, new Vector3(1, 1, 1), new Vector3(path.Position.x, 0, path.Position.y));
            worldGrid[(int) path.Position.x, (int) path.Position.y] = structure;
        }
    }

    private bool OtherObjectExistsInArea(Vector3 position, Vector3 scale)
    {
        var area = new Rect(position.x - (int)scale.x / 2, position.z - (int)scale.z / 2, (int)scale.x, (int)scale.z);

        var objs = worldGrid.GetAll().Where(o => o is not null && o.GetStructureType() != StructureType.None);
        foreach (var obj in objs)
        {
            var otherArea = new Rect(obj.GetStructureOrigin().x - (int)obj.GetStructureSize().x / 2, obj.GetStructureOrigin().z - (int)obj.GetStructureSize().z / 2, (int)obj.GetStructureSize().x, (int)obj.GetStructureSize().z);

            if (Intersects(area, otherArea))
                return true;
        }
        return false;
    }

    private bool Intersects (Rect rect1, Rect rect2) {
        return !(rect1.xMax < rect2.xMin || rect1.xMin > rect2.xMax || rect1.yMax < rect2.yMin || rect1.yMin > rect2.yMax);
    }
}
