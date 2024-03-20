namespace Battleship.Utils;

/// <summary>
/// Struct to ease positional data handling during a game of Battleship
/// </summary>
public struct Position
{
    /// <summary>
    /// x-coordinate / column number
    /// </summary>
    public int X {get; private set;}

    /// <summary>
    /// y-coordinate / row number
    /// </summary>
    public int Y {get; private set;}



    /// <summary>
    /// Constructor for a Position struct
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
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
