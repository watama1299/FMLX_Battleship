using Battleship.Utils;
namespace Battleship.Ammo;

public interface IAmmo
{
    public List<Position> Shoot(Position origin);
    public string ToString();
}
