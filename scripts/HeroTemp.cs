using Godot;
using System;

public partial class HeroTemp : AnimatedSprite2D
{
	// Movement variables
	int speed = 500;
	public Vector2 vel = Vector2.Zero;
	
	// Audio references
	public Music musicScene;
	public AudioStreamPlayer walkSound;
	
	// Game state references
	private GameManageris gameManageris;
	
	// Combat variables
	bool enemy_inattack_range = false;
	bool enemy_attack_cooldown = true;
	int health = 100;
	bool player_alive = true;

	// Animation states
	private enum AnimationState
	{
		idle,
		walk_left,
		walk_right,
		walk_top,
		walk_bottom
	}
	
	private AnimationState currentState = AnimationState.idle;
	
	public override void _Ready()
	{
		gameManageris = GetNode<GameManageris>("/root/GameManageris");
		gameManageris.SaveCurrentScene();
		walkSound = GetNode<AudioStreamPlayer>("Walking");
		GD.Print("HeroTemp script is running!");
		Play("idle");
	}
	
	public override void _Process(double delta)
	{
		var code = GetNode<CanvasLayer>("../../CanvasLayer"); 
		if (code.Visible)
		{
			// If editor UI is visible, don't process movement
			return;
		}
		
		// Movement and animation handling
		bool isMoving = false;
		AnimationState newState = currentState;
		
		// Horizontal movement
		if (Input.IsActionPressed("Move_L"))
		{
			newState = AnimationState.walk_left;
			isMoving = true;
		}
		else if (Input.IsActionPressed("Move_R"))
		{
			newState = AnimationState.walk_right;
			isMoving = true;
		}
		
		// Vertical movement
		if (Input.IsActionPressed("Move_U"))
		{
			newState = AnimationState.walk_top;
			isMoving = true;
		}
		else if (Input.IsActionPressed("Move_D"))
		{
			newState = AnimationState.walk_bottom;
			isMoving = true;
		}
		
		// Handle idle state
		if (!isMoving)
		{
			walkSound.Stop();
			newState = AnimationState.idle;
		}
		
		// Play walking sound if moving
		if (isMoving && !walkSound.Playing)
		{
			walkSound.Play();
		}
		
		// Update animation if state changed
		if (newState != currentState)
		{
			currentState = newState;
			UpdateAnimation();
		}
	}
	
	private void UpdateAnimation()
	{
		// Play the appropriate animation based on current state
		switch (currentState)
		{
			case AnimationState.idle:
				Play("idle");
				break;
			case AnimationState.walk_left:
				Play("walk_left");
				break;
			case AnimationState.walk_right:
				Play("walk_right");
				break;
			case AnimationState.walk_top:
				Play("walk_top");
				break;
			case AnimationState.walk_bottom:
				Play("walk_bottom");
				break;
		}
	}

	

	
}
