using Godot;
using System;

public class MyPopup : WindowDialog
{
    private MiniGame miniGame = null;
    private ColorRect mask = null;

    public void SetPauseSubScene(bool b) {
        if (b)
            mask.Show();
        else
            mask.Hide();

        if(miniGame != null){
            miniGame.SetPauseChildren(b);
			GetNode<Viewport>("ViewportContainer/Viewport").GuiDisableInput = b;
        }
    }

    public void AddChildMiniGame(MiniGame miniGame) {
        GetNode("ViewportContainer/Viewport/").AddChild(miniGame);

        this.miniGame = miniGame;

        this.miniGame.Connect("Win", this, "OnMiniGameWin");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mask = GetNode<ColorRect>("Mask");

		HideCloseButton();
    }

	private void HideCloseButton()
	{
		TextureButton btn = GetCloseButton();

		btn.Visible = false;
		btn.QueueFree();

	}

    void OnMiniGameWin(){
        Hide();
    }
}
