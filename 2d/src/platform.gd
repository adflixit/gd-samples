class_name Platform
extends StaticBody2D


onready var moveable = $Sprite
var flinch_friction = 0.1 * 60
onready var rotation_target = 0
onready var position_target = Vector2.ZERO


func _process(delta):
	delta *= A.game.time_scale
	moveable.rotation = lerp(moveable.rotation, rotation_target, flinch_friction * delta)
	moveable.position = moveable.position.linear_interpolate(position_target, flinch_friction * delta)
	rotation_target = 0
	position_target = Vector2.ZERO


func flinch(pos, vel):
	rotation_target = (pos.x - position.x) / A.platform_width * -sign(vel.y) * vel.length() / 1000
	position_target = -vel / 8
