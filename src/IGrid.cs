namespace Battleship;

public interface IGrid<T>
{
    public T[,] Items {get;}
    public int TotalGrid {get;}
    public bool PlaceItemOnGrid(Position position, T item);
    public bool PlaceItemOnGrid(Dictionary<Position, T> itemAndPositions);
    public bool ContainsPosition(Position position);
    public bool IsPositionEmpty(Position position);
    public bool IsPositionEmpty(List<Position> positions);
}
