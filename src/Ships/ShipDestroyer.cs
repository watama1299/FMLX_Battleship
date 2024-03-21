using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

/// <summary>
/// Concrete class representing a Destroyer ship piece in a normal game of Battleship
/// </summary>
public class ShipDestroyer : Ship, IShip
{
    public override int ShipLength {get; protected set;} = 2;

    public override IDictionary<Position, PegType> Positions {get; protected set;}

    public override bool IsAlive {get; protected set;} = true;



    /// <summary>
    /// Private constructor, only used during cloning process
    /// </summary>
    /// <param name="pos">List of positions</param>
    /// <param name="isAlive">Alive(true) / Dead(false)</param>
    private ShipDestroyer(IDictionary<Position, PegType> pos, bool isAlive) {
        Positions = (Dictionary<Position, PegType>) pos;
        IsAlive = isAlive;
    }

    /// <summary>
    /// Constructor for the ship type Destroyer
    /// </summary>
    public ShipDestroyer() {
        Positions = new Dictionary<Position, PegType>();
    }



    // override object.ToString
    public override string ToString()
    {
        return "Destroyer";
    }

    public override IShip Clone()
    {
        return new ShipDestroyer(Positions, IsAlive);
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
