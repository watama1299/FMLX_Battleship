using Battleship.Utils;
namespace Battleship.GameBoard;

public class Grid<T> : IGrid<T>
{
    public T[,] Items {get; private set;}
    public int TotalGrid {get; private set;}
    


    public Grid(int size) {
        Items = new T[size, size];
        TotalGrid = size * size;
    }
    public Grid(int rows, int cols) {
        Items = new T[rows, cols];
        TotalGrid = rows * cols;
    }


    
    public bool PlaceItemOnGrid(Position position, T item) {
        if (!IsPositionEmpty(position)) return false;
        Items[position.X, position.Y] = item;
        return true;
    }
    public bool PlaceItemOnGrid(Dictionary<Position, T> itemAndPositions) {
        foreach (var posKey in itemAndPositions.Keys) {
            if (!IsPositionEmpty(posKey)) return false;
        }

        foreach (var kv in itemAndPositions) {
            Items[kv.Key.X, kv.Key.Y] = kv.Value;
        }
        return true;
    }



    public bool ContainsPosition(Position position) {
        try {
            var item = Items[position.X, position.Y];
        } catch (Exception) {
            return false;
        }
        return true;
    }
    public bool IsPositionEmpty(Position position) {
        if (!ContainsPosition(position)) return false;
        if (Items[position.X, position.Y] is null) return true;
        return false;
    }
    public bool IsPositionEmpty(List<Position> positions) {
        foreach (var pos in positions) {
            if (!IsPositionEmpty(pos)) return false;
        }
        return true;
    }
}
