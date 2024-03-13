namespace Battleship.Ammo;

public class MissileSingle : IShoot
{
    public List<Position> Shoot(Position origin)
    {
        var output = new List<Position>(); 
        output.Add(origin);
        return output;
    }

}
