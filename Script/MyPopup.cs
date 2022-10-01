using Godot;
using System;

public class MyPopup : WindowDialog
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    MiniGame subScene = null;

    public void SetPauseSubScene(bool b){
        if(subScene != null){
            subScene.SetPauseChildren(b);
        }
    }

    public void AddChildMiniGame(MiniGame miniGame){
        GetNode("ViewportContainer/Viewport/").AddChild(miniGame);

        subScene = miniGame;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //subScene = GetNode<Node_>("ViewportContainer/Viewport/Node");

        //subScene.PauseChildren();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
