using System.Net;
using Battleship.Enums;
namespace Battleship;

public class GameController
{
    // private int _turn;
    private IBoard _templateBoard;
    private List<IShip> _templateShips;
    private Dictionary<IShoot, int> _templateAmmo;
    private List<IPlayer> _players = new(2);
    private Dictionary<IPlayer, PlayerBattleshipData> _playerData = new(2);
    private Queue<IPlayer> _activePlayer = new(2);
    public GameStatus Status {get; private set;}

    public GameController(
        IPlayer p1,
        IPlayer p2,
        IBoard board,
        List<IShip> ships,
        Dictionary<IShoot, int> ammoAndAmount
        ) {
            _players.Add(p1);
            _players.Add(p2);

            _templateBoard = board;
            _templateShips = ships;

            // make sure theres enough ammo provided by user
            int totalAmmo = 0;
            foreach (var amount in ammoAndAmount.Values) {
                totalAmmo += amount;
            }
            if (totalAmmo < board.GridPeg.TotalGrid) {
                throw new Exception($"Please provide enough ammo for the size of the grid!\nGrid size: {board.GridPeg.TotalGrid}");
            }
            _templateAmmo = ammoAndAmount;

            var dataP1 = new PlayerBattleshipData(_templateBoard, _templateAmmo);
            _playerData.Add(p1, dataP1);

            var dataP2 = new PlayerBattleshipData(_templateBoard, _templateAmmo);
            _playerData.Add(p2, dataP2);

            Status = GameStatus.INIT;
    }



    /**
        GAME STATE METHODS
    */
    public GameStatus StartGame() {
        if (Status != GameStatus.INIT) return Status;

        Random rng = new();
        if (rng.Next(10) < 5) {
            _activePlayer.Enqueue(_players[0]);
            _activePlayer.Enqueue(_players[1]);
        } else {
            _activePlayer.Enqueue(_players[1]);
            _activePlayer.Enqueue(_players[0]);
        }

        Status = GameStatus.ONGOING;
        return Status;
    }
    public GameStatus NextPlayer() {
        // check game status still ongoing
        if (Status != GameStatus.ONGOING) {
            throw new Exception("No ongoing game!");
        }

        // get next player
        var nextPlayer = _activePlayer.Dequeue();
        var playerShips = _playerData[nextPlayer].PlayerBoard.ShipsOnBoard;
        if (playerShips is null || playerShips.Count == 0) {
            throw new Exception("Player has no ships!");
        }

        // check if next player still has live ships
        if (playerShips.ContainsValue(true)) {
            _activePlayer.Enqueue(nextPlayer);
            return Status;
        }
        Status = GameStatus.ENDED;
        return Status;

    }
    public GameStatus ResetGame() {
        if (Status != GameStatus.ENDED) return Status;

        var tempData = new Dictionary<IPlayer, PlayerBattleshipData>(2);
        foreach (var player in _players) {
            tempData.Add(player, new PlayerBattleshipData(_templateBoard, _templateAmmo));
        }
        _playerData = tempData;
        Status = GameStatus.INIT;
        return Status;
    }
    public GameStatus ResetGame(IBoard newBoard, List<IShip> newShips) {
        if (Status != GameStatus.ENDED) return Status;

        _templateBoard = newBoard;
        _templateShips = newShips;
        return ResetGame();
    }



    /**
        PLAYER ACTIONS
    */
    public bool PlayerPlaceShip(
        IPlayer player,
        IShip ship,
        Position startCoord,
        ShipOrientation orientation
        ) {
            if (!_templateShips.Contains(ship)) return false;

            var playerBoard = _playerData[player].PlayerBoard;
            bool successfulPlacement = playerBoard.PutShipOnBoard(ship, startCoord, orientation);
            if (!successfulPlacement) return false;
            return true;
    }
    public bool PlayerShoot(
        IPlayer attacker,
        IPlayer target,
        Position position,
        IShoot shootMode
        ) {
            if (attacker.Equals(target)) throw new Exception("Player cannot chose themselves as a target!");

            // check ammo count
            var ammoLeft = _playerData[attacker].GetAmmoCount(shootMode);
            if (ammoLeft <= 0) return false;

            // check target board, then update
            var targetBoard = _playerData[target].PlayerBoard;
            var shipOnPosition = targetBoard.GetShipOnBoard(position);
            if (shipOnPosition?.Positions[position] != PegType.NONE) return false;
            var positionShot = targetBoard.IncomingAttack(position, shootMode);
            
            // update attacker board and data
            var attackerBoard = _playerData[attacker].PlayerBoard;
            attackerBoard.PutPegOnBoard(positionShot);
            _playerData[attacker].RemoveAmmo(shootMode, 1);

            return true;
    }



    /**
        UTILITY METHODS TO GET GAME INFO
    */
    public IBoard GetPlayerBoard(IPlayer player) {
        if (!_playerData.ContainsKey(player)) throw new Exception("No such player!");

        return _playerData[player].PlayerBoard;
    }
    public IGrid<IShip> GetPlayerGridShip(IPlayer player) {
        var board = GetPlayerBoard(player);
        return board.GridShip;
    }
    public IGrid<PegType> GetPlayerGridPeg(IPlayer player) {
        var board = GetPlayerBoard(player);
        return board.GridPeg;
    }
    public Dictionary<IShip, bool> GetPlayerShipsAll(IPlayer player) {
        if (!_playerData.ContainsKey(player)) throw new Exception("No such player!");

        return _playerData[player].PlayerBoard.ShipsOnBoard;
    }
    public List<IShip> GetPlayerShipsLive(IPlayer player) {
        if (!_playerData.ContainsKey(player)) throw new Exception("No such player!");

        List<IShip> liveShips = new();
        var ships = _playerData[player].PlayerBoard.ShipsOnBoard;
        foreach (var ship in ships) {
            if (ship.Value) {
                liveShips.Add(ship.Key);
            }
        }
        return liveShips;
    }
    public List<IShip> GetPlayerShipsSunk(IPlayer player) {
        if (!_playerData.ContainsKey(player)) throw new Exception("No such player!");

        List<IShip> sunkShips = new();
        var ships = _playerData[player].PlayerBoard.ShipsOnBoard;
        foreach (var ship in ships) {
            if (!ship.Value) {
                sunkShips.Add(ship.Key);
            }
        }
        return sunkShips;
    }
    public Dictionary<IShoot, int> GetPlayerAmmoStock(IPlayer player) {
        if (!_playerData.ContainsKey(player)) throw new Exception("No such player!");

        return _playerData[player].Ammo;
    }
    public void GivePlayersAdditionalAmmo(IShoot ammoType, int amount) {
        foreach (var pd in _playerData.Values) {
            pd.GiveAmmo(ammoType, amount);
        }
    }
}
