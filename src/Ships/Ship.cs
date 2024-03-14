using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public abstract class Ship : IShip
{
    public int ShipLength {get; private set;}
    public Dictionary<Position, PegType> Positions {get; private set;}
    public bool IsAlive {get; private set;}



    public Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        var tempPos = GeneratePositions(startCoords, orientation);
        return AssignPositions(tempPos);
    }

    public Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositions, PegType peg = PegType.NONE) {
        foreach (var pos in generatedPositions) {
            Positions.Add(pos, peg);
        }
        return Positions;
    }

    protected Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositions, Dictionary<Position, PegType> positions, PegType peg = PegType.NONE) {
        Positions = positions;
        return AssignPositions(generatedPositions, peg);
    }

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        var possiblePositions = new List<Position>();
        if (orientation == ShipOrientation.VERTICAL) {
            for (int i = 0; i < ShipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X + i, startCoords.Y));
            }
        } else {
            for (int i = 0; i < ShipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X, startCoords.Y + i));
            }
        }
        return possiblePositions;
    }

    protected List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation, int length) {
        ShipLength = length;
        return GeneratePositions(startCoords, orientation);
    }
    
    public bool SinkShip() {
        if (IsAlive & !Positions.Values.ToList().Contains(PegType.NONE) ) {
            IsAlive = false;
        }

        return IsAlive;
    }
}
