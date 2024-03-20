using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

/// <summary>
/// Abstract class which implements the functionalities of the methods from the <c>IShip</c> interface
/// </summary>
public abstract class Ship : IShip
{
    public virtual int ShipLength {get; protected set;}
    public virtual IDictionary<Position, PegType> Positions {get; protected set;}
    public virtual bool IsAlive {get; protected set;} = true;



    public virtual IEnumerable<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
        var possiblePositions = new List<Position>();
        if (orientation == ShipOrientation.VERTICAL) {
            for (int i = 0; i < ShipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X + i, startCoords.Y));
            }
        } else {
            for (int i = 0; i < ShipLength; i++) {
                possiblePositions.Add(new Position(startCoords.X, startCoords.Y + i));
            }
        }
        return possiblePositions;
    }

    public virtual IDictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        var tempPos = GeneratePositions(startCoords, orientation);
        return AssignPositions(tempPos);
    }

    public virtual IDictionary<Position, PegType> AssignPositions(IEnumerable<Position> generatedPositions, PegType peg = PegType.NONE) {
        var temp = new Dictionary<Position, PegType>();
        foreach (var pos in generatedPositions) {
            temp.Add(pos, peg);
        }
        Positions = temp;
        return Positions;
    }

    public virtual PegType GetPegOnPosition(Position pos) {
        var output = new PegType();
        foreach (var kv in Positions) {
            if (kv.Key.Equals(pos)) {
                output = kv.Value;
                break;
            }
        }
        return output;
    }

    public virtual bool SetPegOnPosition(Position pos, PegType peg) {
        bool output = false;
        foreach (var kv in Positions) {
            if (kv.Key.Equals(pos)) Positions[kv.Key] = peg;
            output = true;
        }
        return output;
    }
    


    public virtual bool SinkShip() {
        if (IsAlive) {
            int count = 0;
            foreach (var kv in Positions) {
                if (kv.Value == PegType.HIT) count++;
            }
            if (count == Positions.Count) IsAlive = false;
        }
        return IsAlive;
    }

    public abstract IShip Clone();
}
