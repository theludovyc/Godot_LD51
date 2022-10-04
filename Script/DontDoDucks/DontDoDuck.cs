using System.Collections.Generic;
using Godot;
using System;

public class DontDoDuck : MiniGame
{
	private static List<List<Vector2>> drawData = null;

	private DontDoDuckAnswer answer = null;
	private DontDoDuckDraw draw = null;

	protected override void OnFocus()
	{
		PlaySFX("Duck");
		if (drawData != null)
		{
			CreateAnswer();
		}
		else
		{
			CreateDraw();
		}
	}

	private void CreateAnswer()
	{
		if (draw != null)
		{
			draw.QueueFree();
			draw = null;
		}
		answer = CreateWindow<DontDoDuckAnswer>("res://Scene/MiniGames/DontDoDucks/DontDoDucks_Answer.tscn");
		answer.SetDraw(drawData);

		answer.Connect(nameof(DontDoDuckAnswer.GoodAnswer), this, nameof(OnGoodAnswer));
	}

	private void CreateDraw()
	{
		if (answer != null)
		{
			answer.QueueFree();
			answer = null;
		}
		draw = CreateWindow<DontDoDuckDraw>("res://Scene/MiniGames/DontDoDucks/DontDoDucks_Draw.tscn");

		draw.Connect(nameof(DontDoDuckDraw.OnDrawValidate), this, nameof(OnDrawValidate));
	}

	private T CreateWindow<T>(string path) where T : Node
	{
		PackedScene scene = GD.Load<PackedScene>(path);

		T t = (T)scene.Instance();
		AddChild(t);

		return (t);
	}

	private void OnDrawValidate()
	{
		drawData = draw.GetDrawData();
		EmitSignal(nameof(Win));
	}

	private void OnGoodAnswer()
	{
		drawData = null;
		EmitSignal(nameof(Win));
	}
}
