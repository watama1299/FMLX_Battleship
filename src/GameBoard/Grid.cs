using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.GameBoard;

/// <summary>
/// Concrete class which implements the <c>IGrid</c> interface
/// </summary>
/// <typeparam name="T">Any object which will be contained within the grid</typeparam>
public class Grid<T> : IGrid<T>
{
    public T[,] Items {get; private set;}
    public int TotalGrid {get; private set;}
    

    /// <summary>
    /// Constructor for a square shaped grid
    /// </summary>
    /// <param name="size">Length of one side in units/boxes</param>
    public Grid(int size) {
        Items = new T[size, size];
        TotalGrid = size * size;
    }

    /// <summary>
    /// Constructor for a rectangle shaped grid
    /// </summary>
    /// <param name="rows">Number of rows</param>
    /// <param name="cols">Number of columns</param>
    public Grid(int rows, int cols) {
        Items = new T[rows, cols];
        TotalGrid = rows * cols;
    }


    
    public bool PlaceItemOnGrid(Position position, T item) {
        if (!IsPositionEmpty(position)) return false;
        Items[position.X, position.Y] = item;
        return true;
    }

    public bool PlaceItemOnGrid(IDictionary<Position, T> itemAndPositions) {
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
        if (Items[position.X, position.Y] is null
            || Equals(Items[position.X, position.Y], PegType.NONE)) 
            return true;
        return false;
    }

    public bool IsPositionEmpty(IEnumerable<Position> positions) {
        foreach (var pos in positions) {
            if (!IsPositionEmpty(pos)) return false;
        }
        return true;
    }
}
