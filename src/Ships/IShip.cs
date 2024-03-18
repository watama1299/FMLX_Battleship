using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public interface IShip
{
    public abstract int ShipLength {get;}
    public abstract IDictionary<Position, PegType> Positions {get;}
    public abstract bool IsAlive {get;}



    public IEnumerable<Position> GeneratePositions(Position startCoords, ShipOrientation orientation);
    public IDictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation);
    public IDictionary<Position, PegType> AssignPositions(IEnumerable<Position> generatedPositons, PegType peg = PegType.NONE);
    public PegType GetPegOnPosition(Position pos);
    public bool SetPegOnPosition(Position pos, PegType peg);
    public bool SinkShip();



    public string ToString();
    public IShip Clone();
}
