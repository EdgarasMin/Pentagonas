using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button startNewGame = GetNode<Button>("VBoxContainer/Button");
		Button continueGame = GetNode<Button>("VBoxContainer/Continue");
		Button options = GetNode<Button>("VBoxContainer/Options");
		Button exit = GetNode<Button>("VBoxContainer/Exit");
		
		startNewGame.Pressed += OnStartNewGamePressed;
		continueGame.Pressed += OnContinuePressed;
		options.Pressed += OnOptionsPressed;
		exit.Pressed += OnExitPressed;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	private void OnStartNewGamePressed()
	{
		GD.Print("Start Game Button Pressed");
		GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
	}
	
	private void OnContinuePressed()
	{
		GD.Print("Continue  Button Pressed");
	}
	private void OnOptionsPressed()
	{
<<<<<<< HEAD
		GD.Print("Options Button Pressed");
=======
		Global.LastScene = "res://scenes/main_menu.tscn";
		GetTree().ChangeSceneToFile("res://scenes/Nustatymai.tscn");
>>>>>>> 1Spr
	}
	private void OnExitPressed()
	{
		GD.Print("Exit Game Button Pressed");
		GetTree().Quit();
	}
}
