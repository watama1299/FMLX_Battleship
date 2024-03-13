using Battleship.Utils.Enums;
namespace Battleship.UNUSED;

public class SolidShip
{
    public ShipType Type {get; private set;}
    public Dictionary<Position, PegType> Positions {get; private set;}
    public bool IsAlive {get; private set;}



    public SolidShip(ShipType type) {
        Type = type;
        Positions = new();
        IsAlive = true;
    }

    public Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        var tempPos = GeneratePositions(startCoords, orientation);
        return AssignPositions(tempPos);
    }

    public Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositons, PegType peg = PegType.NONE) {
        foreach (var pos in generatedPositons) {
            Positions.Add(pos, peg);
        }
        return Positions;
    }

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        var possiblePositions = new List<Position>();
        int shipLength = (int) Type;
        if (orientation == ShipOrientation.VERTICAL) {
            for (int i = 0; i < shipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X, startCoords.Y + i));
            }
        } else {
            for (int i = 0; i < shipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X + i, startCoords.Y));
            }
        }
        return possiblePositions;
    }

    public bool SinkShip() {
        if (IsAlive & !Positions.Values.ToList().Contains(PegType.NONE) ) {
            IsAlive = false;
        }

        return IsAlive;
    }
}
