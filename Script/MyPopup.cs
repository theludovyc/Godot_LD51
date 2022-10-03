using Godot;
using System;

public class MyPopup : WindowDialog
{
	private MiniGame miniGame = null;
	private ColorRect mask = null;
	private AudioStreamPlayer2D audioPlayer = null;

	public MiniGame MiniGame {get => miniGame;}

	public void SetPauseSubScene(bool b)
	{
		if (b)
			mask.Show();
		else
			mask.Hide();

		if (miniGame != null)
		{
			miniGame.SetPauseChildren(b);
			GetNode<Viewport>("ViewportContainer/Viewport").GuiDisableInput = b;
		}
	}

	public void AddChildMiniGame(MiniGame miniGame)
	{
		GetNode("ViewportContainer/Viewport/").AddChild(miniGame);

		this.miniGame = miniGame;
		this.miniGame.Connect("Win", this, "OnMiniGameWin");

		CreateAudioStreamPlayer();
		this.miniGame.SetAudioPlayer(audioPlayer);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mask = GetNode<ColorRect>("Mask");

		PlaySFX("NewPopUp");
		HideCloseButton();
	}

	private void CreateAudioStreamPlayer()
	{
		if (audioPlayer != null)
			return;
		audioPlayer = new AudioStreamPlayer2D();
		audioPlayer.Autoplay = false;
		AddChild(audioPlayer);
	}

	protected void PlaySFX(string audioName, string format = "wav")
	{
		if (audioPlayer == null)
			CreateAudioStreamPlayer();
		audioPlayer.Stream = GD.Load<AudioStream>($"res://Audio/SFX/{audioName}.{format}");
		audioPlayer.Play();
	}

	private void HideCloseButton()
	{
		TextureButton btn = GetCloseButton();

		btn.Visible = false;
		btn.QueueFree();

	}

	void OnMiniGameWin()
	{
		Hide();
	}
}
