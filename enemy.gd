extends CharacterBody2D

# Enemy properties
var player = null
var player_chase = false
var MaxHealth = 100
var MinHealth = 0
var damage = 10
var can_attack = true
var health_max = 100
var AttackDamage = 10
var health_min = 0
var Health = 100
var speed = 120
var attack_range = 50  # Distance for attack if no hitbox is available
var player_in_hitbox = false  # Track if player is in hitbox area
var is_moving = false  # Track if enemy is moving

# Node references - don't use @onready to allow null checks
var animation
var attack_cooldown
var attack_area
var detection_area
var enemy_hitbox

func _ready():
	scale = Vector2(0.2, 0.2)
	
	# Get references with null checks
	animation = get_node_or_null("AnimatedSprite2D")
	attack_cooldown = get_node_or_null("AttackCooldown")
	enemy_hitbox = get_node_or_null("enemy_hitbox")
	detection_area = get_node_or_null("DetectionArea")
	
	# Check nodes before using them
	if animation:
		animation.play("still")
	else:
		print("ERROR: AnimatedSprite2D node not found!")
	
	# Setup attack cooldown if it exists
	if attack_cooldown:
		attack_cooldown.wait_time = 1.0
		attack_cooldown.one_shot = true
		attack_cooldown.connect("timeout", Callable(self, "_on_attack_cooldown_timeout"))
	else:
		print("ERROR: AttackCooldown node not found!")
	
	# Get attack area if enemy_hitbox exists
	if enemy_hitbox:
		attack_area = enemy_hitbox.get_node_or_null("CollisionShape2D")
		if attack_area:
			attack_area.disabled = false
		enemy_hitbox.connect("body_entered", Callable(self, "_on_hitbox_body_entered"))
		enemy_hitbox.connect("body_exited", Callable(self, "_on_hitbox_body_exited"))
	else:
		print("ERROR: enemy_hitbox node not found!")
	
	# Connect detection area signals if it exists
	if detection_area:
		detection_area.connect("body_entered", Callable(self, "_on_detection_area_body_entered"))
		detection_area.connect("body_exited", Callable(self, "_on_detection_area_body_exited"))
	else:
		print("ERROR: DetectionArea node not found!")
	
	# Add to enemy group
	add_to_group("Enemy")
	print("Enemy initialized")

func _physics_process(delta):
	# Movement logic
	if player_chase and player != null:
		# Calculate direction to player
		var direction = (player.position - position).normalized()
		velocity = direction * speed
		is_moving = true
		
		# Update animation
		if animation:
			# If we're not attacking, play walk animation
			if animation.animation != "attack":
				animation.play("move")
			
			# Flip sprite based on direction
			if direction.x > 0:
				animation.flip_h = false
			else:
				animation.flip_h = true
	else:
		# Stop moving when player is not being chased
		velocity = Vector2.ZERO
		is_moving = false
		
		# Update animation to idle when not moving and not attacking
		if animation and animation.animation != "attack":
			animation.play("still")
	
	# Apply movement
	move_and_slide()
	
	# Attack logic for both hitbox and distance-based attacks
	if player_chase and can_attack and player != null:
		# Check if player is in hitbox or within range
		if player_in_hitbox or (enemy_hitbox == null and position.distance_to(player.position) < attack_range):
			attack_player()

func attack_player():
	if can_attack and player != null:
		can_attack = false
		print("Enemy attacking player!")
		
		if animation:
			animation.play("attack")
		
		# Check if player has the take_damage method
		if player.has_method("take_damage"):
			player.take_damage(damage)
			print("Damage applied to player: ", damage)
		
		# Start cooldown
		if attack_cooldown:
			attack_cooldown.start()
		else:
			# Fallback cooldown if timer doesn't exist
			await get_tree().create_timer(1.0).timeout
			can_attack = true

func take_damage(amount):
	Health -= amount
	modulate = Color(1, 0.3, 0.3)  # Flash red
	print("Enemy took damage: ", amount, " Health remaining: ", Health)
	
	# Create a timer for the flash effect
	var timer = get_tree().create_timer(0.2)
	await timer.timeout
	modulate = Color(1, 1, 1)
	
	if Health <= 0:
		queue_free()

func _on_attack_cooldown_timeout():
	can_attack = true
	print("Enemy attack cooldown finished - ready to attack again")
	
	# Check if we should immediately attack again
	if player != null and player_in_hitbox:
		attack_player()

func _on_detection_area_body_entered(body):
	if body.is_in_group("Player"):
		player = body
		player_chase = true
		print("Player detected! Starting to chase.")

func _on_detection_area_body_exited(body):
	if body.is_in_group("Player"):
		player_chase = false
		player = null
		print("Player lost! Stopping chase.")
		
		# Return to idle animation when player leaves detection range
		if animation:
			animation.play("still")

func _on_hitbox_body_entered(body):
	if body.is_in_group("Player"):
		print("Player in attack range!")
		player_in_hitbox = true
		if can_attack:
			attack_player()

func _on_hitbox_body_exited(body):
	if body.is_in_group("Player"):
		player_in_hitbox = false
		print("Player left attack range!")
