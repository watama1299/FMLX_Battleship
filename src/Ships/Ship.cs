using Battleship.Enums;
namespace Battleship.Ships;

public abstract class Ship : IShip
{
    public int ShipLength {get; private set;}
    public Dictionary<Position, PegType> Positions {get; private set;} = new();
    public bool IsAlive {get; private set;}



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
        if (orientation == ShipOrientation.VERTICAL) {
            for (int i = 0; i < ShipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X, startCoords.Y + i));
            }
        } else {
            for (int i = 0; i < ShipLength; i++) {
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
