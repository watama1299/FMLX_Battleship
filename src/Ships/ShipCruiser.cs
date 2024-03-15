using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipCruiser : Ship, IShip
{
    public new int ShipLength {get; private set;} = 3;

    public new Dictionary<Position, PegType> Positions {get; private set;} = new(3);

    public new bool IsAlive {get; private set;} = true;



    private ShipCruiser(int length, Dictionary<Position, PegType> pos, bool isAlive) {
        ShipLength = length;
        Positions = pos;
        IsAlive = isAlive;
    }
    public ShipCruiser() {}



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
        return "Cruiser";
    }

    // override object.Equals
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return ShipLength == ((ShipCruiser) obj).ShipLength
            && Positions == ((ShipCruiser) obj).Positions
            && IsAlive == ((ShipCruiser) obj).IsAlive;
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

    public override IShip Clone()
    {
        return new ShipCruiser(ShipLength, Positions, IsAlive);
    }
}
