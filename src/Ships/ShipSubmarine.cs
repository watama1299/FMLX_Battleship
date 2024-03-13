using Battleship.Enums;
namespace Battleship.Ships;

public class ShipSubmarine : Ship, IShip
{
    public new int ShipLength {get; private set;} = 3;

    public new Dictionary<Position, PegType> Positions {get; private set;} = new(3);

    public new bool IsAlive {get; private set;} = true;



    public new Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation)
    {
        return base.AssignPositions(startCoords, orientation);
    }

    public new Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositons, PegType peg = PegType.NONE)
    {
        return base.AssignPositions(generatedPositons, peg);
    }

    public new List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation)
    {
        return base.GeneratePositions(startCoords, orientation);
    }

    public new bool SinkShip()
    {
        return base.SinkShip();
    }
}
