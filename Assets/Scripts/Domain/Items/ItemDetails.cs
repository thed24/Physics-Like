using System;
using UnityEngine;

 [Serializable]
public class ItemDetails
{
    public string Name;
    public string Description;
    public int Value;
    public int Weight;
    public float Cooldown;
    public float NextUse;
    public Texture2D Icon;
    public GameObject GameObject;
}