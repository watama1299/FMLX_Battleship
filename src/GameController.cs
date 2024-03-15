using Battleship.Ammo;
using Battleship.GameBoard;
using Battleship.Players;
using Battleship.Ships;
using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship;

public class GameController
{
    private IBoard _templateBoard;
    private List<IShip> _templateShips;
    private Dictionary<IAmmo, int> _templateAmmo;
    private Dictionary<IPlayer, PlayerBattleshipData> _playersData = new(2);
    private Queue<IPlayer> _activePlayer = new(2);
    public GameStatus Status {get; private set;}



    public GameController(
        IPlayer p1,
        IPlayer p2,
        IBoard board,
        List<IShip> ships, //IEnumerable
        Dictionary<IAmmo, int>? additionalAmmoType = null //IDictionary
        ) {
            _templateBoard = board;
            var boardRows = board.GridShip.Items.GetLength(0);
            var boardCols = board.GridShip.Items.GetLength(1);
            var boardP1 = new Board(boardRows, boardCols);
            var boardP2 = new Board(boardRows, boardCols);

            _templateShips = ships;

            if (additionalAmmoType?.Count > 0) {
                _templateAmmo = additionalAmmoType;
            } else {
                _templateAmmo = new();
            }
            var ammoP1 = new Dictionary<IAmmo, int>();
            var ammoP2 = new Dictionary<IAmmo, int>();
            foreach (var ammo in _templateAmmo) {
                ammoP1.Add(ammo.Key, ammo.Value);
                ammoP2.Add(ammo.Key, ammo.Value);
            }

            try {
                var dataP1 = new PlayerBattleshipData(boardP1, ammoP1);
                _playersData.Add(p1, dataP1);

                var dataP2 = new PlayerBattleshipData(boardP2, ammoP2);
                _playersData.Add(p2, dataP2);
            } catch (Exception) {
                throw;
            }

            Status = GameStatus.INIT;
    }



    /**
        GAME STATE METHODS
    */
    public GameStatus StartGame() {
        if (Status != GameStatus.INIT) return Status;
        
        List<IPlayer> players = _playersData.Keys.ToList();
        foreach (var player in players) {
            var playerShips = GetPlayerShipsAll(player);
            if (playerShips is null || playerShips.Count == 0) {
                throw new Exception("Player has no ships!");
            }
            if (playerShips.Count < _templateShips.Count) {
                throw new Exception("Player havent't put down all their ships!");
            }
        }

        Random rng = new();
        if (rng.Next(10) < 5) {
            _activePlayer.Enqueue(players[0]);
            _activePlayer.Enqueue(players[1]);
        } else {
            _activePlayer.Enqueue(players[1]);
            _activePlayer.Enqueue(players[0]);
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
        var playerShips = GetPlayerShipsStatus(nextPlayer);

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

        var boardRows = _templateBoard.GridShip.Items.GetLength(0);
        var boardCols = _templateBoard.GridShip.Items.GetLength(1);

        var tempData = new Dictionary<IPlayer, PlayerBattleshipData>(2);
        foreach (var player in _playersData.Keys) {
            tempData.Add(player, new PlayerBattleshipData(new Board(boardRows, boardCols), _templateAmmo));
        }
        _playersData = tempData;
        _activePlayer = new(2);
        Status = GameStatus.INIT;
        return Status;
    }
    // change List to IEnumerable
    public GameStatus ResetGame(IBoard newBoard, List<IShip> newShips) {
        if (Status != GameStatus.ENDED) return Status;

        _templateBoard = newBoard;
        _templateShips = newShips; //newShips.ToList()
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
            bool found = false;
            foreach (var s in _templateShips) {
                if (ship.GetType() == s.GetType()) {
                    found = true;
                    break;
                }
            }
            if (!found) return false;

            var playerBoard = GetPlayerBoard(player);
            bool successfulPlacement = playerBoard.PutShipOnBoard(ship, startCoord, orientation);
            if (!successfulPlacement) return false;
            return true;
    }
    public bool PlayerShoot(
        IPlayer attacker,
        IPlayer target,
        Position position,
        IAmmo shootMode
        ) {
            if (attacker.Equals(target)) throw new Exception("Player cannot chose themselves as a target!");

            // check ammo count
            var ammoLeft = GetPlayerAmmoCount(attacker, shootMode);
            if (ammoLeft <= 0) return false;

            // check target board, then update
            var targetBoard = GetPlayerBoard(target);
            var grid = targetBoard.GridShip;
            if (!grid.ContainsPosition(position)) return false;

            var shipOnPosition = targetBoard.GetShipOnBoard(position);
            if (shipOnPosition is not null) {
                if (shipOnPosition.GetPegOnPosition(position) == PegType.MISS
                    || shipOnPosition.GetPegOnPosition(position) == PegType.HIT) return false;
            }
            var positionShot = targetBoard.IncomingAttack(position, shootMode);
            
            // update attacker board and data
            var attackerBoard = GetPlayerBoard(attacker);
            attackerBoard.PutPegOnBoard(positionShot);
            _playersData[attacker].RemoveAmmo(shootMode, 1);

            return true;
    }
    public void GivePlayersAdditionalAmmo(IAmmo ammoType, int amount) {
        foreach (var pd in _playersData.Values) {
            pd.GiveAmmo(ammoType, amount);
        }
    }



    /**
        UTILITY METHODS TO GET GAME INFO
    */
    public List<IPlayer> GetPlayers() {
        return _playersData.Keys.ToList();
    }
    public IPlayer GetCurrentActivePlayer() {
        return _activePlayer.Peek();
    }
    public IPlayer PreviewNextPlayer() {
        var currentPlayer = GetCurrentActivePlayer();
        foreach (var p in GetPlayers()) {
            if (!p.Equals(currentPlayer)) return p;
        }
        return null;
    }
    public IPlayer[] GetPlayerTurn() {
        return _activePlayer.ToArray();
    }
    public PlayerBattleshipData GetPlayerData(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("Player doesn't exist!");
        
        return _playersData[player];
    }
    public IBoard GetPlayerBoard(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        return _playersData[player].PlayerBoard;
    }
    public IGrid<IShip> GetPlayerGridShip(IPlayer player) {
        var board = GetPlayerBoard(player);
        return board.GridShip;
    }
    public IGrid<PegType> GetPlayerGridPeg(IPlayer player) {
        var board = GetPlayerBoard(player);
        return board.GridPeg;
    }
    public Dictionary<IShip, bool> GetPlayerShipsStatus(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        return _playersData[player].PlayerBoard.ShipsOnBoard;
    }
    public List<IShip> GetPlayerShipsAll(IPlayer player) {
        return GetPlayerShipsStatus(player).Keys.ToList();
    }
    public List<IShip> GetPlayerShipsLive(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        List<IShip> liveShips = new();
        var ships = _playersData[player].PlayerBoard.ShipsOnBoard;
        foreach (var ship in ships) {
            if (ship.Value) {
                liveShips.Add(ship.Key);
            }
        }
        return liveShips;
    }
    public List<IShip> GetPlayerShipsSunk(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        List<IShip> sunkShips = new();
        var ships = _playersData[player].PlayerBoard.ShipsOnBoard;
        foreach (var ship in ships) {
            if (!ship.Value) {
                sunkShips.Add(ship.Key);
            }
        }
        return sunkShips;
    }
    public Dictionary<IAmmo, int> GetPlayerAmmoStock(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        return _playersData[player].Ammo;
    }
    public int GetPlayerAmmoCount(IPlayer player, IAmmo ammoType) {
        var ammo = GetPlayerAmmoStock(player);
        return ammo[ammoType];
    }
}
