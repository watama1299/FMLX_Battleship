using Battleship.Utils;
namespace Battleship.Ammo;

public class MissileNuclear : IAmmo
{
    public IEnumerable<Position> Shoot(Position origin) {
        var output = new List<Position>();
        output.Add(origin);

        // Horizontal positions
        output.Add(new Position(origin.X - 2, origin.Y));
        output.Add(new Position(origin.X - 1, origin.Y));
        output.Add(new Position(origin.X + 1, origin.Y));
        output.Add(new Position(origin.X + 2, origin.Y));

        // Vertical positions
        output.Add(new Position(origin.X, origin.Y - 2));
        output.Add(new Position(origin.X, origin.Y - 1));
        output.Add(new Position(origin.X, origin.Y + 1));
        output.Add(new Position(origin.X, origin.Y + 2));

        // Diagonal positions
        output.Add(new Position(origin.X - 1, origin.Y + 1));
        output.Add(new Position(origin.X + 1, origin.Y + 1));
        output.Add(new Position(origin.X + 1, origin.Y - 1));
        output.Add(new Position(origin.X - 1, origin.Y - 1));

        return output;
    }



    // override object.ToString
    public override string ToString()
    {
        return "Nuclear Missile";
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
