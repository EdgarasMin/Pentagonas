using Godot;
using System;

public partial class StopwatchLabel : Label
{
	public float elapsedTime;
	
	public bool stopWatch = false;
	

	
	public override void _Process(double delta)
	{
		elapsedTime = Global.elapsedTime;
		if(elapsedTime == null)
		elapsedTime = 0.0f;
	
		if(stopWatch)return;
		elapsedTime += (float)delta;
		Global.elapsedTime = elapsedTime;
		int minutes = (int)(elapsedTime / 60);
		int seconds = (int)(elapsedTime % 60);
		int milliseconds = (int)((elapsedTime * 1000) % 1000);

		Text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
	}
	public void Stop()
	{
		stopWatch = true;
	}
}
