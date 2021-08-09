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