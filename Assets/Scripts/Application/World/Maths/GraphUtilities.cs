using System;
using System.Collections.Generic;
using System.Linq;

public class GraphUtilities
{
    private static List<Triangle> AddVertexToTriangles(Vertex vertex, List<Triangle> triangles)
    {
        var badTriangles = triangles.Where(triangle => triangle.IsInCircumcircle(vertex)).ToList();
        triangles = triangles.Where(triangle => !triangle.IsInCircumcircle(vertex)).ToList();

        var edges = GetEdgesFrom(badTriangles);
        edges = RemoveDuplicateEdgesFrom(edges);

        foreach (var edge in edges)
            triangles.Add(new Triangle(edge.v0, edge.v1, vertex));

        return triangles;
    }

    private static Triangle CreateSuperTriangle(List<Vertex> verticies)
    {
        var minx = (double) int.MaxValue;
        var miny = (double) int.MaxValue;
        var maxx = (double) int.MinValue;
        var maxy = (double) int.MinValue;

        foreach (var vertex in verticies)
        {
            minx = Math.Min(minx, vertex.x);
            miny = Math.Min(minx, vertex.y);
            maxx = Math.Max(maxx, vertex.x);
            maxy = Math.Max(maxx, vertex.y);
        }

        var dx = (maxx - minx) * 10;
        var dy = (maxy - miny) * 10;

        var v0 = new Vertex(minx - dx, miny - dy * 3);
        var v1 = new Vertex(minx - dx, maxy + dy);
        var v2 = new Vertex(maxx + dx * 3, maxy + dy);

        return new Triangle(v0, v1, v2);
    }

    private static List<Edge> RemoveDuplicateEdgesFrom(List<Edge> edges)
    {
        var uniqueEdges = new List<Edge>();

        for (var i = 0; i < edges.Count(); ++i)
        {
            var isUnique = true;

            for (var j = 0; j < edges.Count(); ++j)
            {
                if (i != j && edges.ElementAt(i).Equals(edges.ElementAt(j)))
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
                uniqueEdges.Add(edges.ElementAt(i));
        }

        return uniqueEdges;
    }

    public static List<Edge> GetEdgesFrom(List<Triangle> triangles)
    {
        var edges = new List<Edge>();

        foreach (var triangle in triangles)
        {
            edges.Add(new Edge(triangle.v0, triangle.v1));
            edges.Add(new Edge(triangle.v1, triangle.v2));
            edges.Add(new Edge(triangle.v2, triangle.v0));
        }

        return edges;
    }
    public static List<Triangle> Triangulate(List<Vertex> vertices)
    {
        var superTriangle = CreateSuperTriangle(vertices);

        var triangles = new List<Triangle> { superTriangle };

        foreach (var vertex in vertices)
            triangles = AddVertexToTriangles(vertex, triangles);

        return triangles.Where(triangle => !triangle.Equals(superTriangle)).ToList();
    }

    public static List<Edge> BuildMinimumSpanningTreeFrom(List<Edge> edges, List<Vertex> vertices)
    {
        var result = new List<Edge>();

        var set = new DisjointedSet<Vertex>();

        foreach (var vertex in vertices)
            set.MakeSet(vertex);

        var sortedEdge = edges.OrderBy(x => x.Weight).ToList();

        foreach (var edge in sortedEdge)
        {
            if (set.Find(edge.v0) != set.Find(edge.v1))
            {
                result.Add(edge);
                set.Union(edge.v0, edge.v1);
            }
        }

        return result;
    }
}
