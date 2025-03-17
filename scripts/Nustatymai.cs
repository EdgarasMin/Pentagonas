using Godot;
using System;

public partial class Nustatymai : Control
{

	public HSlider sliderMusic;
	public HSlider sliderSFX;
	public CheckBox muteButton;
	public OptionButton resolutionButton;
	public Music musicScene;
	
	public CheckBox Fullscreen;
	public OptionButton FpsButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		musicScene = GetNode<Music>("/root/Music");
		musicScene.switchSong("Song2");

		VBoxContainer vbox = GetNode<VBoxContainer>("VBoxContainer");

		sliderMusic = vbox.GetNode<HSlider>("Volume");
		sliderMusic.Value = Global.VolumeSliderValue;
		
		sliderSFX = vbox.GetNode<HSlider>("VolumeSFX");
		sliderSFX.Value = Global.VolumeSFXSliderValue;

		muteButton = vbox.GetNode<CheckBox>("Mute");
		muteButton.ButtonPressed = Global.MuteToggled;
		
		resolutionButton = vbox.GetNode<OptionButton>("Resolutions");
		resolutionButton.Selected=Global.SelectedResolution;
		
		Fullscreen = vbox.GetNode<CheckBox>("Fullscreen");
		Fullscreen.ButtonPressed = Global.FullScreenToggled;

		FpsButton = vbox.GetNode<OptionButton>("FPSchoose");
		FpsButton.Selected = Global.SelectedFps;


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
		AudioServer.SetBusMute(0,toggled);
		Global.MuteToggled = toggled;
	}
	
	public void _on_fullscreen_toggled(bool toggled)
	{
		Global.FullScreenToggled = toggled;
		if(toggled)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
		}
	}
	
	public void _on_resolutions_item_selected(int item)
	{
		Global.SelectedResolution = item;
		switch (item)
		{
			case 0:
	 			DisplayServer.WindowSetSize(new Vector2I(1920, 1080));
				CenterWindow();
	 			break;
 			case 1:
				DisplayServer.WindowSetSize(new Vector2I(1600, 900));
				CenterWindow();
				break;
			case 2:
				DisplayServer.WindowSetSize(new Vector2I(1280, 720));
				CenterWindow();
				break;
		}
	}
	
	public void CenterWindow()
	{
		// Get the position and size of the screen
		Vector2I center = DisplayServer.ScreenGetPosition(0) + DisplayServer.ScreenGetSize(0) / 2;
		
		// Get the size of the window (including decorations)
		Vector2I windowSize = GetWindow().GetSizeWithDecorations();

		// Set the window position to center it on the screen
		GetWindow().SetPosition(center - windowSize / 2);
	}
	
	public void _on_fp_schoose_item_selected(int item)
	{
		Global.SelectedFps = item;
		switch (item)
		{
			case 0:
	 			Engine.MaxFps = 60;
	 			break;
 			case 1:
				Engine.MaxFps = 45;
				break;
			case 2:
				Engine.MaxFps = 30;
				break;
		}
	}
	
	
	public void _on_back_pressed()
	{
		//GetTree().ChangeSceneToFile("res://scenes/HeroScene.tscn");
		GetTree().ChangeSceneToFile(Global.LastScene);
	}
}
