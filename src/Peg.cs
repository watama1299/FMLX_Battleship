namespace Battleship;

public class Peg : IPeg
{
    public int Id {get; private set;}

    public PegType Type {get; private set;}

    public bool PlaceInGrid(Position pos) {
        return true;
    }
}
