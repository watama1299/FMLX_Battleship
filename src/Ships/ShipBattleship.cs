using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipBattleship : Ship
{
    public override int ShipLength {get; protected set;} = 4;

    public override IDictionary<Position, PegType> Positions {get; protected set;}

    public override bool IsAlive {get; protected set;} = true;



    /// <summary>
    /// Private constructor, only used during cloning process
    /// </summary>
    /// <param name="pos">List of positions</param>
    /// <param name="isAlive">Alive(true) / Dead(false)</param>
    private ShipBattleship(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }

    /// <summary>
    /// Constructor for the ship type Battleship
    /// </summary>
    public ShipBattleship() {
        Positions = new Dictionary<Position, PegType>();
    }



    // override object.ToString
    public override string ToString()
    {
        return "Battleship";
    }

    public override IShip Clone()
    {
        return new ShipBattleship(Positions, IsAlive);
    }

    // override object.Equals
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return ShipLength == ((ShipBattleship) obj).ShipLength
            && Positions == ((ShipBattleship) obj).Positions
            && IsAlive == ((ShipBattleship) obj).IsAlive;
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
