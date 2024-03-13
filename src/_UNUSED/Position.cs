using Battleship.Enums;

namespace Battleship.UNUSED;

public class Position : IPosition
{
    public int X {get; set;}
    public int Y {get; set;}

    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    public SolidShip GetShip(Position pos) {
        return new SolidShip(ShipType.DESTROYER);
    }
}
