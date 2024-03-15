namespace Battleship.Utils;

public struct Position
{
    public int X {get; private set;}
    public int Y {get; private set;}



    public Position(int x, int y) {
        X = x;
        Y = y;
    }



    public override string ToString()
    {
        return $"[[{this.X},{this.Y}]]";
    }

    public override bool Equals(object? obj) => obj is Position other && this.Equals(other);

    public readonly bool Equals(Position p) => X == p.X && Y == p.Y;

    public override int GetHashCode() =>  HashCode.Combine(X, Y);

    public static bool operator ==(Position lhs, Position rhs) => lhs.Equals(rhs);

    public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);
}
