namespace Battleship.Players;

/// <summary>
/// Interface which defines the properties that a player playing battleship must have
/// </summary>
public interface IPlayer
{
    /// <summary>
    /// Unique ID number of each player
    /// </summary>
    public int Id {get;}

    /// <summary>
    /// Username of each player
    /// </summary>
    public string Name {get;}
}
