using System;
using System.Collections.Generic;
using System.Linq;

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
