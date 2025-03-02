using Godot;
using System;

public partial class Nustatymai : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_volume_value_changed(float x)
	{
		AudioServer.SetBusVolumeDb(0, x);
	}
	
	public void _on_mute_toggled(bool toggled)
	{
		AudioServer.SetBusMute(0,toggled);
	}
	
	public void _on_resolutions_item_selected(int item)
	{
		switch (item)
		{
			case 0:
	 			DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
	 			break;
 			case 1:
				DisplayServer.WindowSetSize(new Vector2I(1600, 900));
				break;
			case 2:
				DisplayServer.WindowSetSize(new Vector2I(1280, 720));
				break;
		}
	}
	
	public void _on_back_pressed()
	{
		//GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
		GetTree().ChangeSceneToFile(Global.LastScene);
	}
}
