using Godot;
using System;

public class DontDoDuck : MiniGame
{
	private static bool isDrawReady = false;

	public override void _Ready()
	{
		if (isDrawReady)
		{
			DontDoDuckAnswer answer = CreateWindow<DontDoDuckAnswer>("res://Scene/MiniGames/DontDoDucks/DontDoDucks_Answer.tscn");

			answer.Connect(nameof(DontDoDuckAnswer.GoodAnswer), this, nameof(OnGoodAnswer));
		}
		else
		{
			DontDoDuckDraw draw = CreateWindow<DontDoDuckDraw>("res://Scene/MiniGames/DontDoDucks/DontDoDucks_Draw.tscn");

			draw.Connect(nameof(DontDoDuckDraw.OnDrawValidate), this, nameof(OnDrawValidate));
		}
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
		isDrawReady = true;
		EmitSignal(nameof(Win));
	}

	private void OnGoodAnswer()
	{
		isDrawReady = false;
		EmitSignal(nameof(Win));
	}
}
