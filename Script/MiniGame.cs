using System.Collections.Generic;
using System.Linq;
using Godot;
using System;

public class MiniGame : Node
{
	[Signal]
	protected delegate void Win();
	private AudioStreamPlayer2D audioPlayer = null;

	public virtual MusicTheme MusicTheme {get => MusicTheme.Chiptune;}


	public void SetAudioPlayer(AudioStreamPlayer2D audioPlayer)
	{
		this.audioPlayer = audioPlayer;
	}

	public void SetPauseChildren(bool b)
	{
		List<Node> childs = GetAllChilds(this);

		childs.ForEach((c) => PauseNode(c, b));
		if (b)
			OnUnFocus();
		else
			OnFocus();
	}

	protected virtual void OnFocus() { }
	protected virtual void OnUnFocus() { }

	private void PauseNode(Node node, bool isPaused)
	{
		node.SetProcess(!isPaused);
		node.SetPhysicsProcess(!isPaused);

		if (node is AnimatedSprite)
		{
			AnimatedSprite animSprite = (AnimatedSprite)node;
			if (isPaused)
				animSprite.Stop();
			else
				animSprite.Play();
		}
	}

	private List<Node> GetAllChilds(Node node, List<Node> childs = null)
	{
		if (childs == null)
			childs = new List<Node>();

		childs.Add(node);
		foreach (Node child in node.GetChildren())
			childs = GetAllChilds(child, childs);
		return (childs);
	}

	protected void PlaySFX(string audioName, string format = "wav")
	{
		audioPlayer.Stream = GD.Load<AudioStream>($"res://Audio/SFX/{audioName}.{format}");
		audioPlayer.Play();
	}
}
