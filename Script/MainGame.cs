using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MainGame : Control
{
	const int maxPopup = 11;

	Dictionary<string, PackedScene> miniGames = new Dictionary<string, PackedScene>{
		{"Fryer", null},
		{"PetTheDog", null},
		{"ChampagneShower", null},
		{"VegetableSamurai", null},
		{"Memory", null},
		{"DontDoDucks", null}
	};

	private PackedScene myPopup = GD.Load<PackedScene>("res://Scene/MyPopup.tscn");
	private PackedScene GameOver = GD.Load<PackedScene>("res://Scene/GameOver.tscn");
	private PackedScene WinScene = GD.Load<PackedScene>("res://Scene/Win.tscn");
	private Stack<MyPopup> popups = new Stack<MyPopup>();
	private Timer timer = null;
	private Control winScreen = null;
	private Control gameOverScreen = null;
	private bool isGameStoped = false;

	void OnPopupHide()
	{
		if (popups.Count == 0)
			return;

		var popup = popups.Pop();
		popup.QueueFree();

		UpdateRamViewer();
		if (popups.Count > 0)
		{
			popups.Peek().SetPauseSubScene(false);
		}
		else
		{
			Win();
		}
	}

	Vector2 GetRandomPosition()
	{
		return new Vector2(((float)GD.RandRange(10, 990)), (float)GD.RandRange(10, 440));
	}

	void CreatePopup(String miniGameName)
	{
		if (popups.Count > 0)
		{
			popups.Peek().SetPauseSubScene(true);
		}

		MyPopup currentPopup = myPopup.Instance<MyPopup>();
		MiniGame game = miniGames[miniGameName].Instance<MiniGame>();

		currentPopup.AddChildMiniGame(game);
		GetNode("PopUps").AddChild(currentPopup);
		currentPopup.WindowTitle = miniGameName + ".exe";
		currentPopup.SetPosition(GetRandomPosition());
		currentPopup.Connect("popup_hide", this, "OnPopupHide");
		currentPopup.Popup_();
		popups.Push(currentPopup);
		UpdateRamViewer();
	}

	private void UpdateRamViewer()
	{
		GetNode<Label>("Taskbar/RamText").Text = (string.Format("RAM: {0:00}%", (popups.Count / ((float)maxPopup + 1)) * 100));
	}

	public override void _Ready()
	{
		GetNodes();
		StartGame();
	}

	private void StartGame()
	{
		isGameStoped = false;
		winScreen.Visible = false;
		gameOverScreen.Visible = false;

		foreach (var key in miniGames.Keys.ToList())
			miniGames[key] = GD.Load<PackedScene>($"res://Scene/MiniGames/{key}.tscn");

		for (int i = 0; i < maxPopup - 1; i++)
			CreatePopup(GetRandomMiniGameName());
		timer.Start();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (isGameStoped && (inputEvent is InputEventKey))
			StartGame();
	}

	private void GetNodes()
	{
		winScreen = GetNode<Control>("WinScreen");
		gameOverScreen = GetNode<Control>("GameOverScreen");
		timer = GetNode<Timer>("Timer");
	}

	private string GetRandomMiniGameName()
	{
		return miniGames.Keys.ToArray()[Convert.ToInt32(GD.RandRange(0, miniGames.Count - 1))];
	}

	void OnTimerTimeout()
	{
		if (popups.Count > maxPopup - 1)
			Lose();
		else
			CreatePopup(GetRandomMiniGameName());
	}

	private void StopGame()
	{
		while (popups.Count > 0)
		{
			MyPopup popup = popups.Pop();
			popup.QueueFree();
		}

		timer.Stop();
	}

	private async void Win()
	{
		StopGame();
		winScreen.SetAsToplevel(true);
		winScreen.Visible = true;
		winScreen.GetNode<AnimatedSprite>("WinAnim").Play();
		
        await ToSignal(GetTree().CreateTimer(3f), "timeout");
		isGameStoped = true;
	}

	private async void Lose()
	{
		StopGame();

		gameOverScreen.SetAsToplevel(true);
		gameOverScreen.Visible = true;

        await ToSignal(GetTree().CreateTimer(3f), "timeout");
		isGameStoped = true;
	}
}
