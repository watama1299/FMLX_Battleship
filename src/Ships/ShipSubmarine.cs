using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipSubmarine : Ship, IShip
{
    public override int ShipLength {get; protected set;} = 3;

    public override IDictionary<Position, PegType> Positions {get; protected set;}

    public override bool IsAlive {get; protected set;} = true;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isAlive"></param>
    private ShipSubmarine(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }

    /// <summary>
    /// 
    /// </summary>
    public ShipSubmarine() {
        Positions = new Dictionary<Position, PegType>();
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
