using Battleship.Utils;
using Battleship.Utils.Enums;
using Battleship.Ships;
using Battleship.Ammo;
using System.Net;

namespace Battleship.GameBoard;

public class Board : IBoard
{
    public IGrid<IShip> GridShip {get; private set;}
    public Dictionary<IShip, bool> ShipsOnBoard {get; private set;} = new();
    public IGrid<PegType> GridPeg {get; private set;}



    public Board(int squareGridSize) {
        GridShip = new Grid<IShip>(squareGridSize);
        GridPeg = new Grid<PegType>(squareGridSize);
    }

    public Board(int rows, int cols) {
        GridShip = new Grid<IShip>(rows, cols);
        GridPeg = new Grid<PegType>(rows, cols);
    }



    public bool PutShipOnBoard(
        IShip playerShip,
        Position position,
        ShipOrientation orientation
        ) {
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
    public bool CheckShipGridPosition(List<Position> positions) {
        return GridShip.IsPositionEmpty(positions);
    }



    public Dictionary<Position, PegType> IncomingAttack(Position originPosition, IAmmo shotType) {
        Dictionary<Position, PegType> output = new();
        
        List<Position> positionsShot = shotType.Shoot(originPosition);
        
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
            if (!attackedShip.Positions.ContainsValue(PegType.NONE)) {
                bool sunk = attackedShip.SinkShip();
                SetShipStatus(shipCheck, sunk);
            }
        }

        return output;
    }
    private void PutPegOnBoard(Position position, PegType peg) {
        var currentPeg = GridPeg.Items[position.X, position.Y];
        if (currentPeg == PegType.NONE) GridPeg.PlaceItemOnGrid(position, peg);
    }
    public void PutPegOnBoard(Dictionary<Position, PegType> pegPositions) {
        foreach (var kv in pegPositions) {
            PutPegOnBoard(kv.Key, kv.Value);
        }
    }

    private void SetShipStatus(IShip ship, bool isAlive) {
        foreach (var kv in ShipsOnBoard) {
            if (ship.Equals(kv.Key)) ShipsOnBoard[kv.Key] = isAlive;
        }
    }
}
