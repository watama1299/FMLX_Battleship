using Battleship.Utils.Enums;
using Battleship.Utils;
using Battleship.Ships;
using Battleship.Ammo;

namespace Battleship.GameBoard;

public interface IBoard
{
    // public int Id {get;}
    public IGrid<IShip> GridShip {get;}
    public Dictionary<IShip, bool> ShipsOnBoard {get;}
    public IGrid<PegType> GridPeg {get;}

    public bool PutShipOnBoard(
        IShip playerShip,
        Position position,
        ShipOrientation orientation
        );

    public IShip? GetShipOnBoard(Position position);
    public bool CheckShipGridPosition(Position position);
    public bool CheckShipGridPosition(List<Position> positions);
    public Dictionary<Position, PegType> IncomingAttack(Position originPosition, IAmmo shotType);
    public void PutPegOnBoard(Dictionary<Position, PegType> pegPositions);
}
