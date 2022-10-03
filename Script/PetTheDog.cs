using Godot;
using System;

public class PetTheDog : MiniGame
{
    AnimatedSprite anim = null;

	public override MusicTheme MusicTheme {get => MusicTheme.Chiptune;}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
    }

	protected override void OnFocus()
	{
		PlaySFX("Dog");
	}

    void OnButtonDown(String s){
        anim.Play(s);
    }

    void OnAnimationFinished(){
        switch(anim.Animation){
            case "Hand":
                EmitSignal(nameof(Win));
            break;

            default:
                anim.Play("Idle");
            break;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
