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
    }

    public List<Position> GetPositions() {
        return Positions;
    }

    public List<Position> PlaceShip(int startCoords, ShipOrientation orientation) {
        var pos = new List<Position>();
        
        
        return pos;
    }
}
