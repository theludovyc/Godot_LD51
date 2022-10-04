using System.Collections.Generic;
using System.Linq;
using Godot;
using System;

public class DontDoDuckAnswer : Node
{
	[Signal] public delegate void GoodAnswer();
	[Export] private float drawLineSize = 5f;
	[Export] private Color drawColor = new Color(0f, 0f, 0f);
	[Export] private float drawSizeMultiplier = 0.5f;
	[Export] List<string> _wrongAnswers = null;
	[Export] List<string> _goodAnswers = null;

	private static Random rng = new Random();

	public override void _Ready()
	{
		base._Ready();
		SetUpButtons();
	}

	public void SetDraw(List<List<Vector2>> linesData)
	{
		Control parent = GetNode<Control>("Control/Draw");

		foreach (List<Vector2> linePoints in linesData)
		{
			Line2D line = CreateLine();
			foreach (Vector2 point in linePoints)
				line.AddPoint(point * drawSizeMultiplier);
			parent.AddChild(line);
		}
	}

	private Line2D CreateLine()
	{
		Line2D line = new Line2D();

		line.DefaultColor = drawColor;
		line.Width = drawLineSize;
		return (line);
	}

	private void SetUpButtons()
	{
		List<TextureButton> buttons = GetButtons();
		List<string> wrongAnswers = _wrongAnswers.ToList();

		ShuffleList(wrongAnswers);
		ShuffleList(buttons);

		SetupButton(buttons[0], GetRandom(_goodAnswers), true);

		for (int i = 1; i < buttons.Count; i++)
			SetupButton(buttons[i], wrongAnswers[i], false);
	}

	private void SetupButton(TextureButton button, string text, bool isGoodAnswer)
	{
		Label label = button.GetNode<Label>("Label");

		label.Text = text;
		string method = isGoodAnswer ? nameof(OnGoodAnswer) : nameof(OnWrongAnswer);
		button.Connect("button_down", this, method);

	}

	private void OnGoodAnswer()
	{
		EmitSignal(nameof(GoodAnswer));
	}

	private void OnWrongAnswer()
	{
		GD.Print("NON");
	}

	private T GetRandom<T>(List<T> btns)
	{
		return btns[Convert.ToInt32(GD.RandRange(0, btns.Count - 1))];
	}

	public void ShuffleList<T>(List<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}


	private List<TextureButton> GetButtons()
	{
		var buttonsNodes = GetNode("Control/GridContainer").GetChildren();
		List<TextureButton> buttons = new List<TextureButton>();

		foreach (Node b in buttonsNodes)
			buttons.Add(((TextureButton)b));
		return (buttons);
	}

}
