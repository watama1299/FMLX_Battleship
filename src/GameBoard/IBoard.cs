using Battleship.Utils.Enums;
using Battleship.Utils;
using Battleship.Ships;
using Battleship.Ammo;

namespace Battleship.GameBoard;

/// <summary>
/// Interface which defines the board that each player uses in a game of Battleship
/// </summary>
public interface IBoard
{
    /// <summary>
    /// A grid of ships owned by the player
    /// </summary>
    public IGrid<IShip> GridShip {get;}

    /// <summary>
    /// A dictionary of ships owned by the player and whether its alive or dead
    /// </summary>
    public IDictionary<IShip, bool> ShipsOnBoard {get;}

    /// <summary>
    /// A grid of pegs owned by the player to keep track of opponent board
    /// </summary>
    public IGrid<PegType> GridPeg {get;}



    /// <summary>
    /// Method to put a ship on the board
    /// </summary>
    /// <param name="playerShip">Ship to be put on the board</param>
    /// <param name="position">Where it is positioned on the board</param>
    /// <param name="orientation">Orientation of the ship</param>
    /// <returns>
    /// <c>true</c> if placement is successful
    /// <c>false</c> if placement is not successful
    /// </returns>
    public bool PutShipOnBoard(IShip playerShip, Position position, ShipOrientation orientation);

    /// <summary>
    /// Method to get the ship at a certain position on the board
    /// </summary>
    /// <param name="position">A position on the board</param>
    /// <returns>
    /// The ship located at that position,
    /// or <c>null</c> if no ship is found
    /// </returns>
    public IShip? GetShipOnBoard(Position position);

    /// <summary>
    /// Method to check whether there is a ship at a certain position on the board
    /// </summary>
    /// <param name="position">A position on the board</param>
    /// <returns>
    /// <c>true</c> if there is a ship
    /// <c>false</c> if there isn't any
    /// </returns>
    public bool CheckShipGridPosition(Position position);

    /// <summary>
    /// Method to check multiple positions whether there are ships
    /// </summary>
    /// <param name="positions">List of positions to check</param>
    /// <returns>
    /// <c>true</c> if there is a ship
    /// <c>false</c> if there isn't any
    /// </returns>
    public bool CheckShipGridPosition(IEnumerable<Position> positions);


    
    /// <summary>
    /// Method to handle incoming attack from the opponent
    /// Updates the player's grid ship
    /// </summary>
    /// <param name="originPosition">Position chosen by the opponent</param>
    /// <param name="shotType">Ammo type being used in the attack</param>
    /// <returns>
    /// A dictionary of positions and whether the attack hit or missed on those positions
    /// </returns>
    public IDictionary<Position, PegType> IncomingAttack(Position originPosition, IAmmo shotType);

    /// <summary>
    /// Method to put pegs on the grid peg
    /// </summary>
    /// <param name="pegPositions">A dictionary of positions and the type of pegs to be put on those positions</param>
    public void PutPegOnBoard(IDictionary<Position, PegType> pegPositions);
}
