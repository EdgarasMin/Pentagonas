using Godot;
using System;

public partial class Area2d : Area2D
{
	
	public CharacterBody2D specificCharacter;
	public AcceptDialog langas = null;
	private void OnAreaBodyEntered(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			
			if (langas != null)
				langas.Show();
			//GD.Print("Character entered the area!");
		}
	}
	
	private void OnAreaBodyExited(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			if (langas.Visible)
			{
				langas.Hide();
			}
			//GD.Print("Character left the area!");
		}
	}
	// Connect signal on _Ready
	public override void _Ready()
	{
		specificCharacter = GetNode<CharacterBody2D>($"../CharacterBody2D");
		langas = GetNode<AcceptDialog>("AcceptDialog");
		// Connect the 'body_entered' signal to the method
		this.BodyEntered += OnAreaBodyEntered;
		this.BodyExited += OnAreaBodyExited;
	}
}
