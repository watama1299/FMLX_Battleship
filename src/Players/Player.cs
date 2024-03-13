namespace Battleship.Players;

public class Player : IPlayer
{
    private static Random _idGenerator = new();
    public int Id {get; private set;}
    public string Name {get; private set;}

    public Player(string name) {
        Id = _idGenerator.Next();
        Name = name;
    }

    // override object.Equals
    public override bool Equals(object? obj)
    {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return Id == ((Player) obj).Id
            && Name == ((Player) obj).Name;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return Id.GetHashCode() * Name.GetHashCode();
    }
}
