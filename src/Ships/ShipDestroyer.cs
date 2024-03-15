using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipDestroyer : Ship, IShip
{
    public new int ShipLength {get; private set;} = 2;

    public new Dictionary<Position, PegType> Positions {get; private set;} = new(2);

    public new bool IsAlive {get; private set;} = true;



    private ShipDestroyer(int length, IDictionary<Position, PegType> pos, bool isAlive) {
        ShipLength = length;
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }
    public ShipDestroyer() {}




    public new IEnumerable<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        return base.GeneratePositions(startCoords, orientation, ShipLength);
    }
    public new IDictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        return base.AssignPositions(startCoords, orientation);
    }

    public new IDictionary<Position, PegType> AssignPositions(IEnumerable<Position> generatedPositons, PegType peg = PegType.NONE) {
        return base.AssignPositions(generatedPositons, Positions, peg);
    }

    public new bool SinkShip() {
        return base.SinkShip();
    }



    // override object.ToString
    public override string ToString()
    {
        return "Destroyer";
    }

    public override IShip Clone()
    {
        return new ShipDestroyer(ShipLength, Positions, IsAlive);
    }

    // override object.Equals
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return ShipLength == ((ShipDestroyer) obj).ShipLength
            && Positions == ((ShipDestroyer) obj).Positions
            && IsAlive == ((ShipDestroyer) obj).IsAlive;
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
