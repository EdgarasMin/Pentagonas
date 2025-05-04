using Godot;
using System;

public partial class Global : Node
{
	public static float elapsedTime;
	public static string LastScene = "";
	public static int VolumeSliderValue = 80;
	public static int VolumeSFXSliderValue = 80;
	public static bool MuteToggled = true;
	public static bool FullScreenToggled = false;
	public static int SelectedResolution = 0;
	public static int SelectedFps = 0;
}
