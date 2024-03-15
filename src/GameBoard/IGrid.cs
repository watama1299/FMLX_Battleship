using Battleship.Utils;
namespace Battleship.GameBoard;

public interface IGrid<T>
{
    public T[,] Items {get;}
    public int TotalGrid {get;}


    
    public bool PlaceItemOnGrid(Position position, T item);
    public bool PlaceItemOnGrid(IDictionary<Position, T> itemAndPositions);
    public bool ContainsPosition(Position position);
    public bool IsPositionEmpty(Position position);
    public bool IsPositionEmpty(IEnumerable<Position> positions);
}
