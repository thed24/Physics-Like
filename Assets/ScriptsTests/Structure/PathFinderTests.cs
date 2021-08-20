
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

class PathFinderTests{
    [Test]
    public void GivenLinearStartAndEndPoints_WhenFindingPath_ReturnLinearPath()
    {
        var expected = new Stack<Node>();
        expected.Push(new Node(new Vector2(1, 1), true));

        // Given
        var grid = new Grid<Structure>(new Vector2Int(3, 3), Vector2Int.zero);

        // When
        var path = PathFinder.FindPath(new Vector2(1, 0), new Vector2(1, 2), grid);

        // Then
        Assert.AreEqual(expected, path);
    }

    [Test]
    public void GivenDiagonalStartAndEndPoints_WhenFindingPath_ReturnShortestPath()
    {
        var expected = new Stack<Node>();
        expected.Push(new Node(new Vector2(1, 2), true));
        expected.Push(new Node(new Vector2(0, 2), true));
        expected.Push(new Node(new Vector2(0, 1), true));

        // Given
        var grid = new Grid<Structure>(new Vector2Int(3, 3), Vector2Int.zero);

        // When
        var path = PathFinder.FindPath(new Vector2(0, 0), new Vector2(2, 2), grid);

        // Then
        Assert.AreEqual(expected, path);
    }
}