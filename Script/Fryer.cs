using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Fryer : MiniGame
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    static readonly Texture[] potatoes_tex = new []{
        GD.Load<Texture>("res://Art/Fryer/candy.png"),
        GD.Load<Texture>("res://Art/Fryer/chad.png"),
        GD.Load<Texture>("res://Art/Fryer/chrisP.png"),
        GD.Load<Texture>("res://Art/Fryer/vicky.png")
    };

    TextureRect myPotatoes = null;

    int[] indexes;

    int currentIndex = 0;

    bool loose = false;

    void shuffleIndexes(){
        GD.Randomize();

        indexes = indexes.OrderBy(x => GD.Randi()).ToArray();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        indexes = new int[potatoes_tex.Length];

        for(var i = 0; i < potatoes_tex.Length; ++i){
            indexes[i] = i;
        }

        shuffleIndexes();

        myPotatoes = GetNode<TextureRect>("MyPotatoes");

        myPotatoes.Texture = potatoes_tex[indexes[currentIndex]];
    }

    void OnButtonDown(bool b){
        currentIndex += 1;

        if(b && !loose){
            loose = true;
        }

        if(currentIndex >= potatoes_tex.Length){
            if(loose){
                shuffleIndexes();

                loose = false;

                currentIndex = 0;
            }else{
                EmitSignal(nameof(Win));
                return;
            }
        }

        myPotatoes.Texture = potatoes_tex[indexes[currentIndex]];
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
