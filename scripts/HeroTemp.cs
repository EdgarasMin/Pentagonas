using Godot;

public partial class HeroTemp : Sprite2D
{
	int speed = 300; // Increased speed for better visibility
	public Vector2 vel = Vector2.Zero;

	public override void _Ready()
	{
		GD.Print("HeroTemp script is running!"); // Debug message
	}

	public override void _Process(double delta)
	{
		

		// Reset velocity to zero at the start of each frame
		vel += vel/1.2f;
		vel *= 0.55f ;
		
		// Update velocity based on input
		if (Input.IsActionPressed("Move_L"))
		{
			vel.X -= speed;
			
		}
		if (Input.IsActionPressed("Move_R"))
		{
			vel.X += speed;
			
		}
		if (Input.IsActionPressed("Move_U"))
		{
			vel.Y -= speed;
			
		}
		if (Input.IsActionPressed("Move_D"))
		{
			vel.Y += speed;
			
		}

		// Apply damping to the velocity
		vel *= 0.8f; // This reduces the velocity by 20% each frame

		// Update the position based on the velocity
		Position += vel * (float)delta;

		// Debug the position and velocity
		
	}
}
