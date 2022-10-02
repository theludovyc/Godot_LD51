using System.Collections.Generic;
using Godot;
using System;

public class VegetableSamurai : MiniGame
{
	[Export] Vector2 _knifeOffset = Vector2.Zero;
	[Export] List<Texture> _foodState = null;
	[Export] int _clickNumber = 20;

	private TextureButton textureButton = null;
	private TextureRect knife = null;
	private int clickCount = 0;

	public override void _Ready()
	{
		textureButton = GetNode<TextureButton>("Control/TextureButton");
		textureButton.Connect("button_down", this, nameof(OnPressed));
		knife = GetNode<TextureRect>("Knife");
	}

	public override void _Process(float delta)
	{
		UpdateKnifePosition();
	}

	private void UpdateKnifePosition()
	{
		Vector2 knifePosition = GetViewport().GetMousePosition();
		knifePosition -= new Vector2(knife.RectSize.x / 2, knife.RectSize.y / 2);
		knifePosition += _knifeOffset;

		knife.RectPosition = knifePosition;
	}

	private void OnPressed()
	{
		clickCount++;
		UpdateTexture();

		if (clickCount == _clickNumber)
			EmitSignal(nameof(Win));
	}

	private void UpdateTexture()
	{
		int index = Mathf.FloorToInt(((float)clickCount / (float)_clickNumber) * _foodState.Count);

		index = Mathf.Clamp(index, 0, _foodState.Count - 1);
		textureButton.TextureNormal = _foodState[index];
	}

}
