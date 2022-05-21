
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

class PathFinderTests
{
    [Test]
    public void GivenLinearStartAndEndPoints_WhenFindingPath_ReturnLinearPath()
    {
        var expected = new Stack<Node>();
        expected.Push(new Node(new Vector2(1, 1), true, Direction.Up));

        // Given
        var grid = new Grid<Structure>(new Vector3(3, 0, 3));

        // When
        var path = PathFinder.FindPath(new Vector2(1, 0), new Vector2(1, 2), grid);

        // Then
        Assert.AreEqual(expected, path);
    }

    [Test]
    public void GivenDiagonalStartAndEndPoints_WhenFindingPath_ReturnShortestPath()
    {
        var expected = new Stack<Node>();
        expected.Push(new Node(new Vector2(1, 2), true, Direction.Up));
        expected.Push(new Node(new Vector2(0, 2), true, Direction.Right));
        expected.Push(new Node(new Vector2(0, 1), true, Direction.Right));

        // Given
        var grid = new Grid<Structure>(new Vector3(3, 0, 3));

        // When
        var path = PathFinder.FindPath(new Vector2(0, 0), new Vector2(2, 2), grid);

        // Then
        Assert.AreEqual(expected, path);
    }
}