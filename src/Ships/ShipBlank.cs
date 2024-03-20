using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public class ShipBlank : Ship, IShip
{
    public override int ShipLength {get; protected set;} = 1;

    public override IDictionary<Position, PegType> Positions {get; protected set;}

    public override bool IsAlive {get; protected set;} = true;



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="isAlive"></param>
    private ShipBlank(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }

    /// <summary>
    /// 
    /// </summary>
    public ShipBlank() {
        Positions = new Dictionary<Position, PegType>();
    }



    // override object.ToString
    public override string ToString()
    {
        return "Blank";
    }

    public override IShip Clone()
    {
        return new ShipBlank(Positions, IsAlive);
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
