using System;
using System.Collections.Generic;
class DisjointSetNode<T> : IComparable<DisjointSetNode<T>>
{
    public T Element { get; set; }
    public DisjointSetNode<T> Parent { get; set; }
    public int Rank { get; set; } = 0;

    public DisjointSetNode(T element)
    {
        this.Element = element;
        this.Parent = this;
    }

    public int CompareTo(DisjointSetNode<T> other)
    {
        return Rank.CompareTo(other.Rank);
    }
}

public class DisjointedSet<T>
{
    private Dictionary<T, DisjointSetNode<T>> disjointSets = new Dictionary<T, DisjointSetNode<T>>();

    public T Find(T e)
    {
        DisjointSetNode<T> node = Find(GetNode(e));

        if (node == node.Parent)
        {
            return node.Element;
        }

        node.Parent = Find(node.Parent);

        return node.Parent.Element;
    }

    private DisjointSetNode<T> Find(DisjointSetNode<T> node)
    {
        if (node == node.Parent)
            return node;

        return Find(node.Parent);
    }

    public void Union(T e1, T e2)
    {
        DisjointSetNode<T> e1Root = Find(GetNode(e1));
        DisjointSetNode<T> e2Root = Find(GetNode(e2));

        if (e1Root == e2Root)
        {
            return;
        }

        int comparison = e1Root.CompareTo(e2Root);
        if (comparison < 0)
        {
            e1Root.Parent = e2Root;
        }
        else if (comparison > 0)
        {
            e2Root.Parent = e1Root;
        }
        else
        {
            e2Root.Parent = e1Root;
            e1Root.Rank++;
        }
    }

    public void MakeSet(T e)
    {
        var node = new DisjointSetNode<T>(e);
        disjointSets.Add(e, node);
    }

    private DisjointSetNode<T> GetNode(T e)
    {
        DisjointSetNode<T> node = disjointSets[e];

        if (node == null)
        {
            node = new DisjointSetNode<T>(e);
            disjointSets.Add(e, node);
        }

        return node;
    }
}