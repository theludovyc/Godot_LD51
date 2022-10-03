using System.Runtime.InteropServices;
using Godot;
using System;

public class Drawer : Node
{
	[Export] private Color _color = new Color(0f, 0f, 0f);
	[Export] private float _penSize = 5f;
	[Export] private Texture _penTexture = null;

	private Node linesParent = null;
	private Line2D currentLine = null;
	private bool isPressed = false;
	private Sprite _penSprite = null;
	private int pointsNumber = 0;

	public int PointsNumber {get => pointsNumber;}

	public override void _Ready()
	{
		base._Ready();
		linesParent = GetNode("Lines");
		CreatePenSprite();
	}

	public override void _Process(float delta)
	{
		_penSprite.Position = GetViewport().GetMousePosition();
	}

	private void CreatePenSprite()
	{
		_penSprite = new Sprite();
		_penSprite.Scale = Vector2.One / 2;
		_penSprite.Texture = _penTexture;
		AddChild(_penSprite);
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton)
		{
			InputEventMouseButton mouse = (InputEventMouseButton)inputEvent;
			isPressed = mouse.Pressed;
			currentLine = new Line2D();
			currentLine.DefaultColor = _color;
			currentLine.Width = _penSize;
			linesParent.AddChild(currentLine);
		}
		
		if (inputEvent is InputEventMouseMotion && isPressed)
		{
			InputEventMouseMotion motion = (InputEventMouseMotion)inputEvent;

			currentLine.AddPoint(motion.Position);
			pointsNumber++;
		}
	}

}
