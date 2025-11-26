using Godot;
using System;

public partial class NextGenerationButton : Button
{
    [Export] DrawPixels targetTileMapLayer;
    public override void _Ready()
    {
        Pressed += ButtonPressed;
    }

    private void ButtonPressed()
    {
        targetTileMapLayer.Next();
    }
}
