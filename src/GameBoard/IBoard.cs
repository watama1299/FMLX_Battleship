using Battleship.Utils.Enums;
using Battleship.Utils;
using Battleship.Ships;
using Battleship.Ammo;

namespace Battleship.GameBoard;

/// <summary>
/// 
/// </summary>
public interface IBoard
{
    /// <summary>
    /// 
    /// </summary>
    public IGrid<IShip> GridShip {get;}

    /// <summary>
    /// 
    /// </summary>
    public IDictionary<IShip, bool> ShipsOnBoard {get;}

    /// <summary>
    /// 
    /// </summary>
    public IGrid<PegType> GridPeg {get;}



    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerShip"></param>
    /// <param name="position"></param>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public bool PutShipOnBoard(IShip playerShip, Position position, ShipOrientation orientation);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public IShip? GetShipOnBoard(Position position);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckShipGridPosition(Position position);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    public bool CheckShipGridPosition(IEnumerable<Position> positions);


    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="originPosition"></param>
    /// <param name="shotType"></param>
    /// <returns></returns>
    public IDictionary<Position, PegType> IncomingAttack(Position originPosition, IAmmo shotType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pegPositions"></param>
    public void PutPegOnBoard(IDictionary<Position, PegType> pegPositions);
}
