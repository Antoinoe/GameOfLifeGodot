using Godot;
using System;

public partial class DrawPixels : TileMapLayer
{
    [Export] bool startRandom = false;
    [Export] int sizeW = 100;
    [Export] int sizeH = 100;
    [Export] Vector2 cameraOffset = new Vector2(0, 0);
    private bool[,] cellsState;
    int[] rules = { -1,-1, 0, 1, -1, -1,-1,-1,-1,1 };
    public override void _Ready()
    {
        Init();
    }

    public override void _Process(double delta)
    {

    }

    void DebugArray()
    {
        int n = 0;
        for(int i = 0; i< sizeH; i++)
        {
            for(int j=0; j< sizeW; j++)
            {
                if (cellsState[j, i])
                    n++;
            }
        }
        GD.Print($"Alive cells: {n}");  
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventKey keyEvent
            && keyEvent.Pressed
            && keyEvent.Keycode == Key.Space)
        {
            Next();
        }
    }

    private bool[,] CreatePrimordialSoup()
    {
        GD.Print("Hmmm... S O U P ! https://www.youtube.com/watch?v=SuEmD9WRKes");

        var result = new bool[sizeW, sizeH];
        for (int x = 0; x < sizeW; x++)
        {
            for (int y = 0; y < sizeH; y++)
            {
                result[x, y] = Random.Shared.Next(2) == 0;
            }
        }

        return result;
    }

    private void Init()
    {
        cellsState = new bool[sizeW, sizeH];
        if(startRandom)
        { 
            cellsState = CreatePrimordialSoup();
            DisplayCells();
        }
        else
        {
            cellsState = UpdateCellArray();
            DebugArray();
        }
            
    }

    void DisplayCells()
    {
        for (int i = 0; i < sizeH; i++)
        {
            for (int j = 0; j < sizeW; j++)
            {
                SetCell(new Vector2I(j, i), 2, atlasCoords: cellsState[j, i] ? new Vector2I(1, 0) : new Vector2I(0, 0));
            }
        }
        UpdateInternals();
    }

    private bool[,] UpdateCellArray()
    {
        var result = new bool[sizeW, sizeH];
        for (int i = 0; i < sizeH; i++)
        {
            for (int j = 0; j < sizeW; j++)
            {
                result[j, i] = GetCellAtlasCoords(new Vector2I(j, i)) == new Vector2I(1, 0);
            }
        }
        return result;
    }

    private readonly Vector2I[] NeighOffsets =
    {
        new Vector2I(-1, -1), // top-left
        new Vector2I(0, -1),  // top
        new Vector2I(1, -1),  // top-right
        new Vector2I(-1, 0),  // left
        new Vector2I(1, 0),   // right
        new Vector2I(-1, 1),  // bottom-left
        new Vector2I(0, 1),   // bottom
        new Vector2I(1, 1)    // bottom-right
    };
    bool isCalculating = false;
    public void Next()
    {
        if(isCalculating) return;
        isCalculating = true;
        bool[,] newState = new bool[sizeW, sizeH];
        for (int i = 0; i < sizeH; i++)
        {
            for (int j = 0; j < sizeW; j++)
            {
                int aliveNeighboors = 0;
                foreach (var o in NeighOffsets)
                {
                    int nx = j + o.X;
                    int ny = i + o.Y;

                    if (nx < 0 || nx >= sizeW) continue;
                    if (ny < 0 || ny >= sizeH) continue;

                    if (cellsState[nx, ny])
                        aliveNeighboors++;
                }
                int ruleIndex = rules[aliveNeighboors];
                newState[j, i] = ruleIndex switch
                {
                    -1 => false,
                    0 => GetCellAtlasCoords(new Vector2I(j, i)).X == 1,
                    1 => true,
                };
            }
        }
        cellsState = newState;
        DebugArray();
        DisplayCells();
        isCalculating = false;
    }
}
