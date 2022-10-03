using Godot;
using System;

public class MyButton : TextureButton
{
    Tween tween = null;

    public override void _Ready(){
        RectPivotOffset = RectSize / 2;

        tween = GetNode<Tween>("Tween");
    }


    public void SetEnabled(bool b){
        Disabled = !b;

        if(b){
            Modulate = new Color("#ffffff");
        }else{
            Modulate = new Color("#4a4a4a");
        }
    }

    void OnMouseEntered(){
        tween.InterpolateProperty(this, "rect_scale", null, new Vector2(1.1f, 1.1f), 0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }

    void OnDynamiteMouse(){
        tween.InterpolateProperty(this, "rect_scale", null, new Vector2(1, 1), 0.2f, Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }
}
