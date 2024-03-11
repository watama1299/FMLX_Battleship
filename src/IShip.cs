using Battleship.Enums;

namespace Battleship;

public interface IShip
{
    public Dictionary<Position, PegType> Positions {get;}
    public bool IsAlive {get;}

    public Dictionary<Position, PegType> PlaceShip(Position startCoords, ShipOrientation orientation);
    public Dictionary<Position, PegType> PlaceShip(List<Position> generatedPositons, PegType peg = PegType.NONE);

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation);
    public bool SinkShip();
}
