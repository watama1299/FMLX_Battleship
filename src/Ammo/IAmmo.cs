using Battleship.Utils;
namespace Battleship.Ammo;

public interface IAmmo
{
    // IEnumerable instead of List<>
    public IEnumerable<Position> Shoot(Position origin);
    public string ToString();
}
