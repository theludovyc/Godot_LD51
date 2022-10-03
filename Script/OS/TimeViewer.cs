using Godot;
using System;

public class TimeViewer : Label
{
	public override void _Process(float delta)
	{
		var time = OS.GetTime();
		string timeString = string.Format("{0:00}:{1:00}:{2:00}", time["hour"], time["minute"], time["second"]);

		Text = timeString;
	}
}
