using Godot;
using System;

public class WinScreen : WinOrGameOverScreen
{
    public override void _Ready()
    {
        GetNode<AnimatedSprite>("WinAnim").Play();
    }
}
