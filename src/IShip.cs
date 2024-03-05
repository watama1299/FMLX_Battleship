namespace Battleship;

public interface IShip
{
    public int Id {get;}
    public List<Position> Positions {get; set;}
    public bool HasSunk {get; set;}

    public List<Position> PlaceShip(int startCoords, ShipOrientation orientation);
}
