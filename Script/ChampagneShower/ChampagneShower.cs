using Godot;
using System;

public class ChampagneShower : MiniGame
{
	[Export] private float winXPos = 610;
	private Champagne champagne = null;

	private bool hasWin = false;

	public override void _Ready()
	{
		champagne = GetNode<Champagne>("Champagne");
	}

	public override void _Process(float delta)
	{
		if (!hasWin && champagne.Position.x > winXPos)
		{
			EmitSignal(nameof(Win));
			hasWin = true;
		}
	}
}
