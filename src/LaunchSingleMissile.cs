
namespace Battleship;

public class LaunchSingleMissile : IShoot
{
    public List<Position> Shoot(Position origin)
    {
        var output = new List<Position>(); 
        output.Add(origin);
        return output;
    }

}
