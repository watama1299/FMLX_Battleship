using Battleship.Utils;
using Battleship.Utils.Enums;
namespace Battleship.Ships;

public abstract class Ship : IShip
{
    public virtual int ShipLength {get; private set;}
    public virtual Dictionary<Position, PegType> Positions {get; private set;}
    public virtual bool IsAlive {get; private set;} = true;



    public Dictionary<Position, PegType> AssignPositions(Position startCoords, ShipOrientation orientation) {
        var tempPos = GeneratePositions(startCoords, orientation);
        return AssignPositions(tempPos);
    }

    public Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositions, PegType peg = PegType.NONE) {
        var temp = new Dictionary<Position, PegType>();
        foreach (var pos in generatedPositions) {
            temp.Add(pos, peg);
        }
        Positions = temp;
        return Positions;
    }

    protected Dictionary<Position, PegType> AssignPositions(List<Position> generatedPositions, Dictionary<Position, PegType> positions, PegType peg = PegType.NONE) {
        Positions = positions;
        return AssignPositions(generatedPositions, peg);
    }

    public List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation) {
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

    protected List<Position> GeneratePositions(Position startCoords, ShipOrientation orientation, int length) {
        ShipLength = length;
        return GeneratePositions(startCoords, orientation);
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
}
