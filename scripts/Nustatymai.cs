using Godot;
using System;

public partial class Nustatymai : Control
{

	public HSlider sliderMusic;
	public HSlider sliderSFX;
	public CheckBox muteButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VBoxContainer vbox = GetNode<VBoxContainer>("VBoxContainer");

		sliderMusic = vbox.GetNode<HSlider>("Volume");
		sliderMusic.Value = Global.VolumeSliderValue;
		
		sliderSFX = vbox.GetNode<HSlider>("VolumeSFX");
		sliderSFX.Value = Global.VolumeSFXSliderValue;

		muteButton = vbox.GetNode<CheckBox>("Mute");
		

	}

	int SFX_id = AudioServer.GetBusIndex("SFX");
	int Music_id = AudioServer.GetBusIndex("Music");

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		
	}
	
	public void _on_volume_value_changed(float x)
	{
		
		AudioServer.SetBusVolumeDb(Music_id, x-80);
		//AudioServer.SetBusMute(Music_id, x<0.5);
		Global.VolumeSliderValue = (int)x;
		
	}
	
	public void _on_volume_sfx_value_changed(float x)
	{
		
		AudioServer.SetBusVolumeDb(SFX_id, x-80);
		//AudioServer.SetBusMute(Music_id, x<0.5);
		Global.VolumeSFXSliderValue = (int)x;
		
	}
	
	public void _on_mute_toggled(bool toggled)
	{
		//int Music_id = AudioServer.GetBusIndex("Music");
		AudioServer.SetBusMute(Music_id,toggled);
		Global.MuteToggled = toggled;
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
