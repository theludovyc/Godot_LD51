using System.Collections.Generic;
using Godot;
using System;

public enum MusicTheme
{
	Troll,
	Chiptune,
	HipHop,
	Synthwave
}

public class MusicPlayer : Node
{
	[Export] private MusicTheme startTheme = MusicTheme.Troll;
	[Export] private float musicVolume = 0f;

	Dictionary<MusicTheme, AudioStream> streams = new Dictionary<MusicTheme, AudioStream>();
	Dictionary<MusicTheme, AudioStreamPlayer> players = new Dictionary<MusicTheme, AudioStreamPlayer>();

	public override void _Ready()
	{
		InitMusicsStreamDict();
		InitMusicPlayerDict();

		SetActiveMusic(startTheme);
	}

	public void SetActiveMusic(MusicTheme theme)
	{
		foreach (var item in players)
			item.Value.VolumeDb = item.Key == theme ? musicVolume : -80;
	}

	public void PlayAll()
	{
		foreach (var item in players)
			item.Value.Play();
	}

	private void InitMusicsStreamDict()
	{
		streams.Add(MusicTheme.Troll, GD.Load<AudioStream>("res://Audio/Musiques/Troll.mp3"));
		streams.Add(MusicTheme.Chiptune, GD.Load<AudioStream>("res://Audio/Musiques/Chiptune.mp3"));
		streams.Add(MusicTheme.HipHop, GD.Load<AudioStream>("res://Audio/Musiques/HipHop.mp3"));
		streams.Add(MusicTheme.Synthwave, GD.Load<AudioStream>("res://Audio/Musiques/Synthwave.mp3"));
	}

	private void InitMusicPlayerDict()
	{
		foreach (var item in streams)
		{
			AudioStreamPlayer player = new AudioStreamPlayer();

			player.Autoplay = false;
			player.Stream = item.Value;
			players.Add(item.Key, player);
			AddChild(player);
		}
	}

	public void Pause()
	{
		foreach (var item in players)
			item.Value.Stop();
	}
}
