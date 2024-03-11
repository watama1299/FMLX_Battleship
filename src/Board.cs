using Battleship.Enums;

namespace Battleship;

public class Board : IBoard
{
    // public int Id {get; private set;}
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
            var shipPositions = playerShip.GeneratePositions(position, orientation);

            // First check whether positions are valid
            foreach (var pos in shipPositions) {
                if (!GridShip.IsPositionEmpty(pos)) {
                    return false;
                }
            }

            // If positions valid, then place
            foreach (var pos in shipPositions) {
                GridShip.PlaceOnGrid(pos, playerShip);
            }
            playerShip.PlaceShip(shipPositions);
            ShipsOnBoard.Add(playerShip, playerShip.IsAlive);
            return true;
        }
    public IShip? GetShipOnBoard(Position position) {
        var ship = GridShip.Items[position.X, position.Y];
        return ship;
    }

    public bool CheckShipGridPosition(Position position) {
        return GridShip.IsPositionEmpty(position);
    }
    public bool CheckShipGridPosition(List<Position> positions) {
        return GridShip.IsPositionEmpty(positions);
    }

    public Dictionary<Position, PegType> IncomingAttack(Position originPosition, IShoot shotType) {
        Dictionary<Position, PegType> output = new();
        
        List<Position> shotPositions = shotType.Shoot(originPosition);
        
        foreach (var pos in shotPositions) {
            // if target location is empty then pegtype miss
            if (GridShip.IsPositionEmpty(pos)) {
                output.Add(pos, PegType.MISS);

                // update GridShip
                IShip tempShip = new Ship(ShipType.NO_SHIP);
                var tempList = new List<Position>();
                tempList.Add(pos);
                tempShip.PlaceShip(tempList, PegType.MISS);
                GridShip.PlaceOnGrid(pos, tempShip);
                continue;
            }

            // first check if pos has been hit bfore
            var shipCheck = GridShip.Items[pos.X, pos.Y];
            var peg = shipCheck.Positions[pos];
            if (peg != PegType.NONE) {
                continue;
            }

            // else pegtype hit
            output.Add(pos, PegType.HIT);

            // update ship info to hit
            IShip attackedShip = GridShip.Items[pos.X, pos.Y];
            attackedShip.Positions[pos] = PegType.HIT;
            if (!attackedShip.Positions.ContainsValue(PegType.NONE)) {
                bool sunk = attackedShip.SinkShip();
                ShipsOnBoard[attackedShip] = sunk;
            }
        }

        return output;
    }
    
    private void PutPegOnBoard(Position position, PegType peg) {
        if (!GridPeg.IsPositionEmpty(position)) {
            GridPeg.PlaceOnGrid(position, peg);
        }
    }

    public void PutPegOnBoard(Dictionary<Position, PegType> pegPositions) {
        foreach (var kv in pegPositions) {
            PutPegOnBoard(kv.Key, kv.Value);
        }
    }
}
