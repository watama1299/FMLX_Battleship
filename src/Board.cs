namespace Battleship;

public class Board : IBoard
{
    public int Id {get; private set;}
    public IGrid<IShip> GridShip {get; private set;}
    public IGrid<IPeg> GridPeg {get; private set;}

    public Board(int squareGridSize) {
        GridShip = new Grid<IShip>(squareGridSize);
        GridPeg = new Grid<IPeg>(squareGridSize);
    }

    public Board(int rows, int cols) {
        GridShip = new Grid<IShip>(rows, cols);
        GridPeg = new Grid<IPeg>(rows, cols);
    }

    // Method to put ship on board
    

    // Method to put peg on board

}
