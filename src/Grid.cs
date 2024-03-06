namespace Battleship;

public class Grid<T> : IGrid<T>
{
    public T[,] Pieces {get; private set;}
    
    public Grid(T[,] pieces) {
        Pieces = pieces;
    }
}
