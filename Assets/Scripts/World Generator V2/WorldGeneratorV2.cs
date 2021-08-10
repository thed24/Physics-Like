using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGeneratorV2 : MonoBehaviour
{
    private List<GameObject> liveObjects = new List<GameObject>();
    public int structureAmount;

    void Start()
    {
        GenerateObjects();

        var triangles = GraphUtilities.Triangulate(liveObjects.Select(o => new Vertex(o.transform.localPosition.x, o.transform.localPosition.z)).ToList());
        var edges = GraphUtilities.GetEdgesFrom(triangles);
        var vertices = edges.Select(e => e.v0).Concat(edges.Select(e => e.v1)).Distinct().ToList();
        var minimumSpanningTree = GraphUtilities.BuildMinimumSpanningTreeFrom(edges, vertices);

        UnityEngine.Debug.Log(string.Format("Triangles: {0}", triangles.Count));
        UnityEngine.Debug.Log(string.Format("Edges: {0}", edges.Count));
        UnityEngine.Debug.Log(string.Format("Vertices: {0}", vertices.Count));
        UnityEngine.Debug.Log(string.Format("Minimum spanning tree: {0}", minimumSpanningTree.Count));

        Visualize(minimumSpanningTree);
    }

    void Update()
    {

    }

    private void GenerateObjects()
    {
        for (var i = 0; i < structureAmount; i++)
        {
            var newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObject.transform.localScale = new Vector3(Random.Range(2, 25), Random.Range(2, 25), Random.Range(2, 25));
            newObject.transform.position = new Vector3(Random.Range(5, 100), 0, Random.Range(5, 100));

            if (OtherObjectExistsInArea(newObject.transform.position, newObject.transform.localScale))
            {
                Destroy(newObject);
            }
            else
            {
                liveObjects.Add(newObject);
            }
        }
    }

    private bool OtherObjectExistsInArea(Vector3 position, Vector3 scale)
    {
        foreach (var obj in liveObjects)
        {
            if (obj.transform.localPosition.x > position.x - scale.x && obj.transform.localPosition.x < position.x + scale.x &&
                obj.transform.localPosition.z > position.z - scale.z && obj.transform.localPosition.z < position.z + scale.z)
            {
                return true;
            }
        }
        return false;
    }

    private void Visualize(List<Vertex> vertices)
    {
        foreach (var vertex in vertices)
        {
            var newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newObject.transform.localScale = new Vector3(5, 5, 5);
            newObject.transform.position = new Vector3((float)vertex.x, 0, (float)vertex.y);
        }
    }

    private void Visualize(List<Edge> edges)
    {
        foreach (var edge in edges)
        {
            var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPosition(0, new Vector3((float)edge.v0.x, 25, (float)edge.v0.y));
            lineRenderer.SetPosition(1, new Vector3((float)edge.v1.x, 25, (float)edge.v1.y));
        }
    }
}