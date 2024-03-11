namespace Battleship;

public interface IPlayer {
    private static Random _idGenerator = new();
    public int Id {get;}
    public string Name {get;}
}
