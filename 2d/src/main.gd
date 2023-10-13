extends Node


var settings = ConfigFile.new()
var master_tween = Tween.new()
var sfx_tween = Tween.new()
var music_tween = Tween.new()


func _enter_tree():
	var err = settings.load("user://settings.cfg")
	if err:
		match err:
			ERR_FILE_NOT_FOUND:
				save_settings()
			_:
				print("Error loading settings file (%s)." % err)


func _ready():
	add_child(master_tween)
	add_child(sfx_tween)
	add_child(music_tween)
	
	master_tween.name = "MasterTween"
	sfx_tween.name = "SfxTween"
	music_tween.name = "MusicTween"


func save_settings():
	settings.save("user://settings.cfg")


# Volume

func _set_Master_volume(value: float): AudioServer.set_bus_volume_db(0, value)
func _set_Sfx_volume(value: float): AudioServer.set_bus_volume_db(1, value)
func _set_Music_volume(value: float): AudioServer.set_bus_volume_db(2, value)

func fade_bus(name, value):
	var tween = get_node("%sTween" % name)
	var initial = AudioServer.get_bus_volume_db(AudioServer.get_bus_index(name))
	tween.interpolate_method(self, "_set_%s_volume" % name,
							initial, value,
							0.4, 4, 2)
	tween.start()
