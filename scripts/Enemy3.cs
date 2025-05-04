using System;
using Godot;

public partial class Enemy3 : Area2D
{
	private int y = 0;
	private int x = 0;
	public CharacterBody2D specificCharacter;
	private int direction = 1; // Start moving to the right
	public bool gameOver = false;
	public AnimatedSprite2D animation;

	public override void _Ready()
	{
		y = (int)Position.Y;
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

		if (x >= 500 || x <= 0) // Reverse direction at horizontal bounds
		{
			direction *= -1;
		}
	}

	private void OnAreaBodyEntered(Node body)
	{
		if (body == specificCharacter)
		{
			gameOver = true;
			GD.Print("Lost");
		}
	}
}
