using System.Linq;
using System.Collections.Generic;
using Godot;
using System;

public class Memory : MiniGame
{
    [Export] private Texture[] _cardTextures = null;
    [Export] private Texture _cardBack = null;
    [Export] private int _plateSize = 3;

    private static Random rng = new Random();
    private List<TextureButton> _buttons = new List<TextureButton>();
    private GridContainer _cardContainer = null;

    private bool _canPlay = true;
    private List<int> _cardSet = null;
    private int[] _selectedCards = new int[2] { -1, -1 };
    private int _selectedCount = 0;
    private int _findedPeers = 0;


    public override void _Ready()
    {
        _cardContainer = GetNode<GridContainer>("Control/CardContainer");

        InitCardContainer();
        InitButtons();
        _cardSet = GenerateCardSet();
    }

    private List<int> GenerateCardSet()
    {
        int _peerNumber = Mathf.FloorToInt(((_plateSize * _plateSize) / 2));
        List<int> numberList = Enumerable.Range(0, _cardTextures.Length).ToList();
        List<int> cardSet = new List<int>();


        ShuffleList(numberList);

        for (int i = 0; i < _peerNumber; i++)
        {
            cardSet.Add(numberList[0]);
            cardSet.Add(numberList[0]);
            numberList.RemoveAt(0);
        }

        cardSet.Add(numberList[0]);
        ShuffleList(cardSet);

        return (cardSet);
    }

    public void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void InitCardContainer()
    {
        _cardContainer.Columns = _plateSize;
    }

    private void InitButtons()
    {
        for (int i = 0; i < _plateSize * _plateSize; ++i)
        {
            TextureButton button = new TextureButton();

            button.Expand = true;
            button.TextureNormal = _cardBack;
            button.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
            button.SizeFlagsVertical = (int)Control.SizeFlags.ExpandFill;
            button.Connect("button_down", this, nameof(OnCardClick), new Godot.Collections.Array(i));
            _cardContainer.AddChild(button);
            _buttons.Add(button);
        }
    }

    private void OnCardClick(int buttonID)
    {
        if (!_canPlay)
            return;

        int cardTextureIndex = _cardSet[buttonID];
        _buttons[buttonID].TextureNormal = _cardTextures[cardTextureIndex];

        _selectedCards[_selectedCount] = buttonID;
        _selectedCount++;
        if (_selectedCount >= 2)
        {
            if (_cardSet[_selectedCards[0]] == _cardSet[_selectedCards[1]])
                ValidPeer();
            else
                WrongPeer();
        }
    }

    private void ResetCardSelection()
    {
        _selectedCards[0] = -1;
        _selectedCards[1] = -1;
        _selectedCount = 0;
    }

    private async void ValidPeer()
    {
        _buttons[_selectedCards[0]].Disabled = true;
        _buttons[_selectedCards[1]].Disabled = true;
        ResetCardSelection();

        if (++_findedPeers >= Mathf.FloorToInt(((_plateSize * _plateSize) / 2)))
        {
            await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
            EmitSignal(nameof(Win));
            return;
        }
    }

    private async void WrongPeer()
    {
        _canPlay = false;
        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        _canPlay = true;
        _buttons[_selectedCards[0]].TextureNormal = _cardBack;
        _buttons[_selectedCards[1]].TextureNormal = _cardBack;
        ResetCardSelection();
    }
}
