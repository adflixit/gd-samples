class_name Player
extends KinematicBody2D


var gravity = 1440
var jump_force = gravity * 1.0
var bounce_force = gravity * 0.8
var trace_reach = 400
var bounce_sigma = deg2rad(20)

var trace_time = 0
var trace_queried = false
var trace_angle = 0.0
var velocity = Vector2()


func _physics_process(delta):
	delta *= A.game.time_scale
	
	if trace_queried:
		trace_queried = false
		
		var space_state = get_world_2d().direct_space_state
		var angle = Vector2(cos(trace_angle), sin(trace_angle))
		var from = angle * A.player_radius
		var to = from + angle * trace_reach
		var trace = space_state.intersect_ray(global_position + from, global_position + to)
		
		if !trace.empty():
			var fraction = (global_position + from).distance_to(trace.position) / trace_reach
			var push_angle = Vector2(cos(trace_angle + PI), sin(trace_angle + PI))
			velocity = push_angle * (jump_force * (1.0 - fraction))
	
	velocity.y += gravity * delta
	
	var collision = move_and_collide(velocity * delta)
	if collision:
		var angle = collision.normal.angle()
		var bounce_mult = 1.0 if angle < 0 else 0.4
		var bounce_angle = angle - PI/2 + rand_range(bounce_sigma, PI - bounce_sigma)
		var bounce_factor = bounce_force * bounce_mult
		
		velocity = (Vector2(cos(bounce_angle), sin(bounce_angle)) * bounce_factor)
		
		var platform = collision.collider as Platform
		if platform:
			platform.flinch(collision.position, collision.normal * bounce_factor)
	
	if position.y > A.game.pos.y + A.vp_height * 2:
		A.game.destroy_player()


func _unhandled_input(event):
	if (!A.game.is_paused and
		trace_time + 100 < Time.get_ticks_msec() and
		((event is InputEventScreenTouch and event.pressed) or
		(event is InputEventMouseButton and event.pressed))):
			trace_time = Time.get_ticks_msec()
			trace_queried = true
			trace_angle = (global_position - (A.camera.position + event.position)).angle() + PI
