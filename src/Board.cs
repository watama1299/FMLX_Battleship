namespace Battleship;

public class Board : IBoard
{
    public int Id {get; private set;}
    public IGrid<IShip> GridShip {get; private set;}
    public IGrid<IPeg> GridPeg {get; private set;}

    public Board(IGrid<IShip> gridShip, IGrid<IPeg> gridPeg) {
        GridShip = gridShip;
        GridPeg = gridPeg;
    }
}
