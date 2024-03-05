namespace Battleship;

public class Position : IPosition
{
    public int X {get; set;}
    public int Y {get; set;}

    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    public void GetPosition(Position pos) {}
}
