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

			
			
			if (collider1 is StaticBody2D staticBody1 && staticBody1.Name == "StaticBody2D2")
			{
				Teleport(0,0);

				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider1 is StaticBody2D staticBody0 && staticBody0.Name == "X")
			{
				Teleport(0,0);

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
