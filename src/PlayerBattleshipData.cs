namespace Battleship;

public class PlayerBattleshipData
{
    public IBoard PlayerBoard {get; private set;}
    public List<Ship> PlayerShips {get; private set;}
    public int ShipsSunk {get; private set;} = 0;
    public Dictionary<IShoot, int> Ammo {get; private set;}

    public PlayerBattleshipData(IBoard playerBoard, List<Ship> playerShips) {
        PlayerBoard = playerBoard;
        PlayerShips = playerShips;
        Ammo = new();
    }
}
