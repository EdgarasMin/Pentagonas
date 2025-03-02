using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Godot.Button startNewGame = GetNode<Godot.Button>("VBoxContainer/Button");
		Godot.Button continueGame = GetNode<Godot.Button>("VBoxContainer/Continue");
		Godot.Button options = GetNode<Godot.Button>("VBoxContainer/Options");
		Godot.Button exit = GetNode<Godot.Button>("VBoxContainer/Exit");
		
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
		GD.Print("Options Button Pressed");
	}
	private void OnExitPressed()
	{
		GD.Print("Exit Game Button Pressed");
		GetTree().Quit();
	}
}
