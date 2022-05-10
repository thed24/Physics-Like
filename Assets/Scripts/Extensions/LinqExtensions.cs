using System.Collections.Generic;

public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
    {
        foreach (T item in list)
            action(item);
    }
}