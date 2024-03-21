using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

/// <summary>
/// Concrete class representing a blank spot on the grid for ships in a normal game of Battleship
/// </summary>
public class ShipBlank : Ship, IShip
{
    public override int ShipLength {get; protected set;} = 1;

    public override IDictionary<Position, PegType> Positions {get; protected set;}

    public override bool IsAlive {get; protected set;} = true;



    /// <summary>
    /// Private constructor, only used during cloning process
    /// </summary>
    /// <param name="pos">List of positions</param>
    /// <param name="isAlive">Alive(true) / Dead(false)</param>
    private ShipBlank(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }

    /// <summary>
    /// Constructor for the ship type representing a blank spot in the grid ship
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
