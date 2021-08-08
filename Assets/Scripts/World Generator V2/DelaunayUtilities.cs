using System;
using System.Collections.Generic;
using System.Linq;
public class Vertex
{
    public double x { get; set; }
    public double y { get; set; }
    public Vertex(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }
}

public class Edge
{
    public Vertex v0 { get; set; }
    public Vertex v1 { get; set; }

    public Edge(Vertex v0, Vertex v1)
    {
        this.v0 = v0;
        this.v1 = v1;
    }

    public Edge Inverse()
    {
        return new Edge(v1, v0);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        Edge edge = obj as Edge;
        if (edge == null)
        {
            return false;
        }
        return (this.v0.Equals(edge.v0) && this.v1.Equals(edge.v1)) ||
                    (this.v0.Equals(edge.v1) && this.v1.Equals(edge.v0));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("{0}->{1}", v0, v1);
    }
}

public class Triangle
{
    private Vertex center;
    private double radius;

    public Vertex v0 { get; set; }
    public Vertex v1 { get; set; }
    public Vertex v2 { get; set; }
    public Triangle(Vertex v0, Vertex v1, Vertex v2)
    {
        this.v0 = v0;
        this.v1 = v1;
        this.v2 = v2;

        CalculateCircumcircles();
    }

    public static Triangle CreateSuperTriangle(List<Vertex> verticies)
    {
        var minx = 0d;
        var miny = 0d;
        var maxx = 0d;
        var maxy = 0d;

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
    public void CalculateCircumcircles()
    {
        var A = this.v1.x - this.v0.x;
        var B = this.v1.y - this.v0.y;
        var C = this.v2.x - this.v0.x;
        var D = this.v2.y - this.v0.y;

        var E = A * (this.v0.x + this.v1.x) + B * (this.v0.y + this.v1.y);
        var F = C * (this.v0.x + this.v2.x) + D * (this.v0.y + this.v2.y);

        var G = 2.0 * (A * (this.v2.y - this.v1.y) - B * (this.v2.x - this.v1.x));

        var dx = 0d;
        var dy = 0d;

        if (Math.Round(Math.Abs(G)) == 0)
        {
            var minx = new List<double>() { this.v0.x, this.v1.x, this.v2.x }.Min();
            var miny = new List<double>() { this.v0.y, this.v1.y, this.v2.y }.Min();
            var maxx = new List<double>() { this.v0.x, this.v1.x, this.v2.x }.Max();
            var maxy = new List<double>() { this.v0.y, this.v1.y, this.v2.y }.Max();

            this.center = new Vertex((minx + maxx) / 2, (miny + maxy) / 2);

            dx = this.center.x - minx;
            dy = this.center.y - miny;
        }
        else
        {
            var cx = (D * E - B * F) / G;
            var cy = (A * F - C * E) / G;

            this.center = new Vertex(cx, cy);

            dx = this.center.x - this.v0.x;
            dy = this.center.y - this.v0.y;
        }
        this.radius = Math.Sqrt(dx * dx + dy * dy);
    }

    public bool IsInCircumcircle(Vertex v)
    {
        var dx = this.center.x - v.x;
        var dy = this.center.y - v.y;
        return Math.Sqrt(dx * dx + dy * dy) <= this.radius;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        Triangle triangle = obj as Triangle;
        if (triangle == null)
        {
            return false;
        }
        return (this.v0.Equals(triangle.v0) && this.v1.Equals(triangle.v1) && this.v2.Equals(triangle.v2)) ||
                    (this.v0.Equals(triangle.v1) && this.v1.Equals(triangle.v2) && this.v2.Equals(triangle.v0)) ||
                    (this.v0.Equals(triangle.v2) && this.v1.Equals(triangle.v0) && this.v2.Equals(triangle.v1));
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return string.Format("{0}->{1}->{2}", v0, v1, v2);
    }
}

class DelaunayUtilities
{
    private static List<Triangle> AddVertexToTriangles(Vertex vertex, List<Triangle> triangles)
    {
        triangles = triangles.Where(triangle => triangle.IsInCircumcircle(vertex)).ToList();

        var edges = GetEdgesFrom(triangles);
        edges = RemoveDuplicateEdgesFrom(edges);

        foreach (var edge in edges)
            triangles.Add(new Triangle(edge.v0, edge.v1, vertex));

        return triangles;
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
        var superTriangle = Triangle.CreateSuperTriangle(vertices);
        UnityEngine.Debug.Log("Super Triangle: " + superTriangle.v0 + " " + superTriangle.v1 + " " + superTriangle.v2);

        var triangles = new List<Triangle> { superTriangle };

        foreach (var vertex in vertices)
            triangles = AddVertexToTriangles(vertex, triangles);

        return triangles.Where(triangle => !triangle.Equals(superTriangle)).ToList();
    }
}
