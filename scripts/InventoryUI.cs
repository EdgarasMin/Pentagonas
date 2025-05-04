using Godot;
using System;

public partial class InventoryUI : CanvasLayer
{
	private Button draggedButton = null;

	private CanvasLayer Inventor;

	public bool inventorShown = false;
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
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				var icon = button.GetNodeOrNull<TextureRect>("ItemIcon");
				if (icon == null)
				{
					GD.PrintErr($"ItemIcon not found in {button.Name}!");
					return;
				}
				if (draggedButton == null)
				{
					// Start dragging if there's an icon
					if (icon.Texture != null)
					{
						draggedButton = button;
					}
				}
				else
				{
					if (button != draggedButton)
					{
						var fromIcon = draggedButton.GetNode<TextureRect>("ItemIcon");
						var toIcon = button.GetNode<TextureRect>("ItemIcon");

						// Swap icons
						var temp = toIcon.Texture;
						toIcon.Texture = fromIcon.Texture;
						fromIcon.Texture = temp;

						draggedButton = null;
					}
				}
			}
		}
	}
}
