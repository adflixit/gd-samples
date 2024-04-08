class_name Player
extends KinematicBody2D


var gravity = 1440
var jump_force = gravity * 1.0
var bounce_force = gravity * 0.8
var trace_reach = 400
var bounce_sigma =  PI / 8

var trace_queried = false
var trace_vector = Vector2()
var velocity = Vector2()


func _physics_process(delta):
	delta *= A.game.time_scale
	velocity.y += gravity * delta
	
	if trace_queried:
		trace_queried = false
		
		var from = trace_vector * A.player_radius
		var to = from + trace_vector * trace_reach
		var space_state = get_world_2d().direct_space_state
		var trace = space_state.intersect_ray(global_position + from, global_position + to)
		
		if !trace.empty():
			var fraction = (global_position + from).distance_to(trace.position) / trace_reach
			var jump_vector = -trace_vector
			velocity = jump_vector * (jump_force * (1.0 - fraction))
	
	var collision = move_and_collide(velocity * delta)
	if collision:
		var platform = collision.collider as Platform
		if platform:
			platform.flinch(collision.position, velocity)
		
		var normal = collision.normal
		var bounce_vector = normal.rotated(PI / 2 - rand_range(bounce_sigma, PI - bounce_sigma))
		var bounce_factor = bounce_force if normal.y < 0 else bounce_force * 0.4
		velocity = bounce_vector * bounce_factor
	
	if position.y > A.game.pos.y + A.vp_height * 2:
		A.game.destroy_player()


func _unhandled_input(event):
	if (!A.game.is_paused and
		(event is InputEventScreenTouch and event.pressed) or
		(event is InputEventMouseButton and event.pressed)):
			trace_queried = true
			trace_vector = -(global_position - (A.camera.position + event.position)).normalized()
