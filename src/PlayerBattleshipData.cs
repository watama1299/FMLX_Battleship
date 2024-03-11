namespace Battleship;

public class PlayerBattleshipData
{
    public IBoard PlayerBoard {get; private set;}
    public List<IShip> PlayerShips {get; private set;} = new();
    public int ShipsSunk {get; private set;} = 0;
    public Dictionary<IShoot, int> Ammo {get; private set;} = new();

    public PlayerBattleshipData(IBoard playerBoard, List<IShip> playerShips) {
        PlayerBoard = playerBoard;
        PlayerShips = playerShips;
        Ammo = new();
    }

    public bool AddShipSunk() {
        if (ShipsSunk >= PlayerShips.Capacity) {
            return false;
        }
        ShipsSunk += 1;
        return true;
    }

    public bool GiveAmmo(IShoot ammoType, int amount) {
        if (Ammo.ContainsKey(ammoType)) {
            Ammo[ammoType] += amount;
        }
        Ammo.Add(ammoType, amount);
        return true;
    }

    public bool RemoveAmmo(IShoot ammoType, int amount) {
        if (!Ammo.ContainsKey(ammoType)) {
            return false;
        }
        if (Ammo[ammoType] == 0) {
            return false;
        }
        Ammo[ammoType] -= amount;
        return true;
    }
}
