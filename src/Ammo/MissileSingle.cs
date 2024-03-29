using Battleship.Utils;
namespace Battleship.Ammo;

/// <summary>
/// Concrete class for the default single target ammo of battleship game
/// </summary>
public class MissileSingle : IAmmo
{
    public IEnumerable<Position> Shoot(Position origin) {
        return new List<Position> {origin}; 
    }



    // override object.ToString
    public override string ToString()
    {
        return "Single Missile";
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
