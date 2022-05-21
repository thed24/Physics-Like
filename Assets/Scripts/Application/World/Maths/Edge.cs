using System;

public class Edge
{
    public Vertex v0 { get; set; }
    public Vertex v1 { get; set; }
    public int Weight { get; set; }

    public Edge(Vertex v0, Vertex v1)
    {
        this.v0 = v0;
        this.v1 = v1;
        Weight = (int)Math.Sqrt(Math.Pow(v0.x - v1.x, 2) + Math.Pow(v0.y - v1.y, 2));
    }

    public Edge GetInverseEdge()
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
