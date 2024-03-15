using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public interface IShip
{
    public abstract int ShipLength {get;}
    public abstract Dictionary<Position, PegType> Positions {get;}
    public abstract bool IsAlive {get;}

    public Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation);
    public Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositons, PegType peg = PegType.NONE);

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation);
    public bool SinkShip();

    public string ToString();
    public IShip Clone();
    public PegType GetPegOnPosition(Position pos);
    public bool SetPegOnPosition(Position pos, PegType peg);
}
