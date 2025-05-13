using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryUI : CanvasLayer
{
	private Button draggedButton = null;

	private CanvasLayer Inventor;

	public bool inventorShown = false;

    private Dictionary<Button, InventoryItem> slotItems = new();
    public override void _Ready()
	{
		var grid = GetNodeOrNull<GridContainer>("Panel/GridContainer");
		if (grid == null)
		{
			//GD.Print("Gridas neveikia");
		}
		else
		{
			//GD.Print("Gridas Veikia");
			foreach (Button button in grid.GetChildren())
			{
				button.GuiInput += (InputEvent @event) => OnSlotGuiInput(button, @event);
				//GD.Print("paeme mygtukus");
			}
		}
        GD.Print("InventoryUI ready and running!");


    }
	public override void _Process(double delta)
	{
		// Check if the "Code_Editor" action is pressed (bind this in Input Map)
		if (Input.IsActionJustPressed("InventorUI"))
		{
			// Toggle the editor visibility and the flag
			inventorShown = !inventorShown;
			this.Visible = inventorShown;
		}
	}

    private void OnSlotGuiInput(Button button, InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.DoubleClick)
            {
                // Double click detected
                if (slotItems.TryGetValue(button, out var item))
                {
                    GD.Print($"Using item: {item.ItemName}");
                    UseItem(item);
                    slotItems.Remove(button); // Optional: remove after use
                    var icon = button.GetNodeOrNull<TextureRect>("ItemIcon");
                    if (icon != null)
                        icon.Texture = null;
                }
            }
            else if (mouseEvent.Pressed) // Single click logic for dragging
            {
                var icon = button.GetNodeOrNull<TextureRect>("ItemIcon");
                if (icon == null) return;

                if (draggedButton == null && icon.Texture != null)
                {
                    draggedButton = button;
                }
                else if (button != draggedButton)
                {
                    var fromIcon = draggedButton.GetNode<TextureRect>("ItemIcon");
                    var toIcon = button.GetNode<TextureRect>("ItemIcon");

                    var temp = toIcon.Texture;
                    toIcon.Texture = fromIcon.Texture;
                    fromIcon.Texture = temp;

                    // Swap internal data too
                    var tempItem = slotItems[draggedButton];
                    slotItems[draggedButton] = slotItems[button];
                    slotItems[button] = tempItem;

                    draggedButton = null;
                }
            }
        }
    }

    public void AddItemToInventory(InventoryItem item)
    {
        var grid = GetNode<GridContainer>("Panel/GridContainer");
		GD.Print("buvo bandyta prideti daikta");
        foreach (Button button in grid.GetChildren())
        {
            var icon = button.GetNode<TextureRect>("ItemIcon");
            if (icon.Texture == null)
            {
                icon.Texture = item.Icon;
                slotItems[button] = item;
                break;
            }
        }
    }
    private void UseItem(InventoryItem item)
    {
        if (item == null || !item.IsUsable) return;

        // Find the player by group, not by class
        var players = GetTree().GetNodesInGroup("Player");
        if (players.Count == 0) return;

        var player = players[0]; // Assuming only one player

        switch (item.UseEffect)
        {
            case "unlock_door":
                if (player.HasMethod("TryUnlockDoor"))
                {
                    player.Call("TryUnlockDoor");
                }
                else
                {
                    GD.PrintErr("Player does not implement TryUnlockDoor()");
                }
                break;

            case "heal":
                if (player is Node playerNode)
                {
                    var health = (int)playerNode.Get("Health");
                    var maxHealth = (int)playerNode.Get("MaxHealth");

                    playerNode.Set("Health", Mathf.Min(health + item.Value, maxHealth));
                    playerNode.Call("EmitSignal", "HealthChanged", playerNode.Get("Health"), maxHealth);
                }
                break;

            case "speed_boost":
                if (player.HasMethod("ApplySpeedBoost"))
                {
                    player.Call("ApplySpeedBoost", item.Value);
                }
                else
                {
                    GD.PrintErr("Player does not implement ApplySpeedBoost()");
                }
                break;

            default:
                GD.Print("Unknown item effect: " + item.UseEffect);
                break;
        }
    }



}
