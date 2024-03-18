using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipSubmarine : Ship, IShip
{
    public new int ShipLength {get; private set;} = 3;

    public new IDictionary<Position, PegType> Positions {get; private set;}

    public new bool IsAlive {get; private set;} = true;



    private ShipSubmarine(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }
    public ShipSubmarine() {
        Positions = new Dictionary<Position, PegType>();
    }




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
        return "Submarine";
    }

    public override IShip Clone()
    {
        return new ShipSubmarine(Positions, IsAlive);
    }

    // override object.Equals
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return ShipLength == ((ShipSubmarine) obj).ShipLength
            && Positions == ((ShipSubmarine) obj).Positions
            && IsAlive == ((ShipSubmarine) obj).IsAlive;
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
