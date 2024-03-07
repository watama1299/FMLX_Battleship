using Battleship.Enums;
namespace Battleship;

public class Ship : IShip
{
    public int Id {get; private set;}
    public ShipType Type {get; private set;}
    public List<Position> Positions {get; set;}
    public bool HasSunk {get; set;}

    public Ship(int id, ShipType type) {
        Id = id;
        Type = type;
        Positions = new();
    }

    public List<Position> PlaceShip(Position startCoords, ShipOrientation orientation) {
        var pos = new List<Position>();
        if (orientation == ShipOrientation.VERTICAL) {}
        return pos;
    }

    public List<Position> GetPositions() {
        return Positions;
    }

    private List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
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
}
