using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class MainGame : Control
{
	Dictionary<string, PackedScene> miniGames = new Dictionary<string, PackedScene>{
		{"BlueMiniGame", null},
		{"RedMiniGame", null},
		{"PetTheDog", null}
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
		MiniGame game = miniGames[miniGameName].Instance<MiniGame>();

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
		foreach(var key in miniGames.Keys.ToList()){
			miniGames[key] = GD.Load<PackedScene>($"res://Scene/MiniGames/{key}.tscn");
		}

        for (int i = 0; i < 10; i++)
        {
            CreatePopup(GetRandomMiniGameName(), i * 100);
        }
	}

    private string GetRandomMiniGameName()
    {
        int index = Mathf.RoundToInt(GD.Randi() % miniGames.Keys.Count);

        return miniGames.Keys.ToArray()[index];
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
