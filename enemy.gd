extends CharacterBody2D


var speed = 50
var player_chase = false
var player = null

func _physics_process(_delta):
	if player_chase:
		position += (player.position - position)/ speed
		$AnimatedSprite2D.scale = Vector2(0.1, 0.1)
		$AnimatedSprite2D.play("move")
		
	else:
		$AnimatedSprite2D.scale = Vector2(0.1, 0.1)
		$AnimatedSprite2D.play("still")




func _on_area_2d_body_entered(body):
	player = body
	player_chase = true

func _on_area_2d_body_exited(body):
	player = body
	player_chase = false
