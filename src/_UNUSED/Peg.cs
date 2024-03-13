using Battleship.Utils.Enums;

namespace Battleship.UNUSED;

public class Peg : IPeg
{
    public int Id {get; private set;}

    public PegType Type {get; private set;}

    // public bool PlaceInGrid(Position pos) {
    //     return true;
    // }
}
