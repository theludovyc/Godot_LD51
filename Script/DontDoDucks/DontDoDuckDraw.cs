using Godot;
using System;

public class DontDoDuckDraw : Node
{
	[Signal] public delegate void OnDrawValidate();

	public override void _Ready()
	{
		GetNode<TextureButton>("Control/TextureButton").Connect("button_down", this, nameof(DrawValidate));
	}

	private void DrawValidate()
	{
		EmitSignal(nameof(OnDrawValidate));
	}

}
