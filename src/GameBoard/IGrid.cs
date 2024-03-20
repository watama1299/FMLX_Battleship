using Battleship.Utils;
namespace Battleship.GameBoard;

/// <summary>
/// Interface which defines the properties and methods that a Grid in a Battleship game should have
/// </summary>
/// <typeparam name="T">Any object which will be contained within the grid</typeparam>
public interface IGrid<T>
{
    /// <summary>
    /// An 2D array of <c>T</c>
    /// </summary>
    public T[,] Items {get;}

    /// <summary>
    /// The total amount of units/boxes in the grid
    /// </summary>
    public int TotalGrid {get;}


    
    /// <summary>
    /// Method to place an item <c>T</c> on the grid
    /// </summary>
    /// <param name="position">Position on the grid</param>
    /// <param name="item">Item to be placed on the grid</param>
    /// <returns>
    /// <c>true</c> if placement is successful
    /// <c>false</c> if placement is not successful
    /// </returns>
    public bool PlaceItemOnGrid(Position position, T item);

    /// <summary>
    /// Method to place multiple items in multiple positions
    /// </summary>
    /// <param name="itemAndPositions">Dictionary containing positions and item <c>T</c></param>
    /// <returns>
    /// <c>true</c> if placement is successful
    /// <c>false</c> if placement is not successful
    /// </returns>
    public bool PlaceItemOnGrid(IDictionary<Position, T> itemAndPositions);

    /// <summary>
    /// Method to check whether a grid contains a position
    /// </summary>
    /// <param name="position">Position to be checked</param>
    /// <returns>
    /// <c>true</c> if position exists on the grid
    /// <c>false</c> if position doesn't exist on the grid
    /// </returns>
    public bool ContainsPosition(Position position);

    /// <summary>
    /// Method to check whether a position is empty or has an item
    /// </summary>
    /// <param name="position">Position to be checked</param>
    /// <returns>
    /// <c>true</c> if position is empty
    /// <c>false</c> if position is not empty
    /// </returns>
    public bool IsPositionEmpty(Position position);

    /// <summary>
    /// Method to check whether multiple positions are empty or has an item
    /// </summary>
    /// <param name="positions">Positions to be checked</param>
    /// <returns>
    /// <c>true</c> if all positions are empty
    /// <c>false</c> if any of the positions are not empty
    /// </returns>
    public bool IsPositionEmpty(IEnumerable<Position> positions);
}
