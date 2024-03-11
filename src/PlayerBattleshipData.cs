namespace Battleship;

public class PlayerBattleshipData
{
    public IBoard PlayerBoard {get; private set;}
    public int ShipsSunk {get; private set;} = 0;
    public Dictionary<IShoot, int> Ammo {get; private set;} = new();



    public PlayerBattleshipData(
        IBoard playerBoard,
        Dictionary<IShoot, int> ammoAndAmount
        ) {
            PlayerBoard = playerBoard;
            foreach (var ammo in ammoAndAmount) {
                GiveAmmo(ammo.Key, ammo.Value);
            }
    }



    public int GetAmmoCount(IShoot ammoType) {
        if (!Ammo.ContainsKey(ammoType)) {
            return -1;
        }
        return Ammo[ammoType];
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
