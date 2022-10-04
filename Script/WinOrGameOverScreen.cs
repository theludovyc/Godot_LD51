using Godot;
using System;

public class WinOrGameOverScreen : Node
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsPressed()){
            GetTree().ChangeScene("res://Scene/MainGame.tscn");
        }
    }
}
