using Battleship.Utils;
namespace Battleship.Ammo;

/// <summary>
/// Interface for ammo used in the battleship game
/// </summary>
public interface IAmmo
{
    /// <summary>
    /// Method used to shoot the type of shot
    /// </summary>
    /// <param name="origin">Target location</param>
    /// <returns>A list of positions hit by the ammo type</returns>
    public IEnumerable<Position> Shoot(Position origin);



    /// <summary>
    /// Override default <c>ToString</c> implementation
    /// </summary>
    /// <returns>String representation of the ammo type</returns>
    public string ToString();

    /// <summary>
    /// Override default <c>Equals</c> implementation
    /// </summary>
    /// <param name="obj"></param>
    /// <returns><c>true</c> if same, <c>false</c> if not</returns>
    public bool Equals(object? obj);

    /// <summary>
    /// Override default <c>GetHashCode</c> implementation
    /// </summary>
    /// <returns><c>int</c> representing the hashcode</returns>
    public int GetHashCode();
}
