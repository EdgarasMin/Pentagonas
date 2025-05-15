using Godot;
using System;

public partial class BoostPickup : Area2D
{
    private bool pickedUp = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (pickedUp) return;

        if (body.IsInGroup("Player"))
        {
            pickedUp = true;

            var inventoryUI = GetTree().CurrentScene.GetNodeOrNull<InventoryUI>("CanvasLayer2");
            if (inventoryUI == null)
            {
                GD.PrintErr("InventoryUI not found!");
                return;
            }

            var speedItem = new InventoryItem
            {
                ItemName = "Speed Pack",
                Icon = GD.Load<Texture2D>("res://assets/Inventorius/energija.png"), // 🔁 use your actual texture path
                ItemType = "boost",
                UseEffect = "speed_boost",
                IsUsable = true,
                Value = 5 // 3 seconds boost
            };

            inventoryUI.AddItemToInventory(speedItem);
            QueueFree();
        }
    }
}
