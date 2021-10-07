using UnityEngine;

public class ItemDetails
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
    public int Weight { get; set; }
    public Texture2D Icon { get; set;}
    public GameObject GameObject { get; }
}