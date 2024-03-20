namespace Battleship.Players;

/// <summary>
/// Concrete class which implements the <c>IPlayer</c> interface
/// </summary>
public class Player : IPlayer
{
    public int Id {get; private set;}
    public string Name {get; private set;}



    /// <summary>
    /// Constructor for a player object.
    /// The <c>Id</c> of each player is randomly generated upon creation of object.
    /// </summary>
    /// <param name="name">Username of the player</param>
    public Player(string name) {
        Id = new Random().Next();
        Name = name;
    }



    /// <summary>
    /// Overriding of the <c>object.Equals</c> method
    /// </summary>
    /// <param name="obj">Object which it is compared against</param>
    /// <returns>
    /// <c>true</c> if it is the same object
    /// <c>false</c> if it is not the same object
    /// </returns>
    public override bool Equals(object? obj) {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return Id == ((Player) obj).Id
            && Name == ((Player) obj).Name;
    }
    
    /// <summary>
    /// Overriding of the <c>object.GetHashCode</c> method
    /// </summary>
    /// <returns>Hashcode of the <c>Player</c> object</returns>
    public override int GetHashCode() {
        return HashCode.Combine(Id, Name);
    }
}
