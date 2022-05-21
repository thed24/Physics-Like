using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Direction DirectionFromParent;
    public Node Parent;
    public Vector3 Position;
    public float DistanceToTarget;
    public float Cost;
    public float Weight;
    public float Heuristics => (DistanceToTarget != -1 && Cost != -1) ? DistanceToTarget + Cost : -1;
    public bool Walkable;

    public Node(Vector3 posistion, bool walkable, Direction direction, float weight = 1)
    {
        Parent = null;
        Position = posistion;
        DistanceToTarget = -1;
        Cost = 1;
        Weight = weight;
        Walkable = walkable;
        DirectionFromParent = direction;
    }

    public override string ToString()
    {
        return $"{Position.x}, {Position.y}, {Walkable}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Node)obj;
        return other.Position == Position;
    }

    public override int GetHashCode()
    {
        return (int)(Position.GetHashCode() * Weight);
    }
}

public static class PathFinder
{
    public static Stack<Node> FindPath(Vector3 start, Vector3 end, Grid<Structure> grid)
    {
        var startingNode = new Node(start, true, Direction.None);
        var lastNode = new Node(end, true, Direction.None);

        var Path = new Stack<Node>();
        var uncheckedNodes = new List<Node>();
        var nodesInPath = new List<Node>();

        var current = startingNode;
        uncheckedNodes.Add(startingNode);

        while (uncheckedNodes.Count != 0 && !nodesInPath.Exists(x => x.Position == end))
        {
            current = uncheckedNodes[0];
            uncheckedNodes.Remove(current);
            nodesInPath.Add(current);

            var adjacencies = GetAdjacentNodes(current, grid);
            foreach (var adjacentNode in adjacencies)
            {
                if (!nodesInPath.Contains(adjacentNode) && !uncheckedNodes.Contains(adjacentNode))
                {
                    adjacentNode.Parent = current;
                    adjacentNode.DistanceToTarget = Math.Abs(adjacentNode.Position.x - lastNode.Position.x) + Math.Abs(adjacentNode.Position.z - lastNode.Position.z);
                    adjacentNode.Cost = adjacentNode.Weight + adjacentNode.Parent.Cost;
                    uncheckedNodes.Add(adjacentNode);
                }
            }
            uncheckedNodes = uncheckedNodes.OrderBy(node => node.Heuristics).ToList();
        }

        if (!nodesInPath.Exists(x => x.Position == end))
            return null;

        var temp = nodesInPath[nodesInPath.IndexOf(current)];
        if (temp is null)
            return null;

        while (temp != startingNode && temp is not null)
        {
            if (temp.Walkable && temp.Position != end)
                Path.Push(temp);
            temp = temp.Parent;
        };

        return Path;
    }

    private static List<Node> GetAdjacentNodes(Node node, Grid<Structure> grid)
    {
        var adjacentNodes = new List<Node>();
        var adjacentPositionsAndDirections = grid.GetPointsSurrounding(node.Position);

        foreach (var positionAndDirection in adjacentPositionsAndDirections)
        {
            var neighbouringNodeDirection = positionAndDirection.Key;
            var neighbouringNodePosition = positionAndDirection.Value;
            adjacentNodes.Add(new Node(neighbouringNodePosition, grid[neighbouringNodePosition] is null, neighbouringNodeDirection));
        }

        return adjacentNodes;
    }
}
