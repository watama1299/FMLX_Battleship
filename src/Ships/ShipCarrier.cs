using Battleship.Enums;
namespace Battleship.Ships;

public class ShipCarrier : Ship, IShip
{
    public new int ShipLength {get; private set;} = 5;

    public new Dictionary<Position, PegType> Positions {get; private set;} = new(5);

    public new bool IsAlive {get; private set;} = true;



    public new Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        return base.AssignPositions(startCoords, orientation);
    }

    public new Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositons, PegType peg = PegType.NONE) {
        return base.AssignPositions(generatedPositons, peg);
    }

    public new List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        return base.GeneratePositions(startCoords, orientation);
    }

    public new bool SinkShip() {
        return base.SinkShip();
    }



    // override object.Equals
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return ShipLength == ((ShipCarrier) obj).ShipLength
            && Positions == ((ShipCarrier) obj).Positions
            && IsAlive == ((ShipCarrier) obj).IsAlive;
    }
    
    // override object.GetHashCode
    public override int GetHashCode() {
        return HashCode.Combine(
            this.GetType().GetHashCode(),
            ShipLength,
            Positions,
            IsAlive
        );
    }
}
