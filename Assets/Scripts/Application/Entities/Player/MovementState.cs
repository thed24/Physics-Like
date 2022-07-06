public enum MovementState
{
    Idle,
    Walking,
    Running,
    Jumping
}

public static class MovementStateExtensions
{
    public static float GetMovementModifier(this MovementState movementState)
    {
        switch (movementState)
        {
            case MovementState.Idle:
                return 0f;
            case MovementState.Walking:
                return 1f;
            case MovementState.Running:
                return 1.2f;
            case MovementState.Jumping:
                return 1f;
            default:
                return 1f;
        }
    }
}