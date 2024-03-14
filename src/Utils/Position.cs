namespace Battleship.Utils;

public struct Position
{
    public int X {get; private set;}
    public int Y {get; private set;}

    public Position(int x, int y) {
        X = x;
        Y = y;
    }

    // override object.Equals
    public override bool Equals(object obj)
    {        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        return X == ((Position) obj).X
            && Y == ((Position) obj).Y;
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
