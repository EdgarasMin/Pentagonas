using Godot;
using System;

public partial class Global : Node
{
	public static float elapsedTime=0L;
	public static string LastScene = "";
	public static string user = "user";
	public static string Latest="";
	public static string allData1="";
	public static string allData2="";
	public static int VolumeSliderValue = 80;
	public static int VolumeSFXSliderValue = 80;
	public static bool MuteToggled = true;
	public static bool FullScreenToggled = false;
	public static int SelectedResolution = 0;
	public static int SelectedFps = 0;
}
