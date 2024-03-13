namespace Battleship.Ammo;

public class MissileSingle : IAmmo
{
    public List<Position> Shoot(Position origin)
    {
        return new List<Position> {origin}; 
    }

    // override object.Equals
    public override bool Equals(object? obj)
    {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return true;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return this.GetType().GetHashCode();
    }
}
