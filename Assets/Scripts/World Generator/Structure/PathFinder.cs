using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public Node Parent;
    public Vector2 Position;
    public float DistanceToTarget;
    public float Cost;
    public float Weight;
    public float Heuristics => (DistanceToTarget != -1 && Cost != -1) ? DistanceToTarget + Cost : -1;
    public bool Walkable;

    public Node(Vector2 posistion, bool walkable, float weight = 1)
    {
        Parent = null;
        Position = posistion;
        DistanceToTarget = -1;
        Cost = 1;
        Weight = weight;
        Walkable = walkable;
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
    public static Stack<Node> FindPath(Vector2 start, Vector2 end, Grid<Structure> grid)
    {
        Node startingNode = new Node(start, true);
        Node lastNode = new Node(end, true);

        Stack<Node> Path = new Stack<Node>();
        List<Node> uncheckedNodes = new List<Node>();
        List<Node> nodesInPath = new List<Node>();

        Node current = startingNode;
        uncheckedNodes.Add(startingNode);

        while (uncheckedNodes.Count != 0 && !nodesInPath.Exists(x => x.Position == lastNode.Position))
        {
            current = uncheckedNodes[0];
            uncheckedNodes.Remove(current);
            nodesInPath.Add(current);

            List<Node> adjacencies = GetAdjacentNodes(current, grid);
            foreach (Node adjacentNode in adjacencies)
            {
                if (!nodesInPath.Contains(adjacentNode) && !uncheckedNodes.Contains(adjacentNode))
                {
                    adjacentNode.Parent = current;
                    adjacentNode.DistanceToTarget = Math.Abs(adjacentNode.Position.x - lastNode.Position.x) + Math.Abs(adjacentNode.Position.y - lastNode.Position.y);
                    adjacentNode.Cost = adjacentNode.Weight + adjacentNode.Parent.Cost;
                    uncheckedNodes.Add(adjacentNode);
                }
            }
            uncheckedNodes = uncheckedNodes.OrderBy(node => node.Heuristics).ToList<Node>();
        }

        if (!nodesInPath.Exists(x => x.Position == lastNode.Position)) 
            return null;

        Node temp = nodesInPath[nodesInPath.IndexOf(current)];
        if (temp is null) 
            return null;

        while (temp != startingNode && temp is not null)
        {
            if (temp.Walkable)
                Path.Push(temp);
            temp = temp.Parent;
        };

        return Path;
    }

    private static List<Node> GetAdjacentNodes(Node node, Grid<Structure> grid)
    {
        List<Node> adjacentNodes = new List<Node>();

        int row = (int) node.Position.y;
        int column = (int) node.Position.x;

        if (row + 1 < grid.Size.x)
        {
            var adjacentPosition = new Vector2Int(column, row + 1);
            adjacentNodes.Add(new Node(adjacentPosition, grid[adjacentPosition].GetStructureType() == StructureType.None));
        }
        if (row - 1 >= 0)
        {
            var adjacentPosition = new Vector2Int(column, row - 1);
            adjacentNodes.Add(new Node(adjacentPosition, grid[adjacentPosition].GetStructureType() == StructureType.None));
        }
        if (column - 1 >= 0)
        {
            var adjacentPosition = new Vector2Int(column - 1, row);
            adjacentNodes.Add(new Node(adjacentPosition, grid[adjacentPosition].GetStructureType() == StructureType.None));
        }
        if (column + 1 < grid.Size.y)
        {
            var adjacentPosition = new Vector2Int(column + 1, row);
            adjacentNodes.Add(new Node(adjacentPosition, grid[adjacentPosition].GetStructureType() == StructureType.None));
        }

        return adjacentNodes;
    }
}
