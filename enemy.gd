extends CharacterBody2D


var speed = 50
var player_chase = false
var player = null

func _physics_process(delta):
	if player_chase:
		position += (player.position - position)/ speed
		$AnimatedSprite2D.scale = Vector2(0.1, 0.1)
		$AnimatedSprite2D.play("move")
		
	else:
		$AnimatedSprite2D.scale = Vector2(0.1, 0.1)
		$AnimatedSprite2D.play("move")




func _on_area_2d_body_entered(body: Node2D) -> void:
	player = body
	player_chase = true

func _on_area_2d_body_exited(body: Node2D) -> void:
	player = body
	player_chase = true
