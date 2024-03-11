namespace Battleship;

public class Player : IPlayer
{
    private static Random _idGenerator = new();
    public int Id {get; private set;}
    public string Name {get; private set;}

    public Player(string name) {
        Id = _idGenerator.Next();
        Name = name;
    }
}
