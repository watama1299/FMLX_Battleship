using Battleship.Ammo;
using Battleship.GameBoard;
namespace Battleship;

/// <summary>
/// Class which used to contain the player data during a game of Battleship
/// </summary>
public class PlayerBattleshipData
{
    /// <summary>
    /// Property which holds the player's board
    /// </summary>
    public IBoard PlayerBoard {get; private set;}

    /// <summary>
    /// Property which holds the player's ammo stock
    /// </summary>
    public Dictionary<IAmmo, int> Ammo {get; private set;} = new();



    /// <summary>
    /// Constructor for the player data class.
    /// Default ammo <c>MissileSingle</c> and its amount is
    /// automatically assigned based on the <c>TotalGrid</c>
    /// property of the <c>PlayerBoard</c> 
    /// </summary>
    /// <param name="playerBoard">Board for the player to use</param>
    public PlayerBattleshipData(IBoard playerBoard) {
            PlayerBoard = playerBoard;
            Ammo.Add(new MissileSingle(), PlayerBoard.GridPeg.TotalGrid);
    }

    /// <summary>
    /// Constructor overloading which gives the player
    /// additional types of ammo
    /// </summary>
    /// <param name="playerBoard">Board for the player to use</param>
    /// <param name="additionalAmmo">Additional ammo types for the player to use</param>
    /// <exception cref="Exception"></exception>
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
