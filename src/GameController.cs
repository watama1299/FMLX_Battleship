using Battleship.Ammo;
using Battleship.GameBoard;
using Battleship.Players;
using Battleship.Ships;
using Battleship.Utils;
using Battleship.Utils.Enums;
using Microsoft.Extensions.Logging;
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


    private ILogger<GameController>? _log;



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
    /// <param name="logger">For optional logging purposes</param>
    public GameController(
        IPlayer p1,
        IPlayer p2,
        IBoard board,
        IEnumerable<IShip> ships,
        IDictionary<IAmmo, int>? additionalAmmoType = null,
        ILogger<GameController>? logger = null
        ) {
            _log = logger;
            _log?.LogInformation(@"{logger} has been assigned to log GameController {gc}",
                                 logger,
                                 this);

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
            _log?.LogInformation(@"GameController has been created with players {p1} and {p2}, board {board},
                                ships {ships}, additional ammo {ammo} and logger {logger}",
                                p1, p2, board, ships, additionalAmmoType, logger);
        }



    /// <summary>
    /// Method used to start the game. Also randomises the order of the player.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <returns><c>GameStatus.INIT</c></returns>
    public GameStatus StartGame(out string errorMessage) {
        errorMessage = string.Empty;
        if (Status != GameStatus.INIT) {
            errorMessage = "Game is not in an initialised state. Please initialise game first!";
            _log?.LogWarning("Tried to start a game when it hasn't been initialised. Current game status: {status}", Status);
            return Status;
        }
        
        List<IPlayer> players = _playersData.Keys.ToList();
        foreach (var player in players) {
            var tempShips = GetPlayerShipsAll(player, out errorMessage);
            if (errorMessage == "No such player!") {
                return Status;
            }
            var playerShips = new List<IShip>(tempShips);

            // change errormsg to string not exception
            if (playerShips is null || playerShips.Count == 0) {
                errorMessage = "Player has no ships!";
                _log?.LogWarning("Tried to start game but player {player} has no ships.", player);
                return Status;
            }
            if (playerShips?.Count < _templateShips.Count) {
                errorMessage = "Player havent't put down all their ships!";
                _log?.LogWarning("Tried to start game but player {player} has not put down all their ships.", player);
                return Status;
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
        _log?.LogInformation("Game has started. Current game status: {status}", Status);
        return Status;
    }

    /// <summary>
    /// Method to move the turn to the next player or end the game.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <returns>
    /// <c>GameStatus.ONGOING</c> if the game is still on going.
    /// <c>GameStatus.ENDED</c> if the game is now ending.
    /// </returns>
    public GameStatus NextPlayer(out string errorMessage) {
        errorMessage = string.Empty;
        // check game status still ongoing
        if (Status != GameStatus.ONGOING) {
            _log?.LogWarning("Tried to go to next player but no ongoing game. Current game status: {status}", Status);
            errorMessage = "No ongoing game!";
            return Status;
        }

        // get next player
        var nextPlayer = _activePlayer.Dequeue();
        var playerShips = new Dictionary<IShip, bool>(GetPlayerShipsStatus(nextPlayer, out errorMessage));
        if (errorMessage == "No such player!") {
            _log?.LogWarning("Tried to go to next player but next player doesn't exist in the queue. Current game status: {status}", Status);
            return Status;
        }

        // check if next player still has live ships
        if (playerShips.ContainsValue(true)) {
            _activePlayer.Enqueue(nextPlayer);
            _log?.LogInformation("Successfully going to the next player. Current game status: {status}", Status);
            return Status;
        }
        Status = GameStatus.ENDED;
        _log?.LogInformation("Next player has no remaining ships left. Current game status: {status}", Status);
        return Status;

    }

    /// <summary>
    /// Method to reset the game once the game has ended.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <returns>
    /// <c>GameStatus.INIT</c> once the game has been reset.
    /// </returns>
    public GameStatus ResetGame(out string errorMessage) {
        errorMessage = string.Empty;
        if (Status != GameStatus.ENDED) {
            _log?.LogWarning("Tried to go to reset game but game has not ended yet. Current game status: {status}", Status);
            errorMessage = "Game has not ended yet! It cannot be reset without first ending the game!";
            return Status;
        }

        var boardRows = _templateBoard.GridShip.Items.GetLength(0);
        var boardCols = _templateBoard.GridShip.Items.GetLength(1);

        var tempData = new Dictionary<IPlayer, PlayerBattleshipData>(2);
        foreach (var player in _playersData.Keys) {
            tempData.Add(player, new PlayerBattleshipData(new Board(boardRows, boardCols), _templateAmmo));
        }
        _playersData = tempData;
        _activePlayer = new(2);

        Status = GameStatus.INIT;
        _log?.LogInformation("Game has been reset. Current game status: {status}", Status);
        return Status;
    }

    /// <summary>
    /// Method to reset the game using new boards, new ships and/or new ammo types.
    /// </summary>
    /// <param name="errorMessage">Error message</param>
    /// <param name="newBoard">New template board</param>
    /// <param name="newShips">New template ships</param>
    /// <param name="newAdditionalAmmo">New additional ammo</param>
    /// <returns></returns>
    public GameStatus ResetGame(out string errorMessage,
                                IBoard? newBoard = null,
                                IEnumerable<IShip>? newShips = null,
                                IDictionary<IAmmo, int>? newAdditionalAmmo = null) {
        errorMessage = string.Empty;
        if (Status != GameStatus.ENDED) {
            _log?.LogWarning("Tried to go to reset game but game has not ended yet. Current game status: {status}", Status);
            errorMessage = "Game has not ended yet! It cannot be reset without first ending the game!";
            return Status;
        }

        if (newBoard is not null) 
            _templateBoard = newBoard;

        if (newShips is not null) 
            _templateShips = newShips.ToList(); 

        if (newAdditionalAmmo is not null) 
            _templateAmmo = newAdditionalAmmo.ToDictionary(); 
        return ResetGame(out errorMessage);
    }



    /// <summary>
    /// Method to help player place ship at a location in the board
    /// </summary>
    /// <param name="player">Player who is placing the ship</param>
    /// <param name="ship">The type of ship that the player is placing</param>
    /// <param name="startCoord">The coordinate of where the player wants to place the ship</param>
    /// <param name="orientation">The orientation of the ship to be placed</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns></returns>
    public bool PlayerPlaceShip(
        IPlayer player,
        IShip ship,
        Position startCoord,
        ShipOrientation orientation,
        out string errorMessage
        ) {
            bool found = false;
            foreach (var s in _templateShips) {
                if (ship.GetType() == s.GetType()) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                _log?.LogWarning(@"Ship {ship} does not exist in the template ships {ships}.
                                 Cancelling placement...",
                                 ship,
                                 _templateShips);
                errorMessage = "This ship is not part of the list of ships that has been set previously!";
                return false;
            }

            var playerBoard = GetPlayerBoard(player, out errorMessage);
            if (errorMessage == "No such player!") {
                _log?.LogWarning(@"Player {player} was not found. Cancelling placement...",
                                 player);
                return false;
            }
            
            bool successfulPlacement = playerBoard.PutShipOnBoard(ship, startCoord, orientation);
            if (!successfulPlacement) {
                _log?.LogWarning(@"Ship {ship} couldn't be put at position {position} with orientation {orientation}.
                                 Cancelling placement...",
                                 ship,
                                 startCoord,
                                 orientation);
                errorMessage = $"This {ship} couldn't be placed at {startCoord} with this {orientation}!";
                return false;
            }

            errorMessage = string.Empty;
            _log?.LogInformation(@"Ship {ship} was successfully put at position {position} with orientation {orientation}.",
                             ship,
                             startCoord,
                             orientation);
            return true;
    }

    /// <summary>
    /// Method for shooting a missile to the a target board
    /// </summary>
    /// <param name="attacker">The person currently attacking</param>
    /// <param name="target">The person receiving the attack</param>
    /// <param name="position">The coordinates of the attack</param>
    /// <param name="shootMode">The type of missile used for the attack</param>
    /// <param name="errorMessage">Error message if either player is not found</param>
    /// <returns>
    /// <c>true</c> if the position is successfully attacked,
    /// <c>false</c> if the attack failed
    /// </returns>
    public bool PlayerShoot(
        IPlayer attacker,
        IPlayer target,
        Position position,
        IAmmo shootMode,
        out string errorMessage
        ) {
            errorMessage = string.Empty;

            // check existence of both attacker and target
            if (!_playersData.ContainsKey(attacker)) {
                _log?.LogWarning(@"Player attacking {attacker} does not exist in players list {list}.
                                 Cancelling attack...",
                                 attacker,
                                 _playersData.Keys);
                errorMessage = "Attacker doesn't exist in players list!";
                return false;
            }
            if (!_playersData.ContainsKey(target)) {
                _log?.LogWarning(@"Player targetted {target} does not exist in players list {list}.
                                 Cancelling attack...",
                                 attacker,
                                 _playersData.Keys);
                errorMessage = "Target player doesn't exist in players list!";
                return false;
            }

            // check ammo count
            var ammoLeft = GetPlayerAmmoCount(attacker, shootMode, out errorMessage);
            if (ammoLeft <= 0) {
                _log?.LogWarning(@"Player {attacker} does not have enough {ammo}. Current amount: {amount}.
                                 Cancelling attack...",
                                 attacker,
                                 shootMode,
                                 ammoLeft);
                return false;
            }

            // check target board, then update
            var targetBoard = GetPlayerBoard(target, out errorMessage);
            if (errorMessage == "No such player!") {
                _log?.LogWarning(@"Player {target} does not exist. Cancelling attack...",
                                 target);
                return false;
            }

            var grid = targetBoard.GridShip;
            if (!grid.ContainsPosition(position)) {
                _log?.LogWarning(@"Targeted position {position} is out of the target grid {grid}.
                                 Cancelling attack...",
                                 position,
                                 grid);
                return false;
            }

            // 
            var shipOnPosition = targetBoard?.GetShipOnBoard(position);
            if (shipOnPosition is not null) {
                if (shipOnPosition.GetPegOnPosition(position) == PegType.MISS
                    || shipOnPosition.GetPegOnPosition(position) == PegType.HIT) {
                        _log?.LogInformation(@"Position {position} has been attacked before.
                                             Cancelling attack...",
                                             position);
                        return false;
                    }
            }
            var positionShot = targetBoard?.IncomingAttack(position, shootMode);
            
            // update attacker board and data
            var attackerBoard = GetPlayerBoard(attacker, out errorMessage);
            if (errorMessage == "No such player!") {
                _log?.LogWarning(@"Player attacking {attacker} does not exist in players list {list}.
                                 Cancelling attack...",
                                 attacker,
                                 _playersData.Keys);
                errorMessage = "Attacker doesn't exist in players list!";
                return false;
            }

            attackerBoard?.PutPegOnBoard(positionShot);
            _playersData[attacker].RemoveAmmo(shootMode, 1);

            _log?.LogInformation(@"Player {attacker} successfully attacked player {target}
                                 at position {position} using {ammo}",
                                 attacker,
                                 target,
                                 position,
                                 shootMode);
            return true;
    }

    /// <summary>
    /// Method to give both players additional ammo
    /// </summary>
    /// <param name="ammoType">The type of ammo</param>
    /// <param name="amount">The amount of additional ammo given</param>
    public void GivePlayersAdditionalAmmo(IAmmo ammoType, int amount) {
        foreach (var pd in _playersData) {
            _log?.LogInformation(@"Giving player {player} extra ammo: {amount}x {ammo}",
                                 pd.Key,
                                 amount,
                                 ammoType);
            pd.Value.GiveAmmo(ammoType, amount);
        }
    }



    /// <summary>
    /// Utility method to get all the players
    /// </summary>
    /// <returns>The list of players</returns>
    public IEnumerable<IPlayer> GetPlayers() {
        var tempList = _playersData.Keys.ToList();
        _log?.LogInformation(@"Getting all players in the game, {p1} and {p2}",
                             tempList.First(),
                             tempList.Last());
        return tempList;
    }

    /// <summary>
    /// Utility method to get the current active player
    /// </summary>
    /// <returns>Current active player</returns>
    public IPlayer GetCurrentActivePlayer() {
        var tempPlayer = _activePlayer.Peek();
        _log?.LogInformation(@"Getting current active player. Current active player: {player}", tempPlayer);
        return tempPlayer;
    }
    
    /// <summary>
    /// Utility method to get the next player in the queue
    /// </summary>
    /// <returns>Next player to play</returns>
    public IPlayer? PreviewNextPlayer() {
        var currentPlayer = GetCurrentActivePlayer();
        foreach (var p in GetPlayers()) {
            if (!p.Equals(currentPlayer)) {
                _log?.LogInformation(@"Getting next player. Next player: {player}", p);
                return p;
            }
        }
        _log?.LogWarning(@"Next player doesn't exist.");
        return null;
    }
    
    /// <summary>
    /// Utility method to get the players and their order of turn
    /// </summary>
    /// <returns><c>IPlayer[]</c> Player in array of their turn order</returns>
    public IPlayer[] GetPlayerTurn() {
        var tempArray = _activePlayer.ToArray();
        _log?.LogInformation(@"Current player: {p1}. Next player: {p2}",
                             tempArray[0],
                             tempArray[1]); 
        return tempArray;
    }

    /// <summary>
    /// Utility method to get the current game data of a player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns><c>PlayerBattleshipData</c> The player's data</returns>
    public PlayerBattleshipData? GetPlayerData(IPlayer player, out string errorMessage) {
        if (!_playersData.ContainsKey(player)) {
            errorMessage = "No such player!";
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null player data...", player);
            return null;
        }
        
        errorMessage = string.Empty;
        var tempData = _playersData[player];
        _log?.LogInformation(@"Player {player} successfully found. Player's game data: {data}",
                             player,
                             tempData);
        return tempData;
    }

    /// <summary>
    /// Utility method to get the current board data of a player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns><c>IBoard</c> The player's board</returns>
    public IBoard? GetPlayerBoard(IPlayer player, out string errorMessage) {
        if (!_playersData.ContainsKey(player)) {
            errorMessage = "No such player!";
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null board...", player);
            return null;
        }

        errorMessage = string.Empty;
        var tempBoard = _playersData[player].PlayerBoard;
        _log?.LogInformation(@"Player {player} successfully found. Player's board data: {board}",
                             player,
                             tempBoard);
        return tempBoard;
    }

    /// <summary>
    /// Utility method to get a specific player's ship grid
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A grid of ships</returns>
    public IGrid<IShip>? GetPlayerGridShip(IPlayer player, out string errorMessage) {
        var board = GetPlayerBoard(player, out errorMessage);
        if (board is null) {
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null grid ship...", player);
            return null;
        }

        var tempGridShip = board?.GridShip;
        _log?.LogInformation(@"Player {player} successfully found. Player's grid ship data: {gridShip}",
                             player,
                             tempGridShip);
        return tempGridShip;
    }
    
    /// <summary>
    /// Utility method to get a specifc player's peg grid
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A grid of pegs</returns>
    public IGrid<PegType>? GetPlayerGridPeg(IPlayer player, out string errorMessage) {
        var board = GetPlayerBoard(player, out errorMessage);
        if (board is null) {
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null grid peg...", player);
            return null;
        }

        var tempGridPeg = board?.GridPeg;
        _log?.LogInformation(@"Player {player} successfully found. Player's grid peg data: {gridPeg}",
                             player,
                             tempGridPeg);
        return tempGridPeg;
    }

    /// <summary>
    /// Utility method to get all of the ships and its status of a specific player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A dictionary with key ship and value boolean indicating alive/dead</returns>
    public IDictionary<IShip, bool>? GetPlayerShipsStatus(IPlayer player, out string errorMessage) {
        if (!_playersData.ContainsKey(player)) {
            errorMessage = "No such player!";
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null dictionary of ship stasus...", player);
            return null;
        }

        errorMessage = string.Empty;
        var shipsAndStatus = _playersData[player].PlayerBoard.ShipsOnBoard;
        _log?.LogInformation(@"Player {player} successfully found. Player's ship data: {ships}",
                             player,
                             shipsAndStatus);
        return shipsAndStatus;
    }

    /// <summary>
    /// Utility method to get all of the ships of a specific player without its status
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A list of ships owned by the player</returns>
    public IEnumerable<IShip>? GetPlayerShipsAll(IPlayer player, out string errorMessage) {
        var output = GetPlayerShipsStatus(player, out errorMessage);
        if (errorMessage == "No such player!") {
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null list of ships...", player);
            return null;
        }

        var ships = output?.Keys.ToList();
        _log?.LogInformation(@"Player {player} successfully found. Player's ships: {ships}",
                             player,
                             ships);
        return ships;
    }

    /// <summary>
    /// Utility method to get all the live ships of a specific player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A list of live ships owned by the player</returns>
    public IEnumerable<IShip>? GetPlayerShipsLive(IPlayer player, out string errorMessage) {
        if (!_playersData.ContainsKey(player)) {
            errorMessage = "No such player!";
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null list of live ship...", player);
            return null;
        }

        List<IShip> liveShips = new();
        var ships = _playersData[player].PlayerBoard.ShipsOnBoard;
        foreach (var ship in ships) {
            if (ship.Value) {
                liveShips.Add(ship.Key);
            }
        }

        errorMessage = string.Empty;
        _log?.LogInformation(@"Player {player} successfully found. Player's live ships: {ships}",
                             player,
                             liveShips);
        return liveShips;
    }

    /// <summary>
    /// Utility method to get all the dead ships of a specific player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A list of dead ships owned by the player</returns>
    public IEnumerable<IShip>? GetPlayerShipsSunk(IPlayer player, out string errorMessage) {
        if (!_playersData.ContainsKey(player)) {
            errorMessage = "No such player!";
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null list of sunk ships...", player);
            return null;
        }

        List<IShip> sunkShips = new();
        var ships = _playersData[player].PlayerBoard.ShipsOnBoard;
        foreach (var ship in ships) {
            if (!ship.Value) {
                sunkShips.Add(ship.Key);
            }
        }

        errorMessage = string.Empty;
        _log?.LogInformation(@"Player {player} successfully found. Player's sunked ships: {ships}",
                             player,
                             sunkShips);
        return sunkShips;
    }

    /// <summary>
    /// Utility method to get all the ammo owned by a specific player
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="errorMessage">Error message if player not found</param>
    /// <returns>A dictionary of ammo types and each of their amounts</returns>
    public IDictionary<IAmmo, int>? GetPlayerAmmoStock(IPlayer player, out string errorMessage) {
        if (!_playersData.ContainsKey(player)) {
            errorMessage = "No such player!";
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null dictionary of ammo stock...", player);
            return null;
        }

        errorMessage = string.Empty;
        var tempStock = _playersData[player].Ammo;
        _log?.LogInformation(@"Player {player} successfully found. Ammo stock: {stock}",
                             player,
                             tempStock);
        return tempStock;
    }

    /// <summary>
    /// Utility method to get the ammo count of a specific ammo
    /// </summary>
    /// <param name="player">The player which the data is from</param>
    /// <param name="ammoType">Specific ammo which is being queried</param>
    /// <param name="errorMessage">Error message if player not found or if ammo type not found</param>
    /// <returns>Amount of ammo left</returns>
    public int? GetPlayerAmmoCount(IPlayer player, IAmmo ammoType, out string errorMessage) {
        var ammo = GetPlayerAmmoStock(player, out errorMessage);
        if (errorMessage == "No such player!") {
            _log?.LogWarning(@"Player {player} does not exist in the current game. Returning null int of ammo count...", player);
            return null;
        }

        if (!ammo.ContainsKey(ammoType)) {
            errorMessage = "No such ammo!";
            _log?.LogWarning(@"Ammo {ammo} does not exist in the current game. Returning null int of ammo count...", ammoType);
            return null;
        }

        var tempCount = ammo?[ammoType];
        _log?.LogInformation(@"Player {player} and Ammo {ammo} successfully found. Ammo count: {count}",
                             player,
                             ammoType,
                             tempCount);
        return tempCount;
    }
}
