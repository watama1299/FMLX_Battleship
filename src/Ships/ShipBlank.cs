using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipBlank : Ship, IShip
{
    public new int ShipLength {get; private set;} = 1;

    public new Dictionary<Position, PegType> Positions {get; private set;} = new(1);

    public new bool IsAlive {get; private set;} = true;



    public new Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        return base.AssignPositions(startCoords, orientation);
    }

    public new Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositons, PegType peg = PegType.NONE) {
        return base.AssignPositions(generatedPositons, Positions, peg);
    }

    public new List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        return base.GeneratePositions(startCoords, orientation, ShipLength);
    }

    public new bool SinkShip() {
        return base.SinkShip();
    }


    // override object.ToString
    public override string ToString()
    {
        return "Blank";
    }

    // override object.Equals
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return true;
    }
    
    // override object.GetHashCode
    public override int GetHashCode() {
        return this.GetType().GetHashCode();
    }
}
