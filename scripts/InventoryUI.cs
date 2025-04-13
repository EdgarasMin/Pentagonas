using Godot;
using System;

public partial class InventoryUI : Control
{
    public override void _Ready()
    {
        foreach (TextureButton button in GetNode<GridContainer>("GridContainer").GetChildren())
        {
            button.GuiInput += (InputEvent @event) => OnSlotGuiInput(button, @event);

        }
    }

    private TextureButton draggedButton = null;

    private void OnSlotGuiInput(TextureButton button, InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
            {
                if (draggedButton == null)
                {
                    if (button.TextureNormal != null)
                    {
                        draggedButton = button;
                    }
                }
                else
                {
                    if (button != draggedButton)
                    {
                        // Swap the textures
                        var temp = button.TextureNormal;
                        button.TextureNormal = draggedButton.TextureNormal;
                        draggedButton.TextureNormal = temp;

                        draggedButton = null;
                    }
                }
            }
        }
    }

}

