using Godot;
using System;
using System.Collections.Generic;

public class MainGame : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	PackedScene MyPopup = GD.Load<PackedScene>("res://Scene/MyPopup.tscn");

	Stack<MyPopup> popups = new Stack<MyPopup>();

	void OnPopupHide(){
		popups.Pop();

		if(popups.Count > 0){
			popups.Peek().SetPauseSubScene(false);
		}
	}

	void CreatePopup(String miniGameName, int px){
		if(popups.Count > 0){
			popups.Peek().SetPauseSubScene(true);
		}

		var currentPopup = MyPopup.Instance<MyPopup>();

		var Game = GD.Load<PackedScene>($"res://Scene/{miniGameName}.tscn");

		var game = Game.Instance<MiniGame>();

		currentPopup.AddChildMiniGame(game);

		AddChild(currentPopup);

		currentPopup.SetPosition(new Vector2(px, 0));

		currentPopup.Connect("popup_hide", this, "OnPopupHide");

		currentPopup.Popup_();

		popups.Push(currentPopup);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(var i = 0; i < 5; ++i){
			CreatePopup("MiniGame", i*100);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
