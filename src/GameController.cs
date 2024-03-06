namespace Battleship;

public class GameController
{
    private int _turn;
    private List<IPlayer> _players;
    private Dictionary<IPlayer, PlayerBattleshipData> _playerData;
    private IPlayer _activePlayer;

    public GameController(IPlayer p1, IPlayer p2, IBoard board1, IBoard board2) {
        
    }

    public void StartTurn() {}
    public void NextTurn() {}
    public void PlayerPlaceShip(IPlayer player, IShip ship, int startCoord, ShipOrientation orientation) {}
    public void PlayerShoot(IPlayer originPlayer, IPlayer targetPlayer, IPosition position, IShoot shootMode) {}
    public bool GetHitOrMiss(IPlayer player, IPosition position) {return true;}
    public void PlacePeg(IPeg peg, IPosition position) {}
    public IShip GetShipFromPos(IPosition position) {return new Ship(1,ShipType.DESTROYER);}
    public List<IPosition> GetPosFromShip(IShip ship) {return new List<IPosition>();}
    public bool SinkShip(IShip ship) {return true;}

}
