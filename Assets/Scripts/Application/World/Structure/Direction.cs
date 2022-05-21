using UnityEngine;

public enum Direction
{
    Up, Down, Left, Right, None
}

public static class DirectionExtensions
{
    public static Vector3 ToVector3(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector3.forward,
            Direction.Down => Vector3.back,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            _ => Vector3.zero,
        };
    }
}