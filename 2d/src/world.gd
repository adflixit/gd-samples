extends Node2D


class Entry:
	var is_created
	var pos
	var balance
	
	func _init(_is_created, _pos, _balance):
		is_created = _is_created
		pos = _pos
		balance = _balance


const CAPACITY = 20

onready var cap_x = 3
onready var cap_y = 5
onready var dist_x = (A.vp_width - A.platform_width * cap_x) / (cap_x - 1)
onready var dist_y = A.vp_height / cap_y
onready var kernel = CAPACITY - cap_y * 2
onready var kernel_height = kernel * dist_y

var platform_scene = preload("res://assets/prefabs/platform.tscn")

var generated = []
var last_x = 0.0
var last_y = 0.0
var balance = 0
var median = 0
var platform_num = 0
var page = 0
var free_room = CAPACITY


func step():
	page += 1
	
	if page % (kernel / cap_y) == 0:
		generate()
	
	for entry in generated:
		if !entry.is_created and A.game.pos.y - entry.pos.y < A.vp_height * 2:
			create_platform(entry)
	
	for child in get_children():
		if child.position.y - A.game.pos.y > A.vp_height * 2:
			destroy_platform(child)


func on_setup():
	last_x = A.vp_width / 2
	last_y = A.vp_height
	
	A.game.create_player(Vector2(last_x, A.vp_height - (dist_y + A.player_radius + 60)))
	
	generate()
	
	for i in cap_y * 3:
		create_platform(generated[i])


func on_reset():
	balance = 0
	median = 0
	platform_num = 0
	page = 0
	free_room = CAPACITY
	
	for child in get_children():
		destroy_platform(child)
	
	generated.clear()


func add_entry(dir: int):
	var dest_x = last_x + (A.platform_width + dist_x) * dir
	var dest_y = last_y - dist_y
	
	last_x = dest_x
	last_y = dest_y
	platform_num += 1
	balance += dir
	free_room -= 1
	
	generated.append(Entry.new(false, Vector2(dest_x, dest_y), balance))


func create_platform(entry):
	var platform = platform_scene.instance()
	add_child(platform)
	platform.position = entry.pos
	platform.position.y += A.platform_height / 2
	entry.is_created = true


func destroy_platform(platform):
	remove_child(platform)
	platform.queue_free()


func b(index): return generated[-1 - index].balance
func maxr(index): return max(b(index - 1), maxr(index - 1)) if index > 1 else b(0)
func minr(index): return min(b(index - 1), minr(index - 1)) if index > 1 else b(0)


func match_balance_pattern(b0, b1, b2, b3, b4, b5):
	return (b(0) == b0 + b1 and
			b(1) == b0 + b2 and
			b(2) == b0 + b3 and
			b(3) == b0 + b4 and
			b(4) == b0 + b5)


func generate():
	if page > 0:
		free_room = kernel
		generated = generated.slice(kernel, CAPACITY)
	
	while free_room > 0:
		var rand = 1 if randi() & 1 else -1
		var local_balance = balance - median
		var projected_balance = balance + rand
		
		if (platform_num > cap_y and
			((projected_balance < minr(4) and
				match_balance_pattern(projected_balance, 1, 2, 1, 2, 3)) or
			(projected_balance > maxr(4) and
				match_balance_pattern(projected_balance, -1, -2, -1, -2, -3)))):
					median += rand
					add_entry(rand)
		else:
			if platform_num == 0:
				add_entry(0)
			else:
				match local_balance:
					0:
						add_entry(rand)
					-1:
						add_entry(1)
					1:
						add_entry(-1)
