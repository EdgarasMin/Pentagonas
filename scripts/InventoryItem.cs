using Godot;
using System;

public partial class InventoryItem : Resource
{
    public string ItemName { get; set; } = "";
    public Texture2D Icon { get; set; } = null;
    public string ItemType { get; set; } = ""; // e.g. "key", "potion", "weapon"
    public bool IsUsable { get; set; } = true;
    public string UseEffect { get; set; } = ""; // e.g. "unlock_door", "heal", etc.
    public int Value { get; set; } = 0;
}
