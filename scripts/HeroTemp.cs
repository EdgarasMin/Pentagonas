using Godot;
using System;
public partial class HeroTemp : AnimatedSprite2D
{
	int speed = 500;
	public Vector2 vel = Vector2.Zero;
<<<<<<< HEAD
	public Music musicScene;

	public override void _Ready()
	{
		musicScene = GetNode<Music>("/root/Music");
		musicScene.switchSong("Song4");
		GD.Print("HeroTemp script is running!"); // Debug message
=======
	
	// Define boundaries
	private const float LEFT_BOUND = 0;
	private const float RIGHT_BOUND = 1600;
	private const float TOP_BOUND = 0;
	private const float BOTTOM_BOUND = 950;
	
	// Animation states - fixed the typo in walk_right
	private enum AnimationState
	{
		idle,
		walk_left,
		walk_right,  // Fixed typo: was wal_right
		walk_top,
		walk_bottom
	}
	
	private AnimationState currentState = AnimationState.idle;  // Fixed: lowercase idle
	
	public override void _Ready()
	{
		GD.Print("HeroTemp script is running!");
		Play("idle");
>>>>>>> origin/animacija
	}
	
	public override void _Process(double delta)
	{
		// Reset velocity to zero at the start of each frame
		vel *= 0.55f;
		
		// Track if the character is moving in this frame
		bool isMoving = false;
		AnimationState newState = currentState;
		
		// Update velocity and animation state based on input
		if (Input.IsActionPressed("Move_L"))
		{
			vel.X -= speed;
			newState = AnimationState.walk_left;
			isMoving = true;
		}
		else if (Input.IsActionPressed("Move_R"))
		{
			vel.X += speed;
			newState = AnimationState.walk_right;  // Fixed: using correct enum value
			isMoving = true;
		}
		
		if (Input.IsActionPressed("Move_U"))
		{
			vel.Y -= speed;
			newState = AnimationState.walk_top;
			isMoving = true;
		}
		else if (Input.IsActionPressed("Move_D"))
		{
			vel.Y += speed;  // Fixed: changed -= to += to match movement direction
			newState = AnimationState.walk_bottom;
			isMoving = true;
		}
		
		// If not moving, go to idle state
		if (!isMoving)
		{
			newState = AnimationState.idle;
		}
		
		// Update animation if state changed
		if (newState != currentState)
		{
			currentState = newState;
			UpdateAnimation();
		}
		
		// Apply damping to the velocity
		vel *= 0.8f; // Reduces the velocity by 20% each frame
		
		// Update the position based on the velocity
		Position += vel * (float)delta;
		
		// Clamp position within the screen bounds
		Position = new Vector2(
			Mathf.Clamp(Position.X, LEFT_BOUND, RIGHT_BOUND),
			Mathf.Clamp(Position.Y, TOP_BOUND, BOTTOM_BOUND)
		);
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
