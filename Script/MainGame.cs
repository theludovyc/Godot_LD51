using Godot;
using System;
using System.Collections.Generic;

public class MainGame : Control
{
    private static readonly string[] GameNames = {
        "BlueMiniGame",
        "RedMiniGame",
		"PetTheDog"
    };

	private PackedScene myPopup = GD.Load<PackedScene>("res://Scene/MyPopup.tscn");
	private Stack<MyPopup> popups = new Stack<MyPopup>();

	void OnPopupHide() {
		popups.Pop();

		if (popups.Count > 0) {
			popups.Peek().SetPauseSubScene(false);
		}
	}

	void CreatePopup(String miniGameName, int px){
		if (popups.Count > 0) {
			popups.Peek().SetPauseSubScene(true);
		}

		MyPopup currentPopup = myPopup.Instance<MyPopup>();
		PackedScene Game = GD.Load<PackedScene>($"res://Scene/MiniGames/{miniGameName}.tscn");
		MiniGame game = Game.Instance<MiniGame>();

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
        for (int i = 0; i < 10; i++)
        {
            CreatePopup(GetRandomMiniGameName(), i * 100);
        }
	}

    private string GetRandomMiniGameName()
    {
        int index = Mathf.RoundToInt(GD.Randi() % GameNames.Length);

        return (GameNames[index]);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
