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
		{"DontDoDucks", null},
		{"FeedThePlant", null}
	};

	private PackedScene myPopup = GD.Load<PackedScene>("res://Scene/MyPopup.tscn");
	private Stack<MyPopup> popups = new Stack<MyPopup>();
	private Timer timer = null;
	private AnimatedSprite gameOverScreen = null;
	private bool isGameStoped = false;
	private AudioStreamPlayer audioPlayer = null;
	private MusicPlayer musicPlayer = null;
	private WindowDialog startPopup = null;
	private Label startPopupButtonLabel = null;

	void OnPopupHide()
	{
		if (popups.Count == 0 || isGameStoped)
			return;

		var popup = popups.Pop();
		popup.QueueFree();

		UpdateRamViewer();
		if (popups.Count > 0)
		{
			PlaySFX("Click");
			MyPopup focusedPopup = popups.Peek();

			focusedPopup.SetPauseSubScene(false);
			musicPlayer.SetActiveMusic(focusedPopup.MiniGame.MusicTheme);
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

		musicPlayer.SetActiveMusic(game.MusicTheme);
		currentPopup.AddChildMiniGame(game);
		GetNode("PopUps").AddChild(currentPopup);
		currentPopup.WindowTitle = miniGameName + ".exe";
		currentPopup.SetPosition(GetRandomPosition());
		currentPopup.Connect("popup_hide", this, "OnPopupHide");
		currentPopup.Popup_();
		popups.Push(currentPopup);
		UpdateRamViewer();

		if (popups.Count == maxPopup)
			PlaySFX("Warning");
	}

	private void UpdateRamViewer()
	{
		GetNode<Label>("Taskbar/RamText").Text = (string.Format("RAM: {0:00}%", (popups.Count / ((float)maxPopup + 1)) * 100));
	}

	public override void _Ready()
	{
		GetNodes();
		CreateAudioStreamPlayer();
		musicPlayer = GetNode<MusicPlayer>("MusicPlayer");
		foreach (var key in miniGames.Keys.ToList())
			miniGames[key] = GD.Load<PackedScene>($"res://Scene/MiniGames/{key}.tscn");

		StartGame();
	}

	private void StartGame()
	{
		isGameStoped = false;
		gameOverScreen.Visible = false;

		musicPlayer.PlayAll();

		startPopup.PopupCentered();

	}

	private async void CreatePopups()
	{
		await ToSignal(GetTree().CreateTimer(0.25f), "timeout");
		for (int i = 0; i < maxPopup - 1; i++)
		{
			await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
			CreatePopup(GetRandomMiniGameName());
		}
		timer.Start();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (isGameStoped && (inputEvent is InputEventKey))
			StartGame();
	}

	private void GetNodes()
	{
		gameOverScreen = GetNode<AnimatedSprite>("GameOverScreen");
		timer = GetNode<Timer>("Timer");
		startPopup = GetNode<WindowDialog>("StartPopup");
		startPopupButtonLabel = GetNode<Label>("StartPopup/Button/Label");
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
		musicPlayer.Pause();
		timer.Stop();
	}

	private void Win()
	{
		GetTree().ChangeScene("res://Scene/WinScreen.tscn");
	}

	private async void Lose()
	{
		musicPlayer.Pause();
		timer.Stop();
		isGameStoped = true;

		PlaySFX("GLITCH");
		gameOverScreen.SetAsToplevel(true);
		gameOverScreen.Visible = true;
		gameOverScreen.Frame = 0;
		gameOverScreen.Play("Glitch");

		await ToSignal(GetTree().CreateTimer(1f), "timeout");

		GetTree().ChangeScene("res://Scene/GameOver.tscn");
	}

	private void CreateAudioStreamPlayer()
	{
		audioPlayer = new AudioStreamPlayer();
		audioPlayer.Autoplay = false;
		AddChild(audioPlayer);
	}

	protected void PlaySFX(string audioName, string format = "wav")
	{
		audioPlayer.Stream = GD.Load<AudioStream>($"res://Audio/SFX/{audioName}.{format}");
		audioPlayer.Play();
	}

	void OnStartPopupButtonDown(){
		startPopup.QueueFree();

		CreatePopups();

		timer.Start();
	}

	void OnStartPopupButtonMouseEntered(){
		startPopupButtonLabel.AddColorOverride("font_color", new Color("#ffffff"));
	}

	void OnStartPopupButtonMouseExited(){
		startPopupButtonLabel.AddColorOverride("font_color", new Color("#000000"));
	}
}
