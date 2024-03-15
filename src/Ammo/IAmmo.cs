using Battleship.Utils;
namespace Battleship.Ammo;

public interface IAmmo
{
    // IEnumerable instead of List<>
    public List<Position> Shoot(Position origin);
    public string ToString();
}
