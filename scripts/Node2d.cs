using Godot;
using System;

public partial class Node2d : Node2D
{
	Texture2D newTexture;
	bool leverArea = false; 
	TextureRect textureRect;
	TextureRect openWall;
	TextureRect lever;
	CollisionShape2D wallColl;
	CharacterBody2D character;
	Area2D pull;
	dials area1;
	dials area2;
	dials area3;
	dials area4;
	dials area5;
	dials area6;
	
	Enemy enemy1;
	Enemy enemy2;
	Enemy enemy3;
	Enemy enemy4;
	Enemy3 enemy5;
	Enemy3 enemy6;
	Enemy2 enemy7;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Load a new texture from the resources folder
		//Texture2D newTexture = GD.Load<Texture2D>("res://icon.svg");
		newTexture = GD.Load<Texture2D>("res://assets/Level4/lever2.png");
		
		// Get the TextureRect node and assign the new texture
		//TextureRect textureRect = GetNode<TextureRect>("TextureRect");
		//textureRect.Texture = newTexture;
		textureRect = GetNode<TextureRect>("ground");
		openWall = GetNode<TextureRect>("walls/TextureRect64");
		wallColl = GetNode<CollisionShape2D>("wallCollisions/CollisionShape2D43");
		lever = GetNode<TextureRect>("lever");
		character = GetNode<CharacterBody2D>("CharacterBody2D");
		pull = GetNode<Area2D>("press");
		pull.BodyEntered += OnBodyEntered;
		pull.BodyExited += OnBodyEntered;
		

		
		area1 = GetNode<dials>("line1");
		area2 = GetNode<dials>("line2");
		area3 = GetNode<dials>("line3");
		area4 = GetNode<dials>("line4");
		area5 = GetNode<dials>("line5");
		area6 = GetNode<dials>("line6");
		//GD.Print(area1.x);
		
		enemy1 = GetNode<Enemy>("enemy1");
		enemy2 = GetNode<Enemy>("enemy2");
		enemy3 = GetNode<Enemy>("enemy3");
		enemy4 = GetNode<Enemy>("enemy4");
		enemy5 = GetNode<Enemy3>("enemy5");
		enemy6 = GetNode<Enemy3>("enemy6");
		enemy7 = GetNode<Enemy2>("enemy7");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (enemy1.gameOver || enemy2.gameOver || enemy3.gameOver || enemy4.gameOver || enemy5.gameOver || enemy6.gameOver || enemy7.gameOver){
			GetTree().ChangeSceneToFile("res://scenes/node_2d.tscn");
		}
			

		if (Input.IsActionJustPressed("B_pressed") && leverArea )
		{
			if (area1.x == 2 && area2.x == 3 && area3.x == 1 && area4.x == 4 && area5.x == 4 && area6.x == 2)
			{
				lever.Texture = newTexture;
				openWall.Texture = null;
				wallColl.Shape = null;
			}
		}
		//GD.Print(area1.x);
		
			// Load a new texture
			//Texture2D newTexture = GD.Load<Texture2D>("res://path/to/your/image.png");
			// Assign it to the TextureRect
			//textureRect.Texture = newTexture;
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body is CharacterBody2D character)
		{
			leverArea = true;
			//GD.Print("Character entered the area: " + character.Name);
		}
	}
	
	private void OnBodyExited(Node body)
	{
		if (body is CharacterBody2D character)
		{
			leverArea = false;
			//GD.Print("Character entered the area: " + character.Name);
		}
	}
	

	
	
}
