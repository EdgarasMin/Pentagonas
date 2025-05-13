using Godot;
using System;

public partial class Lv3 : CharacterBody2D
{
	// Original properties
	private float bPressedCooldown = 0.5f; // Cooldown time in seconds
	private float bPressedTimer = 0.0f;
	public int Speed = 800;
	private Vector2 vel = Vector2.Zero;
	
	// Combat properties
	public int Health = 100;
	public int MaxHealth = 100;
	public int MinHealth = 0;
	public int AttackDamage = 20;  
	private float attackCooldown = 0.5f; // Attack cooldown in seconds
	private float attackTimer = 0.0f;
	private bool isInvulnerable = false;
	private float invulnerabilityTime = 0.5f;
	private float invulnerabilityTimer = 0.0f;
	
	// Animation properties
	private AnimatedSprite2D animatedSprite;
	private bool facingRight = true;
	private bool isAttacking = false;

    private AudioStreamPlayer attackSound;

    // Signal for UI updates if needed
    [Signal]
	public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);
	
	public override void _Ready()
	{
		//Lv3Task1.ExitTree();
		// Any initialization code here
		EmitSignal(SignalName.HealthChanged, Health, MaxHealth);
		//EmitSignal(SignalName.HealthChanged, Health, MaxHealth);
	
		// Add player to the Player group
		AddToGroup("Player");
		
		// Get the animated sprite node
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (animatedSprite == null)
		{
			GD.Print("AnimatedSprite2D not found! Make sure to add it to your player scene.");
		}

        attackSound = GetNode<AudioStreamPlayer>("AttackSoundPlayer");

    }

    void Teleport(int x, int y)
	{
		Position = new Vector2(x, y);
	}
	
	// Add take damage method for enemy attacks
	public void take_damage(int amount)
	{
		if (isInvulnerable)
			return;
			
		Health -= amount;
		
		// Play hit animation or flash effect here if desired
		Modulate = new Color(1, 0.3f, 0.3f, 1); // Flash red
		
		EmitSignal(SignalName.HealthChanged, Health, MaxHealth);
		
		if (Health <= 0)
		{
			Die();
		}
		else
		{
			// Start invulnerability
			isInvulnerable = true;
			invulnerabilityTimer = invulnerabilityTime;
		}
	}
	
	private async void Die()
{
	// Handle player death
	GD.Print("Player died!");
	
	// Disable movement
	SetProcess(false);
	SetPhysicsProcess(false);
	
	// Find and stop the stopwatch
	var stopwatchLabel = GetTree().Root.FindChild("stopwatch_label", true, false);
	if (stopwatchLabel != null && stopwatchLabel is StopwatchLabel stopwatch)
	{
		//stopwatch.Stop();
	}
	else
	{
		GD.Print("Stopwatch label not found!");
	}
	
	// Show death animation or effect
	if (animatedSprite != null)
	{
		// Flash red and fade out
		Modulate = new Color(1, 0, 0, 1);
		
		// Create a tween for fade out effect
		var tween = CreateTween();
		tween.TweenProperty(this, "modulate", new Color(1, 0, 0, 0.2f), 1.0f);
		tween.Play();
	}
	
	// Wait for 3 seconds before restarting
	await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
	
	// Restart the level
	GetTree().ReloadCurrentScene();
}
	
	// Method to attack enemies
	private void Attack()
	{
		if (attackTimer > 0)
			return;
			
		GD.Print("Player attacking!");

        if (attackSound != null)
        {
            attackSound.Play();
        }

        // Play attack animation based on facing direction
        if (animatedSprite != null)
		{
			isAttacking = true;
			
			if (facingRight)
			{
				animatedSprite.Play("attack_right");
				// Connect to the animation_finished signal for one-time use
				if (!animatedSprite.IsConnected("animation_finished", new Callable(this, nameof(OnAttackAnimationFinished))))
				{
					animatedSprite.Connect("animation_finished", new Callable(this, nameof(OnAttackAnimationFinished)));
				}
			}
			else
			{
				animatedSprite.Play("attack_left");
				// Connect to the animation_finished signal for one-time use
				if (!animatedSprite.IsConnected("animation_finished", new Callable(this, nameof(OnAttackAnimationFinished))))
				{
					animatedSprite.Connect("animation_finished", new Callable(this, nameof(OnAttackAnimationFinished)));
				}
			}
		}
		
		// Get all enemies in attack range
		var attackArea = GetNode<Area2D>("AttackArea");
		if (attackArea == null)
		{
			GD.Print("AttackArea not found! Make sure to add it to your player scene.");
			return;
		}
		
		var bodies = attackArea.GetOverlappingBodies();
		foreach (var body in bodies)
		{
			// Make sure we're only attacking enemies, not ourselves
			if (body != this && body is CharacterBody2D enemy && body.HasMethod("take_damage"))
			{
				enemy.Call("take_damage", AttackDamage);
			}
		}
		
		// Start attack cooldown
		attackTimer = attackCooldown;
	}
	
	void Collision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision1 = GetSlideCollision(i);
			var collider1 = collision1.GetCollider();

			if (collider1 is StaticBody2D staticBody)
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

	public override void _PhysicsProcess(double delta)
	{
		// Decrease timers
		if (bPressedTimer > 0)
		{
			bPressedTimer -= (float)delta;
		}
		
		if (attackTimer > 0)
		{
			attackTimer -= (float)delta;
		}
		
		if (isInvulnerable)
		{
			invulnerabilityTimer -= (float)delta;
			if (invulnerabilityTimer <= 0)
			{
				isInvulnerable = false;
				Modulate = new Color(1, 1, 1, 1); // Reset color
			}
		}
		
		var code = GetNode<CanvasLayer>("../CanvasLayer"); 
		if (code.Visible)
		{
			// If the editor is visible, don't process movement
			return;
		}

		// Check for attack input
		if (Input.IsActionJustPressed("attack"))
		{
			Attack();
		}

		// Check for animation completion
		if (isAttacking && animatedSprite != null)
		{
			// Check if the animation has finished playing
			if (!animatedSprite.IsPlaying() || (animatedSprite.Animation != "attack_left" && animatedSprite.Animation != "attack_right"))
			{
				isAttacking = false;
				// Return to idle animation based on facing direction
				animatedSprite.Play(facingRight ? "idle" : "idle");
			}
		}

		// Dampen velocity slightly
		vel *= 0.55f;

		// Don't process movement if attacking
		if (!isAttacking)
		{
			// Update velocity based on input
			if (Input.IsActionPressed("Move_L"))
			{
				vel.X -= Speed;
				facingRight = false;
				if (animatedSprite != null && !isAttacking)
				{
					animatedSprite.Play("idle");
				}
			}
			if (Input.IsActionPressed("Move_R"))
			{
				vel.X += Speed;
				facingRight = true;
				if (animatedSprite != null && !isAttacking)
				{
					animatedSprite.Play("idle");
				}
			}
			if (Input.IsActionPressed("Move_U"))
			{
				vel.Y -= Speed;
			}
			if (Input.IsActionPressed("Move_D"))
			{
				vel.Y += Speed;
			}
		}

		// Apply damping to the velocity
		vel *= 0.8f;

		Collision();

		// Set the built-in velocity and move
		Velocity = vel*32 * (float)delta;
		MoveAndSlide();
	}
	
	// Connected to the animation_finished signal when the attack animation completes
	private void OnAttackAnimationFinished()
	{
		if (isAttacking && animatedSprite != null)
		{
			isAttacking = false;
			// Return to idle animation based on facing direction
			animatedSprite.Play(facingRight ? "idle" : "idle");
			
			// Disconnect the signal to avoid it being called for other animations
			if (animatedSprite.IsConnected("animation_finished", new Callable(this, nameof(OnAttackAnimationFinished))))
			{
				animatedSprite.Disconnect("animation_finished", new Callable(this, nameof(OnAttackAnimationFinished)));
			}
		}
	}
}
