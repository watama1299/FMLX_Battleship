using System.Net;
using Battleship.Enums;
namespace Battleship;

public class GameController
{
    // private int _turn;
    private IBoard _templateBoard;
    private List<IShip> _templateShips;
    private List<IPlayer> _players = new(2);
    private Dictionary<IPlayer, PlayerBattleshipData> _playerData = new(2);
    private Queue<IPlayer> _activePlayer = new(2);
    public GameStatus Status {get; private set;}

    public GameController(
        IPlayer p1,
        IPlayer p2,
        IBoard board,
        List<IShip> ships
        ) {
            _players.Add(p1);
            _players.Add(p2);

            _templateBoard = board;
            _templateShips = ships;

            var dataP1 = new PlayerBattleshipData(_templateBoard, _templateShips);
            var dataP2 = new PlayerBattleshipData(_templateBoard, _templateShips);
            _playerData.Add(p1, dataP1);
            _playerData.Add(p2, dataP2);

            Status = GameStatus.INIT;
    }


    public void GivePlayersAmmo(IShoot ammoType, int amount) {
        foreach (var pd in _playerData.Values) {
            pd.GiveAmmo(ammoType, amount);
        }
    }
    public void StartGame() {
        if (Status != GameStatus.INIT) return;

        Random rng = new();
        if (rng.Next(10) < 5) {
            _activePlayer.Enqueue(_players[0]);
            _activePlayer.Enqueue(_players[1]);
        } else {
            _activePlayer.Enqueue(_players[1]);
            _activePlayer.Enqueue(_players[0]);
        }

        Status = GameStatus.ONGOING;
    }
    public IPlayer NextTurn() {
        if (Status != GameStatus.ONGOING) {
            throw new Exception("No ongoing game!");
        }

        var player = _activePlayer.Dequeue();
        _activePlayer.Enqueue(player);
        return player;
    }
    public bool CheckPlayerShips(IPlayer player) {
        var playerShips = _playerData[player].PlayerShips;
        if (playerShips is null || playerShips.Count == 0) {
            throw new Exception("Player has no ships!");
        }

        foreach (var ship in playerShips) {
            if (ship.HasSunk == false) return false;
        }
        Status = GameStatus.ENDED;
        return true;
    }
    public void ResetGame() {
        if (Status != GameStatus.ENDED) return;

        var tempData = new Dictionary<IPlayer, PlayerBattleshipData>(2);
        foreach (var player in _players) {
            tempData.Add(player, new PlayerBattleshipData(_templateBoard, _templateShips));
        }
        _playerData = tempData;
        Status = GameStatus.INIT;
    }
    public void ResetGame(IBoard newBoard, List<IShip> newShips) {
        if (Status != GameStatus.ENDED) return;

        _templateBoard = newBoard;
        _templateShips = newShips;
        ResetGame();
    }



    public bool PlayerPlaceShip(
        IPlayer player,
        IShip ship,
        Position startCoord,
        ShipOrientation orientation
        ) {
            var playerBoard = _playerData[player].PlayerBoard;
            return playerBoard.PutShipOnBoard(ship, startCoord, orientation);
    }


    public void PlayerShoot(
        IPlayer originPlayer,
        IPlayer targetPlayer,
        Position position,
        IShoot shootMode
        ) {

    }



    public bool GetHitOrMiss(IPlayer player, Position position) {
        return true;
    }
    public IShip GetShipFromPos(Position position) {
        return new Ship(ShipType.DESTROYER);
    }
    public List<Position> GetPosFromShip(IShip ship) {
        return new List<Position>();
    }
    public bool SinkShip(IShip ship) {
        return true;
    }

}
