using Godot;
using System;

public partial class Prisijungimas : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Godot.Button Start = GetNodeOrNull<Godot.Button>("Panel/Button");
		Start.Pressed += StartPressed;
		GD.Print("Login phase");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	
		
	}	
	private void StartPressed()
	{
		GD.Print("Start Game Button Pressed");
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
