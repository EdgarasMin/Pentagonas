using Godot;
using System;

public partial class portalas4lygio : Area2D
{
	
	public CharacterBody2D specificCharacter;
	//public AcceptDialog langas = null;
	private void OnAreaBodyEntered(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			//if (langas != null)
				//langas.Show();
			//GD.Print("Character entered the area!");
			
			//GetTree().ChangeSceneToFile("res://scenes/Level1.tscn");
			CallDeferred("changeScene");
			
		}
	}
	private void changeScene()
	{
		var stopwatchLabel = GetTree().Root.FindChild("stopwatch_label", true, false);
		if (stopwatchLabel != null && stopwatchLabel is StopwatchLabel stopwatch)
		{
			//stopwatch.Stop();
		}
		GetTree().ChangeSceneToFile("res://scenes/rekordai.tscn");
	}
	
	private void OnAreaBodyExited(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			GD.Print("Character left the area!");
		}
	}
	// Connect signal on _Ready
	public override void _Ready()
	{
		
		specificCharacter = GetNode<CharacterBody2D>($"../CharacterBody2D");
		//langas = GetNode<AcceptDialog>("AcceptDialog");
		// Connect the 'body_entered' signal to the method
		this.BodyEntered += OnAreaBodyEntered;
		this.BodyExited += OnAreaBodyExited;
	}
}
