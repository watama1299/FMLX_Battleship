namespace Battleship;

public interface IPeg
{
    public int Id {get;}
    public PegType Type {get;}
    public bool PlaceInGrid(Position pos);
}
