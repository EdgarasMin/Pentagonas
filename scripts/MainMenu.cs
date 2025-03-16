using Godot;
using System;

public partial class MainMenu : Control
{
	public Music musicScene;
private void PlayClickingSound()
	{
		
		ClickingSound.Play();
		GD.Print("Click sound was played");
	}
	private void PlayHoveringSound(Button button)
	{
		HoveringSound.Play();
		GD.Print("Hovering sound was played");
	}
	private AudioStreamPlayer ClickingSound;
	private	AudioStreamPlayer HoveringSound;
	public override void _Ready()
	{
	
		musicScene = GetNode<Music>("/root/Music");
		musicScene.switchSong("Song3");
		ClickingSound = GetNode<AudioStreamPlayer>("ClickingSound");
		HoveringSound = GetNode<AudioStreamPlayer>("HoveringSound");
		Button startNewGame = GetNode<Button>("VBoxContainer/Button");
		Button continueGame = GetNode<Button>("VBoxContainer/Continue");
		Button options = GetNode<Button>("VBoxContainer/Options");
		Button exit = GetNode<Button>("VBoxContainer/Exit");
		
		startNewGame.Pressed += OnStartNewGamePressed;
		continueGame.Pressed += OnContinuePressed;
		options.Pressed += OnOptionsPressed;
		exit.Pressed += OnExitPressed;
		 foreach (Button button in GetTree().GetNodesInGroup("MenuButtons"))
		{
			button.MouseEntered += () => PlayHoveringSound(button);
		}
	}
	
	
	public override void _Process(double delta)
	{
	
	   

	}

  
	
   
	
	private void OnStartNewGamePressed()
	{
		
		GD.Print("Start Game Button Pressed");
		AudioManager.PlaySound(ClickingSound.Stream);
		//PlayClickingSound();
		GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
		
		
	}
	
	private void OnContinuePressed()
	{
		AudioManager.PlaySound(ClickingSound.Stream);
		//PlayClickingSound();
		GD.Print("Continue  Button Pressed");
	}
	
	private void OnOptionsPressed()
	{

		Global.LastScene = "res://scenes/main_menu.tscn";
		AudioManager.PlaySound(ClickingSound.Stream);
		//PlayClickingSound();
		GetTree().ChangeSceneToFile("res://scenes/Nustatymai.tscn");
		
	}
	
	
	
	
   
	private void OnExitPressed()
	{
		
		GD.Print("Exit Game Button Pressed");
		AudioManager.PlaySound(ClickingSound.Stream);
		//PlayClickingSound();
		GetTree().Quit();
	}
	
}
