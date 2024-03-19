using Battleship.Ammo;
using Battleship.GameBoard;
using Battleship.Players;
using Battleship.Ships;
using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship;

public class GameController
{
    /// <summary>
    /// Private variable used to store the template board.
    /// Used when resetting the game.
    /// </summary>
    private IBoard _templateBoard;

    /// <summary>
    /// Private variable used to store the template ships.
    /// Used when resetting the game.
    /// </summary>
    private List<IShip> _templateShips;

    /// <summary>
    /// Private variable used to store the template ammo.
    /// Used when resetting the game.
    /// </summary>
    private Dictionary<IAmmo, int> _templateAmmo;

    /// <summary>
    /// Private dictionary used to store players and their data.
    /// </summary>
    private Dictionary<IPlayer, PlayerBattleshipData> _playersData = new(2);

    /// <summary>
    /// Private queue used to keep track of the current active player.
    /// </summary>
    private Queue<IPlayer> _activePlayer = new(2);

    /// <summary>
    /// Public variable used to keep track of the status of the current game.
    /// </summary>
    public GameStatus Status {get; private set;}



    /// <summary>
    /// Constructor for game controller. Used to setup the game
    /// </summary>
    /// <param name="p1">Player 1</param>
    /// <param name="p2">Player 2</param>
    /// <param name="board">Board that will be used by both players</param>
    /// <param name="ships">Ships that will be used by both players</param>
    /// <param name="additionalAmmoType">Additional types of ammo added to the game.
    ///                                  If null, game will only use default ammo based on size of board.
    /// </param>
    public GameController(
        IPlayer p1,
        IPlayer p2,
        IBoard board,
        IEnumerable<IShip> ships,
        IDictionary<IAmmo, int>? additionalAmmoType = null
        ) {
            _templateBoard = board;
            var boardRows = board.GridShip.Items.GetLength(0);
            var boardCols = board.GridShip.Items.GetLength(1);
            var boardP1 = new Board(boardRows, boardCols);
            var boardP2 = new Board(boardRows, boardCols);

            _templateShips = new(ships);

            if (additionalAmmoType?.Count > 0) {
                _templateAmmo = new(additionalAmmoType);
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



    /// <summary>
    /// Method used to start the game. Also randomises the order of the player.
    /// </summary>
    /// <returns><c>GameStatus.INIT</c></returns>
    /// <exception cref="Exception">
    /// Thrown exception if player has no ships or has not put all their ships on their boards.
    /// </exception>
    public GameStatus StartGame() { // out string errormsg
        if (Status != GameStatus.INIT) return Status;
        
        List<IPlayer> players = _playersData.Keys.ToList();
        foreach (var player in players) {
            var playerShips = new List<IShip>(GetPlayerShipsAll(player));

            // change errormsg to string not exception
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

    /// <summary>
    /// Method to move the turn to the next player or end the game.
    /// </summary>
    /// <returns>
    /// <c>GameStatus.ONGOING</c> if the game is still on going.
    /// <c>GameStatus.ENDED</c> if the game is now ending.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if there is currently no ongoing game.
    /// </exception>
    public GameStatus NextPlayer() {
        // check game status still ongoing
        if (Status != GameStatus.ONGOING) {
            throw new Exception("No ongoing game!");
        }

        // get next player
        var nextPlayer = _activePlayer.Dequeue();
        var playerShips = new Dictionary<IShip, bool>(GetPlayerShipsStatus(nextPlayer));

        // check if next player still has live ships
        if (playerShips.ContainsValue(true)) {
            _activePlayer.Enqueue(nextPlayer);
            return Status;
        }
        Status = GameStatus.ENDED;
        return Status;

    }

    /// <summary>
    /// Method to reset the game once the game has ended.
    /// </summary>
    /// <returns>
    /// <c>GameStatus.INIT</c> once the game has been reset.
    /// </returns>
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

    /// <summary>
    /// Method to reset the game using new boards, new ships and/or new ammo types.
    /// </summary>
    /// <param name="newBoard">New template board</param>
    /// <param name="newShips">New template ships</param>
    /// <param name="newAdditionalAmmo">New additional ammo</param>
    /// <returns></returns>
    public GameStatus ResetGame(IBoard? newBoard = null, IEnumerable<IShip>? newShips = null, IDictionary<IAmmo, int>? newAdditionalAmmo = null) {
        if (Status != GameStatus.ENDED) return Status;

        if (newBoard is not null) 
            _templateBoard = newBoard;

        if (newShips is not null) 
            _templateShips = newShips.ToList(); 

        if (newAdditionalAmmo is not null) 
            _templateAmmo = newAdditionalAmmo.ToDictionary(); 
        return ResetGame();
    }



    /// <summary>
    /// Method to help player place ship at a location in the board
    /// </summary>
    /// <param name="player">Player who is placing the ship</param>
    /// <param name="ship">The type of ship that the player is placing</param>
    /// <param name="startCoord">The coordinate of where the player wants to place the ship</param>
    /// <param name="orientation">The orientation of the ship to be placed</param>
    /// <returns></returns>
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

    /// <summary>
    /// Method for shooting a missile to the a target board
    /// </summary>
    /// <param name="attacker">The person currently attacking</param>
    /// <param name="target">The person receiving the attack</param>
    /// <param name="position">The coordinates of the attack</param>
    /// <param name="shootMode">The type of missile used for the attack</param>
    /// <returns>
    /// <c>true</c> if the position is successfully attacked,
    /// <c>false</c> if the attack failed
    /// </returns>
    public bool PlayerShoot(
        IPlayer attacker,
        IPlayer target,
        Position position,
        IAmmo shootMode
        ) {
            // check ammo count
            var ammoLeft = GetPlayerAmmoCount(attacker, shootMode);
            if (ammoLeft <= 0) return false;

            // check target board, then update
            var targetBoard = GetPlayerBoard(target);
            var grid = targetBoard.GridShip;
            if (!grid.ContainsPosition(position)) return false;

            // 
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

    /// <summary>
    /// Method to give players additional ammo
    /// </summary>
    /// <param name="ammoType">The type of ammo</param>
    /// <param name="amount">The amount of additional ammo given</param>
    public void GivePlayersAdditionalAmmo(IAmmo ammoType, int amount) {
        foreach (var pd in _playersData.Values) {
            pd.GiveAmmo(ammoType, amount);
        }
    }



    /// <summary>
    /// Utility method to get all the players
    /// </summary>
    /// <returns>The list of players</returns>
    public IEnumerable<IPlayer> GetPlayers() {
        return _playersData.Keys.ToList();
    }

    /// <summary>
    /// Utility method to get the current active player
    /// </summary>
    /// <returns>Current active player</returns>
    public IPlayer GetCurrentActivePlayer() {
        return _activePlayer.Peek();
    }
    
    /// <summary>
    /// Utility method to get the next player in the queue
    /// </summary>
    /// <returns>Next player to play</returns>
    public IPlayer? PreviewNextPlayer() {
        var currentPlayer = GetCurrentActivePlayer();
        foreach (var p in GetPlayers()) {
            if (!p.Equals(currentPlayer)) return p;
        }
        return null;
    }
    
    /// <summary>
    /// Utility method to get the players and their order of turn
    /// </summary>
    /// <returns><c>IPlayer[]</c> Player in array of their turn order</returns>
    public IPlayer[] GetPlayerTurn() {
        return _activePlayer.ToArray();
    }

    /// <summary>
    /// Utility method to get the current game data of a player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <returns><c>PlayerBattleshipData</c> The player's data</returns>
    /// <exception cref="Exception">
    /// Thrown if the player does not exist in the game
    /// </exception>
    public PlayerBattleshipData GetPlayerData(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("Player doesn't exist!");
        
        return _playersData[player];
    }

    /// <summary>
    /// Utility method to get the current board data of a player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <returns><c>IBoard</c> The player's board</returns>
    /// <exception cref="Exception">
    /// Thrown if the player does not exist in the game
    /// </exception>
    public IBoard GetPlayerBoard(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        return _playersData[player].PlayerBoard;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public IGrid<IShip> GetPlayerGridShip(IPlayer player) {
        var board = GetPlayerBoard(player);
        return board.GridShip;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public IGrid<PegType> GetPlayerGridPeg(IPlayer player) {
        var board = GetPlayerBoard(player);
        return board.GridPeg;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IDictionary<IShip, bool> GetPlayerShipsStatus(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        return _playersData[player].PlayerBoard.ShipsOnBoard;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public IEnumerable<IShip> GetPlayerShipsAll(IPlayer player) {
        return GetPlayerShipsStatus(player).Keys.ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<IShip> GetPlayerShipsLive(IPlayer player) {
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<IShip> GetPlayerShipsSunk(IPlayer player) {
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IDictionary<IAmmo, int> GetPlayerAmmoStock(IPlayer player) {
        if (!_playersData.ContainsKey(player)) throw new Exception("No such player!");

        return _playersData[player].Ammo;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ammoType"></param>
    /// <returns></returns>
    public int GetPlayerAmmoCount(IPlayer player, IAmmo ammoType) {
        var ammo = GetPlayerAmmoStock(player);
        return ammo[ammoType];
    }
}
