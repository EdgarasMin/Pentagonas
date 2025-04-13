using Godot;
using System;

public partial class MainMenu : Control
{
	public Music musicScene;

	public GameManageris gameManageris;
   
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

		// Get the autoload GameManager safely
		if (HasNode("/root/GameManageris"))
		{
			gameManageris = GetNode<GameManageris>("/root/GameManageris");
		}
		else
		{
			GD.PrintErr("GameManageris not found in the scene tree!");
			return;
		}

		musicScene = GetNode<Music>("/root/Music");
		musicScene.switchSong("Song1");

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
			button.Pressed += () => AudioManager.PlaySound(ClickingSound.Stream);
		}

		GD.Print("MainMenu initialized.");
	}

  



	public override void _Process(double delta)
	{
	
	   

	}





	private void OnStartNewGamePressed()
	{
		GD.Print("Start Game Button Pressed");

		if (!IsInsideTree())
		{
			GD.PrintErr("MainMenu is not yet added to the scene tree.");
			return;
		}

		if (gameManageris == null)
		{
			GD.PrintErr("GameManageris is null, skipping reset.");
		}
		else
		{
			//gameManageris.ResetProgress();
		}

		GetTree().ChangeSceneToFile("res://scenes/Ivadinis.tscn");
	}


	private void OnContinuePressed()
	{

		
		GD.Print("Continue  Button Pressed");
		GetTree().ChangeSceneToFile(gameManageris.CurrentScene);
		//GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
	}
	
	private void OnOptionsPressed()
	{

		Global.LastScene = "res://scenes/main_menu.tscn";
		
		GetTree().ChangeSceneToFile("res://scenes/Nustatymai.tscn");
		
	}
	

   
	private void OnExitPressed()
	{
		
		GD.Print("Exit Game Button Pressed");
		GetTree().Quit();
	}
	
}
