extends CharacterBody2D

# Enemy properties
var player = null
var player_chase = false
var health = 100
var damage = 10
var can_attack = true
var  health_max = 100
var AttackDamage = 25
var health_min = 0
	

@onready var animation = $AnimatedSprite2D
@onready var attack_cooldown = $AttackCooldown

func _ready():
	scale = Vector2(0.2, 0.2)
	animation.play("idle")
	attack_cooldown.wait_time = 1.0
	attack_cooldown.one_shot = true
	attack_cooldown.connect("timeout", _on_attack_cooldown_timeout)

func _physics_process(delta):
	# Movement logic
	if player_chase and player != null:
		var direction = (player.position - position).normalized()
		velocity = direction * 100
		animation.play("walk")
	else:
		velocity = Vector2.ZERO
		animation.play("idle")
	
	move_and_slide()
	
	# Attack logic
	if player_chase and can_attack and player != null:
		var hitbox = $Hitbox/CollisionShape2D
		if hitbox.disabled == false:  # Only attack if hitbox is active
			var bodies = $Hitbox.get_overlapping_bodies()
			for body in bodies:
				if body.is_in_group("Player"):
					attack_player()

func attack_player():
	if can_attack and player != null:
		can_attack = false
		animation.play("attack")
		if player.has_method("take_damage"):
			player.take_damage(damage)
		attack_cooldown.start()

func take_damage(amount):
	health -= amount
	modulate = Color(1, 0.3, 0.3)  # Flash red
	
	var timer = get_tree().create_timer(0.2)
	await timer.timeout
	modulate = Color(1, 1, 1)
	
	if health <= 0:
		queue_free()

func _on_attack_cooldown_timeout():
	can_attack = true

func _on_detection_area_body_entered(body):
	if body.is_in_group("Player"):
		player = body
		player_chase = true

func _on_detection_area_body_exited(body):
	if body.is_in_group("Player"):
		player_chase = false
		player = null
