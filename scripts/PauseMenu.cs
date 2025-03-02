using Godot;
using System;

public partial class PauseMenu : Window
{
	bool windowShown = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Escape"))
		{
			windowShown = !windowShown;
			GetTree().Paused = windowShown;
			this.Visible = windowShown;
			
		}
	}
	
	public void _on_resume_pressed()
	{
		windowShown = !windowShown;
		GetTree().Paused = windowShown;
		this.Visible = windowShown;
	}
	
	public void _on_options_pressed()
	{
		windowShown = !windowShown;
		GetTree().Paused = windowShown;
		Global.LastScene = "res://scenes/HeroScene.tscn";
		GetTree().ChangeSceneToFile("res://scenes/Nustatymai.tscn");
	}
	
	public void _on_main_menu_pressed()
	{
		windowShown = !windowShown;
		GetTree().Paused = windowShown;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
