extends Node


onready var main = get_node("/root/Main")
onready var game = get_node("/root/Main/Game")
onready var world = get_node("/root/Main/Game/World")
onready var camera = get_node("/root/Main/Game/Camera")

onready var vp_width = get_viewport().get_visible_rect().size.x
onready var vp_height = get_viewport().get_visible_rect().size.y

onready var player_radius = 90
onready var platform_width = 260
onready var platform_width_half = 130
onready var platform_height = 30
