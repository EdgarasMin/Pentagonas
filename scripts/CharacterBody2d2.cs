using Godot;
using System;

public  partial  class CharacterBody2d2 : CharacterBody2D
{
	void Teleport(int x, int y){
		Position = new Vector2(x,y);
	}
	AnimatedSprite2D sprite;
	private bool isSpinning = false;
	private float spinTimeLeft = 3f;
	public float RotationSpeed = 360f; // degrees per second
	public int newScene = 0;
	
	private AudioStreamPlayer soundTP;
	void Collision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision1 = GetSlideCollision(i);
			var collider1 = collision1.GetCollider();

			
			if(collider1 is StaticBody2D staticBody){
			if ( staticBody.Name == "Portal1"||staticBody.Name == "Portal3"||staticBody.Name == "Portal5"||staticBody.Name == "Portal9")
			{
				Teleport(0,475);

				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if ( staticBody.Name == "Portal2")
			{
				Teleport(3236,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (staticBody.Name == "Portal6")
			{
				Teleport(4775,-239);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (staticBody.Name == "Portal4")
			{
				Teleport(4955,455);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (staticBody.Name == "Portal8")
			{
				Teleport(7800,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (staticBody.Name == "Portal7")
			{
				Teleport(6290,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (staticBody.Name == "Portal10")
			{
				//GetTree().ChangeSceneToFile("res://scenes/Level3.tscn");
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
				isSpinning = true;
				AudioManager.PlaySound(soundTP.Stream);
			}
			}
		}
	}
	
	
	public int Speed = 800;

	private Vector2 vel = Vector2.Zero;
	public override void _Ready()
	{
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		soundTP = GetNode<AudioStreamPlayer>("TP");
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaTime = (float)delta;
		if (CodeEditing.Instance != null && CodeEditing.Instance.editorShown)
		{
			// If the editor is visible, don't process movement
			return;
		}
		
		if (isSpinning)
	{
		sprite.Rotation += Mathf.DegToRad(RotationSpeed * deltaTime);
		spinTimeLeft -= deltaTime;

		if (spinTimeLeft <= 0f)
		{
			isSpinning = false;
			spinTimeLeft = 0f;
			GetTree().ChangeSceneToFile("res://scenes/Level3.tscn");

			//GD.Print("Changed scene");
			//sprite.Rotation = 0f; // <-- Reset rotation after spin
		}
	}

		// Dampen velocity slightly
		vel *= 0.55f;

		// Update velocity based on input
		if (Input.IsActionPressed("Move_L"))
		{
			vel.X -= Speed;
		}
		if (Input.IsActionPressed("Move_R"))
		{
			vel.X += Speed;
		}
		if (Input.IsActionPressed("Move_U"))
		{
			vel.Y -= Speed;
		}
		if (Input.IsActionPressed("Move_D"))
		{
			vel.Y += Speed;
		}

		// Apply damping to the velocity
		vel *= 0.8f;

		Collision();

		// Set the built-in velocity and move
		Velocity = vel*32 * (float)delta;
		MoveAndSlide();
	}
	
}
