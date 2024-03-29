using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

/// <summary>
/// Concrete class representing a Carrier ship piece in a normal game of Battleship
/// </summary>
public class ShipCarrier : Ship, IShip
{
    public override int ShipLength {get; protected set;} = 5;

    public override IDictionary<Position, PegType> Positions {get; protected set;}

    public override bool IsAlive {get; protected set;} = true;



    /// <summary>
    /// Private constructor, only used during cloning process
    /// </summary>
    /// <param name="pos">List of positions</param>
    /// <param name="isAlive">Alive(true) / Dead(false)</param>
    private ShipCarrier(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }

    /// <summary>
    /// Constructor for the ship type Carrier
    /// </summary>
    public ShipCarrier() {
        Positions = new Dictionary<Position, PegType>();
    }



    // override object.ToString
    public override string ToString()
    {
        return "Carrier";
    }

    public override IShip Clone()
    {
        return new ShipCarrier(Positions, IsAlive);
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
