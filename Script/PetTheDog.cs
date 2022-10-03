using Godot;
using System;
using System.Linq;

public class PetTheDog : MiniGame
{
    AnimatedSprite anim = null;

    bool canClick = true;

	public override MusicTheme MusicTheme {get => MusicTheme.Chiptune;}

    MyButton[] buttons;

    int[] indexes;

    void shuffleIndexes(){
        indexes = indexes.OrderBy(x => GD.Randi()).ToArray();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");

        buttons = new MyButton[3]{GetNode<MyButton>("Dynamite"), GetNode<MyButton>("Knife"), GetNode<MyButton>("Hand")};

        indexes = new int[buttons.Length];

        for(var i = 0; i < buttons.Length; ++i){
            indexes[i] = i;
        }

        shuffleIndexes();

        Vector2[] buttonPositions = new Vector2[buttons.Length];

        for(var i = 0; i < buttons.Length; ++i){
            buttonPositions[i] = buttons[i].RectPosition;
        }

        for(var i = 0; i < buttons.Length; ++i){
            buttons[i].RectPosition = buttonPositions[indexes[i]];
        }
    }

    void disableButtons(){
        foreach(var button in buttons){
            button.SetEnabled(false);
        }
    }

    void enableButtons(){
        foreach(var button in buttons){
            button.SetEnabled(true);
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

			PlaySFX("Dog_" + s);
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
