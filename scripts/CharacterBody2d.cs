using Godot;
using System;

public partial class CharacterBody2d : CharacterBody2D
{
	
	private CharacterBody2D player;
	void Teleport(int x, int y){
		Position = new Vector2(x,y);
	}
	public Music musicScene;
	AnimatedSprite2D sprite;
	private bool isSpinning = false;
	private float spinTimeLeft = 3f;
	public float RotationSpeed = 360f; // degrees per second
	public int newScene = 0;
    private CollisionShape2D doorCollision;
    private AudioStreamPlayer soundTP;

    private bool isSpeedBoosted = false;
    private float speedBoostTimer = 0f;
    private int originalSpeed;


    public override void _Ready()
	{
		var stopwatchLabel = GetTree().Root.FindChild("stopwatch_label", true, false);
		if (stopwatchLabel != null && stopwatchLabel is StopwatchLabel stopwatch)
		{
			stopwatch.stopWatch=false;
		}

        AddToGroup("Player");








        //Clears bugs when scenes have already open objects
        CodeEditing2.ExitTree();
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GD.Print("Character ready: ");
		musicScene = GetNode<Music>("/root/Music");
		musicScene.switchSong("Song4");
		soundTP = GetNode<AudioStreamPlayer>("TP");
		
	}
	void Collision()
	{
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);
			var collider = collision.GetCollider();

			
			
			if (collider is StaticBody2D staticBody && staticBody.Name == "StaticBody2D2")
			{
				
				//GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
				isSpinning = true;
				AudioManager.PlaySound(soundTP.Stream);
				newScene = 1;
				GD.Print("1 act");
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			if (collider is StaticBody2D area && area.Name == "Area2D4")
			{
				
				//GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
				isSpinning = true;
				AudioManager.PlaySound(soundTP.Stream);
				newScene = 2;
				GD.Print("2 act");
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
			
			if (collider is StaticBody2D area2 && area2.Name == "portalas")
			{
				
				//GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
				isSpinning = true;
				AudioManager.PlaySound(soundTP.Stream);
				newScene = 3;
				GD.Print("3 act");
				// Optional: custom logic here
				// Example: QueueFree(), Play effect, etc.
			}
		}
	}
	
	
	
	public int Speed = 800;

	private Vector2 vel = Vector2.Zero;
	

	public override void _PhysicsProcess(double delta)
	{
		float deltaTime = (float)delta;
		if (CodeEditing2.Instance != null && CodeEditing2.Instance.editorShown)
		{
			GD.Print("Error: ");
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
			
			//GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
			switch(newScene)
			{
				case 1:
					GetTree().ChangeSceneToFile("res://scenes/Level2.tscn");
					break;
				case 2:
					GetTree().ChangeSceneToFile("res://scenes/Level1.tscn");
					break;
				case 3:
					GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
					break;
				default:
					Console.WriteLine("no value.");
					break;

			}
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

        // Handle speed boost timer
        if (isSpeedBoosted)
        {
            speedBoostTimer -= (float)delta;
            if (speedBoostTimer <= 0)
            {
                isSpeedBoosted = false;
                Speed = originalSpeed;
                GD.Print("⏱️ Speed boost ended.");
            }
        }


        // Set the built-in velocity and move
        Velocity = vel*32 * (float)delta;
		MoveAndSlide();
	}
    public void TryUnlockDoor()
    {
        var nearbyDoors = GetTree().GetNodesInGroup("Door");
		if(nearbyDoors.Count == 0)
		{
			GD.Print("nera duru grupes paimtos");
		}
        foreach (var node in nearbyDoors)
        {
            if (node is StaticBody2D door && Position.DistanceTo(door.Position) < 500)
            {
                GD.Print("🚪 Door unlocked!");

				// Disable "nu"
				doorCollision = GetNodeOrNull<CollisionShape2D>("../StaticBody2D3/nu");
                doorCollision.Disabled = true;

                // Play animation "2"
                var animSprite = door.GetNodeOrNull<AnimatedSprite2D>("DoorAnimatedSprite2d");
				
                if (animSprite != null)
                {
                    animSprite.Frame = 1;
                }

                return;
            }
        }

        GD.Print("No nearby door to unlock.");
    }
    public void ApplySpeedBoost(int duration)
    {
        if (isSpeedBoosted)
            return;

        GD.Print($"🚀 Speed boost for {duration} seconds!");
        isSpeedBoosted = true;
        originalSpeed = Speed;
        Speed *= 2;
        speedBoostTimer = duration;
    }






}
