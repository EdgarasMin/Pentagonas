extends ProgressBar


var parent
var max_value_amount
var min_value_amount
func _ready():
	parent = get_parent()
	max_value_amount = parent.MaxHealth
	min_value_amount = parent.MinHealth
	
func _process(delta):
	self.value = parent.Health
	if parent.Health != max_value_amount:
		self.visible = true
		if parent.Health == min_value_amount:
			self.visible = false
	else:
		self.visible = false
	
