using Godot;
using System;

public class MyPopup : WindowDialog
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private MiniGame miniGame = null;

    public void SetPauseSubScene(bool b) {
        if(miniGame != null){
            miniGame.SetPauseChildren(b);
        }
    }

    public void AddChildMiniGame(MiniGame miniGame) {
        GetNode("ViewportContainer/Viewport/").AddChild(miniGame);

        this.miniGame = miniGame;

        this.miniGame.Connect("Win", this, "OnMiniGameWin");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //subScene = GetNode<Node_>("ViewportContainer/Viewport/Node");

        //subScene.PauseChildren();
    }

    void OnMiniGameWin(){
        Hide();
    }
}
