using Battleship.Enums;

namespace Battleship;

public interface IBoard
{
    // public int Id {get;}
    public IGrid<IShip> GridShip {get;}
    public IGrid<PegType> GridPeg {get;}

    public bool PutShipOnBoard(
        IShip playerShip,
        Position position,
        ShipOrientation orientation
        );

    public bool CheckShipGridPosition(Position position);
    public bool CheckShipGridPosition(List<Position> positions);
    public Dictionary<Position, PegType> IncomingAttack(Position originPosition, IShoot shotType);
    public void PutPegOnBoard(Dictionary<Position, PegType> pegPositions);
}
