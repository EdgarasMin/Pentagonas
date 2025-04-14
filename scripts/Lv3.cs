using Godot;
using System;

public  partial  class Lv3 : CharacterBody2D
{
	private float bPressedCooldown = 0.5f; // Cooldown time in seconds
	private float bPressedTimer = 0.0f;
	void Teleport(int x, int y){
		Position = new Vector2(x,y);
	}
	void Collision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision1 = GetSlideCollision(i);
			var collider1 = collision1.GetCollider();

			 
			
			if (collider1 is StaticBody2D staticBody )
			{
				var hidden1 = GetNode<CanvasGroup>("../Hidden1");
				var hidden2 = GetNode<CanvasGroup>("../Hidden2");
				var hidden3 = GetNode<CanvasGroup>("../Hidden3");
				var hidden4 = GetNode<CanvasGroup>("../Hidden4");
				var hidden5 = GetNode<CanvasGroup>("../Hidden5");
				var hidden6 = GetNode<CanvasGroup>("../Hidden6");
				if(hidden2.Visible && (!hidden3.Visible || !hidden4.Visible || !hidden5.Visible || !hidden6.Visible)){
					hidden3.Visible=true;
					hidden4.Visible=true;
					hidden5.Visible=true;
					hidden6.Visible=true;
				}
				if(staticBody.Name == "B1" && Input.IsActionPressed("B_pressed") 
				&& bPressedTimer <= 0)
				{	
					GD.Print("B1!");
				 	var hidden = GetNode<CanvasGroup>("../Hidden1"); 
					hidden.Visible = !hidden.Visible;
					bPressedTimer = bPressedCooldown;
					hidden2.Visible=true;
					hidden3.Visible=true;
					hidden4.Visible=true;
					hidden5.Visible=true;
					hidden6.Visible=true;
				}
				
				
				if(!hidden1.Visible && staticBody.Name == "B2" && Input.IsActionPressed("B_pressed") 
				&& bPressedTimer <= 0)
				{
					
					
				 	var hidden = GetNode<CanvasGroup>("../Hidden2"); 
					hidden.Visible = !hidden.Visible;
					bPressedTimer = bPressedCooldown;
					hidden3.Visible=true;
					hidden4.Visible=true;
					hidden5.Visible=true;
					hidden6.Visible=true;
				}
				
				if(!hidden2.Visible &&(staticBody.Name == "B4" || staticBody.Name == "B5")&& Input.IsActionPressed("B_pressed") 
				&& bPressedTimer <= 0)
				{
				 	var hidden = GetNode<CanvasGroup>("../Hidden3"); 
					hidden.Visible = !hidden.Visible;
					bPressedTimer = bPressedCooldown;
					hidden4.Visible=true;
					hidden5.Visible=true;
					hidden6.Visible=true;
				}
				
				if(!hidden3.Visible &&staticBody.Name == "B3" && Input.IsActionPressed("B_pressed") 
				&& bPressedTimer <= 0)
				{
				 	var hidden = GetNode<CanvasGroup>("../Hidden4"); 
					hidden.Visible = !hidden.Visible;
					bPressedTimer = bPressedCooldown;
				}
				
				if(!hidden3.Visible &&staticBody.Name == "B6" && Input.IsActionPressed("B_pressed") 
				&& bPressedTimer <= 0)
				{
				 	var hidden = GetNode<CanvasGroup>("../Hidden5"); 
					hidden.Visible = !hidden.Visible;
					bPressedTimer = bPressedCooldown;
				}
				
				if(!hidden3.Visible &&staticBody.Name == "B7" && Input.IsActionPressed("B_pressed") 
				&& bPressedTimer <= 0)
				{
				 	var hidden = GetNode<CanvasGroup>("../Hidden6"); 
					hidden.Visible = !hidden.Visible;
					bPressedTimer = bPressedCooldown;
				}
				
			}
		}
	}
	
	
	public int Speed = 800;

	private Vector2 vel = Vector2.Zero;
	

	public override void _PhysicsProcess(double delta)
	{
		// Decrease the timer each frame
		if (bPressedTimer > 0)
		{
			bPressedTimer -= (float)delta;
		}
		
		var code = GetNode<CanvasLayer>("../CanvasLayer"); 
		if (code.Visible)
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
