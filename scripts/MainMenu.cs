using Godot;
using System;

public partial class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    private AudioStreamPlayer ClickingSound;
    private AudioStreamPlayer HoveringSound;
    public override void _Ready()

	{
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

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    private void PlayClickingSound()
    {
        //if (ClickingSound != null && !ClickingSound.Playing)
        //{

        //}
        ClickingSound.Play();
        GD.Print("Click sound was played");
    }
    private void PlayHoveringSound(Button button)
    {
        HoveringSound.Play();
        GD.Print("Hovering sound was played");
    }
    public override void _Process(double delta)
	{
		
	}
	
	private void OnStartNewGamePressed()
	{
		GD.Print("Start Game Button Pressed");
        PlayClickingSound();
        GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
    }
    
    private void OnContinuePressed()
    {
        PlayClickingSound();
        GD.Print("Continue  Button Pressed");
    }
    private void OnOptionsPressed()
    {
        PlayClickingSound();
        GD.Print("Options Button Pressed");
    }
    private void OnExitPressed()
	{
        PlayClickingSound();
        GD.Print("Exit Game Button Pressed");
		GetTree().Quit();
	}
}
