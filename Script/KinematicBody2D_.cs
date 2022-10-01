using Godot;
using System;

public class KinematicBody2D_ : Godot.KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.PrintT(Name, GetTree().CurrentScene.Name);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_left")){
            Position += new Vector2(20, 0);
        }
    }
}
