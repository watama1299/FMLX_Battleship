using Battleship.Enums;
namespace Battleship.Ships;

public interface IShip
{
    public int ShipLength {get;}
    public Dictionary<Position, PegType> Positions {get;}
    public bool IsAlive {get;}

    public Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation);
    public Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositons, PegType peg = PegType.NONE);

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation);
    public bool SinkShip();
}
