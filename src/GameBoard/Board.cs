using Battleship.Utils;
using Battleship.Utils.Enums;
using Battleship.Ships;
using Battleship.Ammo;

namespace Battleship.GameBoard;

/// <summary>
/// Concrete class which implements the <c>IBoard</c> interface
/// </summary>
public class Board : IBoard
{
    public IGrid<IShip> GridShip {get; private set;}
    public IDictionary<IShip, bool> ShipsOnBoard {get; private set;}
    public IGrid<PegType> GridPeg {get; private set;}



    /// <summary>
    /// Constructor for boards that have square shaped grids
    /// </summary>
    /// <param name="squareGridSize">Length of one side in units/boxes</param>
    public Board(int squareGridSize) {
        GridShip = new Grid<IShip>(squareGridSize);
        GridPeg = new Grid<PegType>(squareGridSize);
        ShipsOnBoard = new Dictionary<IShip, bool>();
    }

    /// <summary>
    /// Constructor for boards that have rectangular shaped grids
    /// </summary>
    /// <param name="rows">Number of rows on the grid</param>
    /// <param name="cols">Number of columns on the grid</param>
    public Board(int rows, int cols) {
        GridShip = new Grid<IShip>(rows, cols);
        GridPeg = new Grid<PegType>(rows, cols);
        ShipsOnBoard = new Dictionary<IShip, bool>();
    }



    public bool PutShipOnBoard(IShip playerShip, Position position, ShipOrientation orientation) {
            var potentialPositions = playerShip.GeneratePositions(position, orientation);

            // First check whether positions are valid
            foreach (var pos in potentialPositions) {
                if (!GridShip.IsPositionEmpty(pos)) {
                    return false;
                }
            }

            // If positions valid, then place
            foreach (var pos in potentialPositions) {
                GridShip.PlaceItemOnGrid(pos, playerShip);
            }
            playerShip.AssignPositions(potentialPositions);
            ShipsOnBoard.Add(playerShip, true);
            return true;
        }
    public IShip? GetShipOnBoard(Position position) {
        IShip? ship = null;

        try {
            ship = GridShip.Items[position.X, position.Y];
        } catch (Exception) {
            return ship;
        }
        return ship;
    }
    public bool CheckShipGridPosition(Position position) {
        return GridShip.IsPositionEmpty(position);
    }
    public bool CheckShipGridPosition(IEnumerable<Position> positions) {
        return GridShip.IsPositionEmpty((List<Position>) positions);
    }



    public IDictionary<Position, PegType> IncomingAttack(Position originPosition, IAmmo shotType) {
        Dictionary<Position, PegType> output = new();
        
        var positionsShot = shotType.Shoot(originPosition);
        
        foreach (var pos in positionsShot) {
            if (!GridShip.ContainsPosition(pos)) continue;

            // if target location is empty then pegtype miss
            if (GridShip.IsPositionEmpty(pos)) {
                output.Add(pos, PegType.MISS);

                // update GridShip
                var tempShip = new ShipBlank();
                var tempList = new List<Position>() {pos};
                tempShip.AssignPositions(tempList, PegType.MISS);
                GridShip.PlaceItemOnGrid(pos, tempShip);
                continue;
            }

            // first check if pos has been hit bfore
            var shipCheck = GridShip.Items[pos.X, pos.Y];
            var peg = shipCheck.GetPegOnPosition(pos);
            if (peg != PegType.NONE) {
                continue;
            }

            // else pegtype hit
            output.Add(pos, PegType.HIT);

            // update info of ship that was hit
            IShip attackedShip = GetShipOnBoard(pos);
            attackedShip.SetPegOnPosition(pos, PegType.HIT);

            var tempDict = attackedShip.Positions.ToDictionary();
            if (!tempDict.ContainsValue(PegType.NONE)) {
                bool sunk = attackedShip.SinkShip();
                SetShipStatus(shipCheck, sunk);
            }
        }

        return output;
    }
    
    /// <summary>
    /// Helper method to update the status of the ships on board
    /// </summary>
    /// <param name="ship">Ship to be updated</param>
    /// <param name="isAlive">
    /// <c>true</c> if the ship is still alive, 
    /// <c>false</c> if the ship has sunk
    /// </param>
    private void SetShipStatus(IShip ship, bool isAlive) {
        var tempShips = new Dictionary<IShip, bool>();
        foreach (var kv in ShipsOnBoard) {
            if (!ship.Equals(kv.Key)) {
                tempShips.Add(kv.Key, kv.Value);
            } else {
                tempShips.Add(kv.Key, isAlive);
            }
        }
        ShipsOnBoard = tempShips;
    }

    /// <summary>
    /// Method to put a singular peg on a singular position
    /// </summary>
    /// <param name="position">Position on the grid</param>
    /// <param name="peg">Type of peg put in the position</param>
    private void PutPegOnBoard(Position position, PegType peg) {
        var currentPeg = GridPeg.Items[position.X, position.Y];
        if (currentPeg == PegType.NONE) GridPeg.PlaceItemOnGrid(position, peg);
    }

    public void PutPegOnBoard(IDictionary<Position, PegType> pegPositions) {
        foreach (var kv in pegPositions) {
            PutPegOnBoard(kv.Key, kv.Value);
        }
    }
}
