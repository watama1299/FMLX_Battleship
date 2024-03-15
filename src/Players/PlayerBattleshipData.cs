using Battleship.Ammo;
using Battleship.GameBoard;
namespace Battleship;

public class PlayerBattleshipData
{
    public IBoard PlayerBoard {get; private set;}
    public Dictionary<IAmmo, int> Ammo {get; private set;} = new();



    public PlayerBattleshipData(IBoard playerBoard) {
            PlayerBoard = playerBoard;
            Ammo.Add(new MissileSingle(), PlayerBoard.GridPeg.TotalGrid);
    }
    public PlayerBattleshipData(IBoard playerBoard, IDictionary<IAmmo, int> additionalAmmo) : this(playerBoard) {
            foreach (var ammo in additionalAmmo) {
                if (ammo.Value < 0) {
                    throw new Exception($"Amount of ammo {ammo.Key} cannot be lesser than 0!");
                }
                Ammo.Add(ammo.Key, ammo.Value);
            }
    }



    public int GetAmmoCount(IAmmo ammoType) {
        if (!Ammo.ContainsKey(ammoType)) {
            return -1;
        }
        return Ammo[ammoType];
    }

    public bool GiveAmmo(IAmmo ammoType, int amount) {
        if (amount <= 0) return false;

        if (Ammo.ContainsKey(ammoType)) {
            Ammo[ammoType] += amount;
        }

        Ammo.Add(ammoType, amount);
        return true;
    }

    public bool RemoveAmmo(IAmmo ammoType, int amount) {
        if (amount <= 0) return false;
        
        if (!Ammo.ContainsKey(ammoType)) {
            return false;
        }
        if (Ammo[ammoType] == 0) {
            return false;
        }
        if (amount > Ammo[ammoType]) {
            return false;
        }

        Ammo[ammoType] -= amount;
        return true;
    }
}
