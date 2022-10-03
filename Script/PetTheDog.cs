using Godot;
using System;

public class PetTheDog : MiniGame
{
    AnimatedSprite anim = null;

    bool canClick = true;

    TextureButton[] buttons;
	public override MusicTheme MusicTheme {get => MusicTheme.Chiptune;}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");

        buttons = new TextureButton[3]{GetNode<TextureButton>("Dynamite"), GetNode<TextureButton>("Knife"), GetNode<TextureButton>("Hand")};
    }

    void disableButtons(){
        foreach(var button in buttons){
            button.Disabled = true;

            button.Modulate = new Color("#4a4a4a");
        }
    }

    void enableButtons(){
        foreach(var button in buttons){
            button.Disabled = false;

            button.Modulate = new Color("#ffffff");
        }
    }

	protected override void OnFocus()
	{
		PlaySFX("Dog");
	}

    void OnButtonDown(String s){
        if(canClick){
            anim.Play(s);
            
            disableButtons();
        }
    }

    void OnAnimationFinished(){
        switch(anim.Animation){
            case "Hand":
                EmitSignal(nameof(Win));
            break;

            case "Idle":
            break;

            default:
                anim.Play("Idle");

                enableButtons();
            break;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
