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

    public T Find(T element)
    {
        DisjointSetNode<T> node = Find(GetNode(element));

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

    public void Union(T firstElement, T secondElement)
    {
        DisjointSetNode<T> firstElementsRoot = Find(GetNode(firstElement));
        DisjointSetNode<T> secondElementsRoot = Find(GetNode(secondElement));

        if (firstElementsRoot == secondElementsRoot)
        {
            return;
        }

        int comparison = firstElementsRoot.CompareTo(secondElementsRoot);
        if (comparison < 0)
        {
            firstElementsRoot.Parent = secondElementsRoot;
        }
        else if (comparison > 0)
        {
            secondElementsRoot.Parent = firstElementsRoot;
        }
        else
        {
            secondElementsRoot.Parent = firstElementsRoot;
            firstElementsRoot.Rank++;
        }
    }

    public void MakeSet(T element)
    {
        var node = new DisjointSetNode<T>(element);
        disjointSets.Add(element, node);
    }

    private DisjointSetNode<T> GetNode(T element)
    {
        DisjointSetNode<T> node = disjointSets[element];

        if (node == null)
        {
            node = new DisjointSetNode<T>(element);
            disjointSets.Add(element, node);
        }

        return node;
    }
}