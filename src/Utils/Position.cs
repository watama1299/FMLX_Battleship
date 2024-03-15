namespace Battleship.Utils;

public struct Position
{
    public int X {get; private set;}
    public int Y {get; private set;}

    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    // // override object.Equals
    // public override bool Equals(object? obj)
    // {
    //     if (obj == null || GetType() != obj.GetType())
    //     {
    //         return false;
    //     }

    //     return X == ((Position) obj).X
    //         && Y == ((Position) obj).Y;
    // }

    // // override object.GetHashCode
    // public override int GetHashCode()
    // {
    //     return (X, Y).GetHashCode();
    // }


    public override string ToString()
    {
        return $"{this.X},{this.Y}";
    }


    public override bool Equals(object? obj) => obj is Position other && this.Equals(other);

    public readonly bool Equals(Position p) => X == p.X && Y == p.Y;

    public override int GetHashCode() =>  X ^ Y;

    public static bool operator ==(Position lhs, Position rhs) => lhs.Equals(rhs);

    public static bool operator !=(Position lhs, Position rhs) => !(lhs == rhs);
}
