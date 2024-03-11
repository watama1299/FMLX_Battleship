using Battleship.Enums;
namespace Battleship;

public class Ship : IShip
{
    public ShipType Type {get; private set;}
    public Dictionary<Position, PegType> Positions {get; private set;}
    public bool IsAlive {get; private set;}



    public Ship(ShipType type) {
        Type = type;
        Positions = new();
        IsAlive = true;
    }

    public Dictionary<Position, PegType> PlaceShip(Position startCoords, ShipOrientation orientation) {
        var tempPos = GeneratePositions(startCoords, orientation);
        PlaceShip(tempPos);
        return Positions;
    }

    public Dictionary<Position, PegType> PlaceShip(List<Position> generatedPositons, PegType peg = PegType.NONE) {
        foreach (var pos in generatedPositons) {
            Positions.Add(pos, peg);
        }
        return Positions;
    }

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        var pos = new List<Position>();
        int length = (int) Type;
        if (orientation == ShipOrientation.VERTICAL) {
            for (int i = 0; i < length; i++) {
                pos.Add(new Position(startCoords.X, startCoords.Y + i));
            }
        } else {
            for (int i = 0; i < length; i++) {
                pos.Add(new Position(startCoords.X + i, startCoords.Y));
            }
        }
        return pos;
    }

    public bool SinkShip() {
        if (IsAlive) {
            IsAlive = false;
        }

        return IsAlive;
    }
}
