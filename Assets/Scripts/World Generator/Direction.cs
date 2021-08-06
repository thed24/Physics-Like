using UnityEngine;

public enum Direction
{
    Forward, Back, Left, Right, Up, Down, None
}

public static class DirectionExtensions
{
    public static Vector3 ModifyVectorBasedOnDirection(Vector3 vector, Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                vector.z += 10;
                break;
            case Direction.Back:
                vector.z -= 10;
                break;
            case Direction.Left:
                vector.x -= 10;
                break;
            case Direction.Right:
                vector.x += 10;
                break;
            case Direction.Up:
                vector.y += 10;
                break;
            case Direction.Down:
                vector.y -= 10;
                break;
            default:
                break;
        }
        return vector;
    }
}