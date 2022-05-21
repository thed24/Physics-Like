using UnityEngine;

public enum Hand
{
    Left,
    Right
}

public static class HandExtensions
{
    public static Hand Opposite(this Hand hand)
    {
        return hand == Hand.Left ? Hand.Right : Hand.Left;
    }

    public static int GetIndex(this Hand hand)
    {
        return (int)hand;
    }

    public static KeyCode GetDropKey(this Hand hand)
    {
        return hand == Hand.Left ? KeyCode.Q : KeyCode.R;
    }
}