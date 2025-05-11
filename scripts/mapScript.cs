using Godot;
using System;

public partial class mapScript : CanvasLayer
{
	public bool mapShown = false;
	public override void _Ready()
	{
		ConnectButton("Panel/GridContainer/Button", "res://scenes/Ivadinis.tscn");
		ConnectButton("Panel/GridContainer/Button2", "res://scenes/Level1.tscn");
		ConnectButton("Panel/GridContainer/Button3", "res://scenes/Level2.tscn");
		ConnectButton("Panel/GridContainer/Button4", "res://scenes/Level3.tscn");
	}
	public override void _Process(double delta)
	{
		// Check if the "Code_Editor" action is pressed (bind this in Input Map)
		if (Input.IsActionJustPressed("MapUI"))
		{
			// Toggle the editor visibility and the flag
			mapShown = !mapShown;
			this.Visible = mapShown;
		}
	}

	private void ConnectButton(string nodePath, string scenePath)
	{
		var button = GetNodeOrNull<Button>(nodePath);
		if (button != null)
		{
			button.Pressed += () => ChangeScene(scenePath);
		}
		else
		{
			GD.PrintErr($"Button not found at path: {nodePath}");
		}
	}

	private void ChangeScene(string scenePath)
	{
		var scene = GD.Load<PackedScene>(scenePath);
		if (scene != null)
		{
			GetTree().ChangeSceneToPacked(scene);
		}
		else
		{
			GD.PrintErr($"Failed to load scene at path: {scenePath}");
		}
	}
}
