using Godot;
using System;
public partial class HeroTemp : AnimatedSprite2D
{
	int speed = 500;
	public Vector2 vel = Vector2.Zero;
	public Music musicScene;
	public AudioStreamPlayer walkSound;
	private GameManageris gameManageris;

	// Reset velocity to zero at the start of each frame




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
		gameManageris = GetNode<GameManageris>("/root/GameManageris");
		gameManageris.SaveCurrentScene();
		walkSound = GetNode<AudioStreamPlayer>("Walking");
		//musicScene = GetNode<Music>("/root/Music");
		//musicScene.switchSong("Song4");
		GD.Print("HeroTemp script is running!");
		Play("idle");
	}
	
	public override void _Process(double delta)
	{
		if (CodeEditing.Instance != null && CodeEditing.Instance.editorShown)
		{
			// If the editor is visible, don't process movement

			return;
		}
		
		
		// Track if the character is moving in this frame
		bool isMoving = false;
		AnimationState newState = currentState;
		
		// Update velocity and animation state based on input
		if (Input.IsActionPressed("Move_L"))
		{
		
			newState = AnimationState.walk_left;
			isMoving = true;
		}
		else if (Input.IsActionPressed("Move_R"))
		{
		
			newState = AnimationState.walk_right;  // Fixed: using correct enum value
			isMoving = true;
		}
		
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
		
		// If not moving, go to idle state
		if (!isMoving)
		{
			walkSound.Stop();
			newState = AnimationState.idle;
		}
		
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
