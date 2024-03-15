using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public abstract class Ship : IShip
{
    public virtual int ShipLength {get; private set;}
    public virtual Dictionary<Position, PegType> Positions {get; private set;}
    public virtual bool IsAlive {get; private set;} = true;



    public IEnumerable<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
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

    protected IEnumerable<Position> GeneratePositions(Position startCoords, ShipOrientation orientation, int length) {
        ShipLength = length;
        return GeneratePositions(startCoords, orientation);
    }

    public IDictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        var tempPos = GeneratePositions(startCoords, orientation);
        return AssignPositions(tempPos);
    }

    public IDictionary<Position, PegType> AssignPositions(IEnumerable<Position> generatedPositions, PegType peg = PegType.NONE) {
        var temp = new Dictionary<Position, PegType>();
        foreach (var pos in generatedPositions) {
            temp.Add(pos, peg);
        }
        Positions = temp;
        return Positions;
    }

    protected IDictionary<Position, PegType> AssignPositions(IEnumerable<Position> generatedPositions, IDictionary<Position, PegType> positions, PegType peg = PegType.NONE) {
        Positions = (Dictionary<Position, PegType>) positions;
        return AssignPositions(generatedPositions, peg);
    }

    public virtual PegType GetPegOnPosition(Position pos) {
        var output = new PegType();
        foreach (var kv in Positions) {
            if (kv.Key.Equals(pos)) output = kv.Value;
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
    
    public bool SinkShip() {
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
