using Battleship.Enums;

namespace Battleship;

public interface IShip
{
    // public int Id {get;}
    public Dictionary<Position, PegType> Positions {get;}
    public bool HasSunk {get;}

    public Dictionary<Position, PegType> PlaceShip(Position startCoords, ShipOrientation orientation);
    public Dictionary<Position, PegType> PlaceShip(List<Position> generatedPositons);

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation);
    public bool SinkShip();
}
