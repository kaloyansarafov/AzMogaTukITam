namespace AzMogaTukITam.Model;

public class SelectedLayer : LayerBase
{
    private Coordinates _currentPointer = new Coordinates();

    public SelectedLayer(Grid grid)
        : base(grid)
    {
    }

    public override int ZIndex { get; protected set; } = 200;

    public override DisplayValue DisplayValue { get; protected set; } = new DisplayValue()
        {Value = '+', DisplayBackground = ConsoleColor.DarkCyan, DisplayForeground = ConsoleColor.White};

    public override bool[,] Data { get; protected set; }
    public override int ConsolePriority { get; protected set; }
    public override Action<Game, ConsoleKeyInfo> ConsoleAction { get; protected set; }
    public override Action<Game> UpdateAction { get; protected set; }

    public Coordinates CurrentPointer => _currentPointer;

    public override int RequiredTurns { get; protected set; } = 0;

    public Coordinates SetCurrentPointer(Coordinates cord)
    {
        if (cord.X < 0 || cord.X > this.Data.GetLength(1) || cord.Y < 0 || cord.Y > this.Data.GetLength(0))
            return _currentPointer;
        this.ClearCurrentPointer();
        _currentPointer = cord;
        this.Data[_currentPointer.Y, _currentPointer.X] = true;
        return _currentPointer;
    }

    public Coordinates MoveCurrentPointer(Coordinates rel)
    {
        if (_currentPointer.X + rel.X < 0 || _currentPointer.X + rel.X >= this.Data.GetLength(1) ||
            _currentPointer.Y + rel.Y < 0 || _currentPointer.Y + rel.Y >= this.Data.GetLength(0)) return _currentPointer;
        this.ClearCurrentPointer();
        _currentPointer.X += rel.X;
        _currentPointer.Y += rel.Y;
        this.Data[_currentPointer.Y, _currentPointer.X] = true;
        return _currentPointer;
    }

    public void ClearCurrentPointer()
    {
        for (int y = 0; y < this.Data.GetLength(0); y++)
        for (int x = 0; x < this.Data.GetLength(1); x++)
            this.Data[y, x] = false;
    }
}