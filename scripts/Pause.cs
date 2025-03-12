using Godot;
using System;

public partial class Pause : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button resume = GetNode<Button>("PanelContainer/VBoxContainer/Resume");
		Button options = GetNode<Button>("PanelContainer/VBoxContainer/Options");
		Button mainMenu = GetNode<Button>("PanelContainer/VBoxContainer/mainMenu");
		
		resume.Pressed += OnResumePressed;
		options.Pressed += OnOptionsPressed;
		mainMenu.Pressed += OnMainMenuPressed;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Escape"))
		{
			if (!this.IsVisibleInTree())
			{
				this.Show();
				GetTree().Paused = true;
			}
				
			else if (this.IsVisibleInTree())
			{
				this.Hide();
				GetTree().Paused = false;
			}
				
			
		}
		
	}
	
	private void OnResumePressed()
	{
		this.Hide();
		GetTree().Paused = false;
	}
	
	private void OnOptionsPressed()
	{
		GetTree().Paused = false;
		Global.LastScene = "res://scenes/HeroScene.tscn";
		GetTree().ChangeSceneToFile("res://scenes/Nustatymai.tscn");
	}
	
	private void OnMainMenuPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
