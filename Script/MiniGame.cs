using Godot;
using System;

public class MiniGame : Node
{
    [Signal]
    protected delegate void Win();

    public void SetPauseChildren(bool b){
        foreach(Node node in GetChildren()){
            node.SetProcess(!b);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
