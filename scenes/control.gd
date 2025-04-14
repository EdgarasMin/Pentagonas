using Godot;
using System;

public partial class Pause : Control
{
	public PanelContainer panel;
	public PanelContainer panel2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button resume = GetNode<Button>("PanelContainer/VBoxContainer/Resume");
		Button options = GetNode<Button>("PanelContainer/VBoxContainer/Options");
		Button mainMenu = GetNode<Button>("PanelContainer/VBoxContainer/mainMenu");
		panel = GetNode<PanelContainer>("PanelContainer");
		panel2 = GetNode<PanelContainer>("PanelContainer2");
		resume.Pressed += OnResumePressed;
		options.Pressed += OnOptionsPressed;
		mainMenu.Pressed += OnMainMenuPressed;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Escape"))
		{
			if (!panel.IsVisibleInTree())
			{
				//this.Show();
				panel.Show();
				GD.Print("showinmg");
				GetTree().Paused = true;
			}
				
			else if (panel.IsVisibleInTree())
			{
				//this.Hide();
				panel.Hide();
				GD.Print("hiding");
				GetTree().Paused = false;
			}
				
			
		}
		
	}
	
	private void OnResumePressed()
	{
		panel.Hide();
		GetTree().Paused = false;
	}
	
	private void OnOptionsPressed()
	{
		//GetTree().Paused = false;
		//Global.LastScene = "res://scenes/HeroScene.tscn";
		//GetTree().ChangeSceneToFile("res://scenes/Nustatymai.tscn");
		panel.Hide();
		panel2.Show();
	}
	
	private void OnMainMenuPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
