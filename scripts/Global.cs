using Godot;
using System;

public partial class Global : Node
{
	public static string LastScene = "";
	public static int VolumeSliderValue = 80;
	public static int VolumeSFXSliderValue = 80;
	public static bool MuteToggled = false;
	public static bool FullScreenToggled = false;
	public static int SelectedResolution = 2;
	public static int SelectedFps = 0;
}
