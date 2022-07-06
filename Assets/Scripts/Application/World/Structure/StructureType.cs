using System;

public enum StructureType
{
    Wall,
    Floor,
    Roof
}

public static class StructureTypeExtensions
{
    public static string ToTag(this StructureType structureType)
    {
        return structureType switch
        {
            StructureType.Wall => "Wall",
            StructureType.Floor => "Floor",
            StructureType.Roof => "Roof",
            _ => throw new ArgumentException("Unknown StructureType")
        };
    }
}