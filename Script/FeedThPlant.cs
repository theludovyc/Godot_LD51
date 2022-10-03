using Godot;
using System;

public class FeedThPlant : MiniGame
{
	AnimatedSprite anim = null;

	bool canClick = true;

	TextureButton[] buttons;
	public override MusicTheme MusicTheme { get => MusicTheme.HipHop; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		anim = GetNode<AnimatedSprite>("AnimatedSprite");

		buttons = new TextureButton[5] {
			GetNode<TextureButton>("Fish"),
			GetNode<TextureButton>("Bone"),
			GetNode<TextureButton>("Human"),
			GetNode<TextureButton>("Pizza"),
			GetNode<TextureButton>("Bee")
		};
	}

	void disableButtons()
	{
		foreach (var button in buttons)
		{
			button.Disabled = true;

			button.Modulate = new Color("#4a4a4a");
		}
	}

	void enableButtons()
	{
		foreach (var button in buttons)
		{
			button.Disabled = false;

			button.Modulate = new Color("#ffffff");
		}
	}

	protected override void OnFocus()
	{
	}

	void OnButtonDown(String s)
	{
		if (canClick)
		{
			anim.Play(s);

			disableButtons();
		}
	}

	void OnAnimationFinished()
	{
		switch (anim.Animation)
		{
			case "Yes":
				EmitSignal(nameof(Win));
				break;

			case "Idle":
				break;

			default:
				anim.Play("Idle");

				enableButtons();
				break;
		}
	}
}
