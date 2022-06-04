namespace AzMogaTukITam.Model;

public class SelectedLayer : LayerBase
{

    private Coordinates currentPointer = new Coordinates();
    
    public SelectedLayer(Grid grid)
        : base(grid)
    {}

    public override int ZIndex { get; protected set; } = 100;
    public override DisplayValue DisplayValue { get; protected set; } = new DisplayValue() { Value = '+', DisplayBackground = ConsoleColor.DarkCyan, DisplayForeground = ConsoleColor.White };
    public override bool[,] Data { get; protected set; }
    public override int ConsolePriority { get; protected set; }
    public override Action<Game, ConsoleKeyInfo> ConsoleAction { get; protected set; }
    public override Action<Game> UpdateAction { get; protected set; }

    public Coordinates CurrentPointer => currentPointer;

    public override int RequiredTurns { get; protected set; } = 0;

    public Coordinates SetCurrentPointer(Coordinates cord)
    {
        if (cord.X < 0 || cord.X > this.Data.GetLength(1) || cord.Y < 0 || cord.Y > this.Data.GetLength(0)) return currentPointer; 
        this.ClearCurrentPointer();
        currentPointer = cord;
        this.Data[currentPointer.Y, currentPointer.X] = true;
        return currentPointer;
    }

    public Coordinates MoveCurrentPointer(Coordinates rel)
    {
        if (currentPointer.X + rel.X < 0 || currentPointer.X + rel.X >= this.Data.GetLength(1) || currentPointer.Y + rel.Y < 0 || currentPointer.Y + rel.Y >= this.Data.GetLength(0)) return currentPointer; 
        this.ClearCurrentPointer();
        currentPointer.X += rel.X;
        currentPointer.Y += rel.Y;
        this.Data[currentPointer.Y, currentPointer.X] = true;
        return currentPointer;
    }

    public void ClearCurrentPointer()
    {
        this.SetCurrentPointer(new Coordinates(){ X = 0, Y = 0 });
        for (int y = 0; y < this.Data.GetLength(0); y++)
            for (int x = 0; x < this.Data.GetLength(1); x++)
                this.Data[y, x] = false;
    }

}