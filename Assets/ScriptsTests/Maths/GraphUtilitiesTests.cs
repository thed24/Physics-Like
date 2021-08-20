
using System.Collections.Generic;
using NUnit.Framework;

public class GraphUtilitiesTests
{
    [Test]
    public void GivenSingleTriangle_WhenGetEdgesFromSingleTriangle_ThenAListOfEdgesIsReturned()
    {
        var expected = new List<Edge> {
        new Edge(new Vertex(0, 0), new Vertex(1, 0)),
        new Edge(new Vertex(1, 0), new Vertex(1, 1)),
        new Edge(new Vertex(0, 0), new Vertex(1, 1))
    };

        // Given
        var triangle = new List<Triangle>() {
        new Triangle(new Vertex(0, 0), new Vertex(1, 0), new Vertex(1, 1))
    };

        // When
        var edges = GraphUtilities.GetEdgesFrom(triangle);

        // Then
        Assert.That(edges, Is.EqualTo(expected));
    }
}
