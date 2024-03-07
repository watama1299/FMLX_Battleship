namespace Battleship;

public interface IGrid<T>
{
    public T[,] Items {get;}
    public bool PlaceOnGrid(Position position, T item);
    public bool PlaceOnGrid(Dictionary<Position, T> itemAndPositions);
    public bool IsPositionEmpty(Position position);
    public bool IsPositionEmpty(List<Position> positions);
}
