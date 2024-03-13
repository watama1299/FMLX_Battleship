namespace Battleship.Ammo;

public class MissileBarrage : IShoot
{
    public List<Position> Shoot(Position origin)
    {
        var output = new List<Position>();
        output.Add(origin);

        // Horizontal positions
        output.Add(new Position(origin.X - 1, origin.Y));
        output.Add(new Position(origin.X + 1, origin.Y));

        // Vertical positions
        output.Add(new Position(origin.X, origin.Y - 1));
        output.Add(new Position(origin.X, origin.Y + 1));
        
        return output;
    }
}
