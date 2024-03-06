namespace Battleship;

public interface IBoard
{
    public int Id {get;}
    public IGrid<IShip> GridShip {get;}
    public IGrid<IPeg> GridPeg {get;}
}
