using Godot;
using System;

public partial class Enemy2 : Area2D
{
	private int y = 0;
	private int x = 0;
	public CharacterBody2D specificCharacter;
	private int direction = -1;
	public bool gameOver = false;
	public AnimatedSprite2D animation;
	
	public override void _Ready()
	{
		y = (int)Position.Y; // Initialize y with current Y position
		x = (int)Position.X;
		this.BodyEntered += OnAreaBodyEntered;
		specificCharacter = GetNode<CharacterBody2D>($"../CharacterBody2D");
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animation.Play("default");
	}

	public override void _Process(double delta)
	{
		x += direction;
		Position = new Vector2(x, y);

		if (x <= -500 || x >= 0) // Reverse direction at bounds
		{
			direction *= -1;
		}
		
		
		
	}
	
	private void OnAreaBodyEntered(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			gameOver = true;
			GD.Print("Lost");

			//GD.Print("Character entered the area!");
		}
	}
}
