extends Node


signal paused
signal reset
signal setup
signal start
signal game_over

enum {
	GS_PREPARE,
	GS_PLAY,
	GS_GAME_OVER,
}

const DEF_TIME_SCALE = 1.2

var time_scale = DEF_TIME_SCALE
var time_scale_tween = Tween.new()

var is_paused = false
var can_pause = true

var state = GS_PREPARE
var pos = Vector2()
var player_scene = preload("res://assets/prefabs/player.tscn")
var player

var step = 0
var next_platform
var target_platform
var shift_right

onready var half_offset = A.world.dist_x + A.platform_width_half
onready var full_offset = A.world.dist_x + A.platform_width


func _ready():
	add_child(time_scale_tween)
	setup()


func _physics_process(delta):
	if state != GS_PLAY:
		return
	
	var dist = A.vp_height - (player.position.y - pos.y)
	var threshold = A.world.dist_y * 2
	
	if dist > threshold:
		pos.y -= dist - threshold
		A.camera.position = pos
	
	if pos.y < -(step + 1) * A.world.dist_y:
		step += 1
		if step % A.world.cap_y == 0:
			A.world.step()
		get_next_platform()
	
	if target_platform:
		if pos.y > target_platform.y:
			var t = 1.0 - (pos.y - target_platform.y) / A.world.dist_y
			
			if !shift_right:
				pos.x = (target_platform.x - (A.vp_width + half_offset) + full_offset * t)
			else:
				pos.x = (target_platform.x + half_offset - full_offset * t)
		else:
			if shift_right:
				pos.x = target_platform.x - A.platform_width_half
			else:
				pos.x = target_platform.x + A.platform_width_half - A.vp_width
			target_platform = null
	
	if (player.position.x < pos.x or
		player.position.x > pos.x + A.vp_width or
		player.position.y > pos.y + A.vp_height):
			game_over()


func get_next_platform():
	next_platform = A.world.generated[step % A.world.kernel + A.world.cap_y].pos
	if !target_platform and (next_platform.x < pos.x or next_platform.x > pos.x + A.vp_width):
		target_platform = next_platform
		shift_right = next_platform.x < pos.x


func create_player(pos):
	player = player_scene.instance()
	A.world.add_child(player)
	player.position = pos


func destroy_player():
	if player:
		A.world.remove_child(player)
		player.queue_free()
		player = null


func set_paused(value):
	if can_pause or !value:
		is_paused = value
		emit_signal("paused", value)


func toggle_paused():
	set_paused(!is_paused)


func setup():
	randomize()
	time_scale = 0
	emit_signal("setup")
	get_next_platform()


func reset():
	state = GS_PREPARE
	can_pause = true
	pos = Vector2.ZERO
	A.camera.position = pos
	
	destroy_player()
	
	step = 0
	target_platform = null
	
	emit_signal("reset")


func restart():
	reset()
	setup()


func start():
	state = GS_PLAY
	time_scale = DEF_TIME_SCALE
	emit_signal("start")


func game_over():
	restart()


func on_paused(value):
	if state != GS_PREPARE:
		fade_time_scale(0.0 if value else DEF_TIME_SCALE)


func fade_time_scale(value):
	time_scale_tween.interpolate_property(self, "time_scale",
											time_scale, value,
											0.4, 4, 2)
	time_scale_tween.start()


func _unhandled_input(event):
	if event is InputEventKey:
		if event.pressed:
			if event.scancode == KEY_TAB:
				toggle_paused()
	
	if (state == GS_PREPARE and !is_paused and
		((event is InputEventScreenTouch and (event as InputEventScreenTouch).pressed) or
		(event is InputEventMouseButton and (event as InputEventMouseButton).pressed))):
			start()
