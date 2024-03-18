using Battleship.Utils;
namespace Battleship.Ammo;

public interface IAmmo
{
    public IEnumerable<Position> Shoot(Position origin);



    public string ToString();
    public bool Equals(object? obj);
    public int GetHashCode();
}
