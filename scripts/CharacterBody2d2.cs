using Godot;
using System;

public  partial  class CharacterBody2d2 : CharacterBody2D
{
	void Teleport(int x, int y){
		Position = new Vector2(x,y);
	}
	void Collision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision1 = GetSlideCollision(i);
			var collider1 = collision1.GetCollider();

			
			
			if (collider1 is StaticBody2D staticBody1 && (staticBody1.Name == "Portal1"||staticBody1.Name == "Portal3"||staticBody1.Name == "Portal5"||staticBody1.Name == "Portal9"))
			{
				Teleport(0,475);

				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D staticBody0 && staticBody0.Name == "Portal2")
			{
				Teleport(3236,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D staticBody2 && staticBody2.Name == "Portal6")
			{
				Teleport(3236,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D  staticBody3 && staticBody3.Name == "Portal4")
			{
				Teleport(4775,-239);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D  staticBody4 && staticBody4.Name == "Portal8")
			{
				Teleport(7800,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D  staticBody5 && staticBody5.Name == "Portal7")
			{
				Teleport(6290,475);
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D  staticBody6 && staticBody6.Name == "Portal10")
			{
				GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
		}
	}
	
	
	public int Speed = 800;

	private Vector2 vel = Vector2.Zero;
	

	public override void _PhysicsProcess(double delta)
	{
		
		if (CodeEditing.Instance != null && CodeEditing.Instance.editorShown)
		{
			// If the editor is visible, don't process movement
			return;
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
