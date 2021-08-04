public enum Direction
{
    Up, Down, Left, Right, None
}

public static class DirectionExtensions
{
    public static Vector3 ModifyVectorBasedOnDirection(Vector3 vector, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                vector.z += 10;
                break;
            case Direction.Down:
                vector.z -= 10;
                break;
            case Direction.Left:
                vector.x -= 10;
                break;
            case Direction.Right:
                vector.x += 10;
                break;
            default:
                break;
        }
        return vector;
    }

    public static bool IsDirectionHorizontal(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return false;
            case Direction.Down:
                return false;
            case Direction.Left:
                return true;
            case Direction.Right:
                return true;
            default:
                return false;
        }
    }
}