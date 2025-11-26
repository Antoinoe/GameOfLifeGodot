using Godot;
using System;

public partial class GOLTimer : Timer
{
    [Export] DrawPixels targetTileMapLayer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Timeout += OnTimerTimeout;
    }

    private void OnTimerTimeout()
    {
        targetTileMapLayer.Next();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
