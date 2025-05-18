using Godot;
using System;

public partial class KeyPickup : Area2D
{
    private bool pickedUp = false; // ← add this at class level

    public override void _Ready()
    {
        GD.Print("KeyPickup _Ready start");
        BodyEntered += _on_body_entered;
    }

    private void _on_body_entered(Node body)
    {
        if (pickedUp) return; // ← stop duplicates

        if (body.IsInGroup("Player"))
        {
            pickedUp = true; // ← lock this

            var inventoryUI = GetTree().CurrentScene.GetNode<InventoryUI>("CanvasLayer2");
            if (inventoryUI == null)
            {
                GD.PrintErr("InventoryUI not found!");
                return;
            }

            var keyItem = new InventoryItem
            {
                ItemName = "Golden Key",
                Icon = GD.Load<Texture2D>("res://assets/Inventorius/raktas.png"),
                ItemType = "key",
                UseEffect = "unlock_door",
                IsUsable = true
            };

            inventoryUI.AddItemToInventory(keyItem);
            QueueFree(); // safely destroy after 1 run
        }
    }
}
