using Godot;
using System;

public partial class HealthPickup : Area2D
{
    private bool pickedUp = false;

    public override void _Ready()
    {
        GD.Print("HealthPickup _Ready start");
        BodyEntered += _on_body_entered;
    }

    private void _on_body_entered(Node body)
    {
        GD.Print("Kliuvo kas nors: " + body.Name);

        if (pickedUp) return;

        if (body.IsInGroup("Player"))
        {
            pickedUp = true;

            var inventoryUI = GetTree().CurrentScene.GetNode<InventoryUI>("CanvasLayer2");
            if (inventoryUI == null)
            {
                GD.PrintErr("InventoryUI not found!");
                return;
            }

            var healthItem = new InventoryItem
            {
                ItemName = "Medkit",
                Icon = GD.Load<Texture2D>("res://assets/Inventorius/vaistinele.png"),
                ItemType = "consumable",
                UseEffect = "heal",
                IsUsable = true,
                Value = 50
            };


            inventoryUI.AddItemToInventory(healthItem);
            GD.Print("Health item turėtų būti pridėtas į inventory");

            QueueFree();
        }
    }
}
