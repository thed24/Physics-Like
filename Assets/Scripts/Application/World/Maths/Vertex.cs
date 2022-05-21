using System;
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

    public bool Equals(Vertex other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Vertex && Equals((Vertex)obj);
    }

    public static bool operator <(Vertex first, Vertex second)
    {
        return Math.Abs(first.x + first.y) < Math.Abs(second.x + second.y);
    }

    public static bool operator >(Vertex first, Vertex second)
    {
        return Math.Abs(first.x + first.y) > Math.Abs(second.x + second.y);
    }

    public static bool operator <=(Vertex first, Vertex second)
    {
        return Math.Abs(first.x + first.y) <= Math.Abs(second.x + second.y);
    }

    public static bool operator >=(Vertex first, Vertex second)
    {
        return Math.Abs(first.x + first.y) >= Math.Abs(second.x + second.y);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x.GetHashCode() * 397) ^ y.GetHashCode();
        }
    }
}