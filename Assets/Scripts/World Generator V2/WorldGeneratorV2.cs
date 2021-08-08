using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGeneratorV2 : MonoBehaviour
{
    private List<GameObject> liveObjects = new List<GameObject>();
    public int structureAmount;

    void Start(){
        for (var i = 0; i < structureAmount; i++)
        {
            var newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObject.transform.localScale = new Vector3(Random.Range(2, 25), Random.Range(2, 25), Random.Range(2, 25));
            newObject.transform.position = new Vector3(Random.Range(5, 100), 0, Random.Range(5, 100));

            liveObjects.Add(newObject);
        }

        var triangles = DelaunayUtilities.Triangulate(liveObjects.Select(o => new Vertex(o.transform.localPosition.x, o.transform.localPosition.z)).ToList());
        var edges = DelaunayUtilities.GetEdgesFrom(triangles);

        VisualizeEdges(edges);
    }

    void Update(){

    }

    private void VisualizeEdges(List<Edge> edges){
        foreach (var edge in edges)
        {
            //For creating line renderer object
            var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;    

            //For drawing line in the world space, provide the x,y,z values
            lineRenderer.SetPosition(0, new Vector3((float) edge.v0.x, 10, (float) edge.v0.y)); //x, y and z position of the starting point of the line
            lineRenderer.SetPosition(1, new Vector3((float) edge.v1.x, 10, (float) edge.v1.y)); //x, y and z position of the end point of the line
        }
    }
}