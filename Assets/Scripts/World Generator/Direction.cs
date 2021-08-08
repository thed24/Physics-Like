using UnityEngine;

public enum Direction
{
    Forward, Back, Left, Right, Up, Down, None
}

public static class DirectionExtensions
{
    public static Vector3 ModifyVectorBasedOnDirection(Vector3 vector, Direction direction, Vector3 amount)
    {
        switch (direction)
        {
            case Direction.Forward:
                vector.z += amount.z;
                break;
            case Direction.Back:
                vector.z -= amount.z;
                break;
            case Direction.Left:
                vector.x -= amount.x;
                break;
            case Direction.Right:
                vector.x += amount.x;
                break;
            case Direction.Up:
                vector.y += amount.y;
                break;
            case Direction.Down:
                vector.y -= amount.y;
                break;
            default:
                break;
        }
        return vector;
    }
}