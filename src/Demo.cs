﻿using Battleship;
using Battleship.Ammo;
using Battleship.GameBoard;
using Battleship.Players;
using Battleship.Ships;
using Battleship.Utils;
using Battleship.Utils.Enums;
using Spectre.Console;

namespace GameDemo;

class Demo {
    static void Main(string[] args) {
        Console.Write("Enter name for Player 1 > ");
        Player p1 = new Player("Yanto");

        Player p2 = new Player("Aditya");
        Console.WriteLine($"You're playing against > {p2.Name}");
        // Console.ReadLine();

        Console.WriteLine("Using a 10x10 board...");
        Board board = new Board(10);
        // Console.ReadLine();

        Console.WriteLine("Using default ship pieces...");
        List<IShip> ships = new List<IShip> {
            new ShipBattleship(),
            new ShipCarrier(),
            new ShipCruiser(),
            new ShipDestroyer(),
            new ShipSubmarine()
        };
        foreach (var ship in ships) {
            Console.WriteLine(ship + ": " + ship.ShipLength + " boxes");
        }
        // Console.ReadLine();

        Console.WriteLine("Initialising game...");
        var ammo = new Dictionary<IAmmo, int> {
            { new MissileBarrage(), 2 },
            { new MissileNuclear(), 1 }
        };

        GameController gc = new(p1, p2, board, ships, ammo);
        Console.WriteLine($"Game state: {gc.Status}");

        PrintPlayerInfo(gc, p1);
        // Console.ReadLine();


        Console.WriteLine("Placing P2 ships...");
        PlacingShipsRandom(gc, p2, ships);
        PrintPlayerInfo(gc, p2);
        // Console.ReadLine();
        

        Console.WriteLine("Place your ships...");
        Console.Write("Do you want to randomise your ships? (y/n) > ");
        var ans = Console.ReadLine();
        if (ans.ToLower() == "y") PlacingShipsRandom(gc, p1, ships);
        else if (ans.ToLower() == "n") PlacingShipsSet(gc, p1, ships);
        // PlacingShipsRandom(gc, p1, ships);
        PrintPlayerInfo(gc, p1);
        // Console.ReadLine();

        // for (int i = 0; i < gc.GetPlayerBoard(p1).GridShip.TotalGrid; i++) {
        //     gc.PlayerShoot(p1, p2, new Position(i/10, i%5), new MissileSingle());
        // }
        PrintPlayerInfo(gc, p1);
        PrintPlayerInfo(gc, p2);


        GameStatus gameStatus = gc.StartGame();
        var r = new Random();
        var c = new Random();
        while (gameStatus != GameStatus.INIT) {
            var activePlayer = gc.GetCurrentActivePlayer();
            Console.WriteLine($"{activePlayer.Name} is playing...");
            var nonActivePlayer = gc.PreviewNextPlayer();

            var posX = r.Next(10);
            var posY = c.Next(10);
            Console.WriteLine($"{activePlayer.Name} is trying to launch {new MissileSingle()} at position [[{posX}, {posY}]]");
            bool successfulShot = gc.PlayerShoot(activePlayer, nonActivePlayer, new Position(posX, posY), new MissileSingle());
            while (successfulShot) {
                posX = r.Next(10);
                posY = c.Next(10);
                Console.WriteLine($"{activePlayer.Name} is trying to launch {new MissileSingle()} at position [[{posX}, {posY}]]");
                successfulShot = gc.PlayerShoot(activePlayer, nonActivePlayer, new Position(posX, posY), new MissileSingle());
            }
            Thread.Sleep(r.Next(10));
            gameStatus = gc.NextPlayer();
            // Console.WriteLine(gameStatus);

            if (gameStatus == GameStatus.ENDED) {
                PrintPlayerInfo(gc, p1);
                PrintPlayerInfo(gc, p2);
                Console.WriteLine($"{nonActivePlayer.Name} wins!");

                Console.Write("Would you like to play again? (y/n) > ");
                if (Console.ReadLine() == "y") {
                    Console.Clear();
                    Console.WriteLine("Resetting game...");
                    gameStatus = gc.ResetGame();

                    Console.WriteLine("Place your ships...");
                    Console.Write("Do you want to randomise your ships? (y/n) > ");
                    ans = Console.ReadLine();
                    if (ans.ToLower() == "y") PlacingShipsRandom(gc, p1, ships);
                    else if (ans.ToLower() == "n") PlacingShipsSet(gc, p1, ships);
                    // PlacingShipsRandom(gc, p1, ships);
                    PlacingShipsRandom(gc, p2, ships);

                    PrintPlayerInfo(gc, p1);
                    PrintPlayerInfo(gc, p2);

                    Console.WriteLine("Starting game again...");
                    gameStatus = gc.StartGame();
                } else {
                    Environment.Exit(0);
                }
            }
        }
    }

    static Grid PrintGrid<T>(IGrid<T> gridItem) {
        int rows = gridItem.Items.GetLength(0);
        int cols = gridItem.Items.GetLength(1);


        var grid = new Grid().Centered();
        for (int a = 0; a < cols + 1; a++) {
            grid.AddColumn(new GridColumn().Centered());
        }


        for (int i = 0; i < rows + 1; i++) {
            string[] temp = new string[cols + 1];
            for (int j = 0; j < cols + 1; j++) {
                if (i == 0) {
                    if (j == 0) {
                        temp[j] = " ";
                    }
                    else {
                        temp[j] = ((char) ('A' + j - 1)).ToString();
                    }
                } else {
                    if (j == 0) {
                        temp[j] = (i).ToString();
                    }
                    else {

                        if (typeof(T) == typeof(IShip)) {
                            var item = gridItem.Items[i-1, j-1];
                            if (item is null) {
                                    temp[j] = "[[  ]]";
                                }
                            else {
                                var pos = new Position(i-1, j-1);
                                var peg = ((IShip) item).GetPegOnPosition(pos);
                                char[] chars = item.ToString()
                                                    .ToLower()
                                                    .ToCharArray();
                                string ch = chars.ElementAt(0).ToString() + chars.ElementAt(1).ToString();
                                if (ch == "bl") {
                                    temp[j] = $"[[OO]]";
                                }
                                else {
                                    if (peg == PegType.HIT) temp[j] = $"[red][[{ch}]][/]";
                                    else temp[j] = $"[green][[{ch}]][/]";
                                }
                            }
                        } 
                        
                        
                        else {
                            if (Equals(gridItem.Items[i-1, j-1], PegType.NONE)) {
                                temp[j] = "[[ ]]";
                            }
                            else if (Equals(gridItem.Items[i-1, j-1], PegType.MISS)) {
                                temp[j] = "[yellow][[O]][/]";
                            }
                            else if (Equals(gridItem.Items[i-1, j-1], PegType.HIT)){
                                temp[j] = "[red][[X]][/]";
                            }
                        }
                    }
                }                
            }
            grid.AddRow(temp).Centered();
        }
        return grid;
    }
    static Grid PrintAmmo(Dictionary<IAmmo, int> ammo) {
        var grid = new Grid();
        int ammoTypes = ammo.Keys.Count;

        grid.AddColumns(new GridColumn().Centered(), new GridColumn().Centered());
        grid.AddRow("Ammo Types", "Amount Left");
        foreach (var kv in ammo) {
            string[] temp = new string[2];
            temp[0] = kv.Key.ToString();
            temp[1] = kv.Value.ToString();
            grid.AddRow(temp);
        }

        return grid;
    }
    static void PrintPlayerInfo(GameController gc, IPlayer player) {
        var board = gc.GetPlayerData(player).PlayerBoard;
        var ammo = gc.GetPlayerData(player).Ammo;

        var ui = new Table()
            .Centered()
            .Title($"{player.Name}'s Deck");
        ui.AddColumn(new TableColumn("Your Ships").Centered());
        ui.AddColumn(new TableColumn("Enemy Ships").Centered());
        ui.AddColumn(new TableColumn("Ammo Stock").Centered());

        ui.AddRow(
            PrintGrid(board.GridShip).Centered(),
            PrintGrid(board.GridPeg).Centered(),
            PrintAmmo(ammo)
            );

        AnsiConsole.Write(ui);
    }
    static Position InputToPosition(int rows, int cols) {
        return new Position(rows, cols);
    }
    static Position InputToPosition(string input) {
        string[] temp = input.ToLower().Split(" ");
        int rows = int.Parse(temp[1]) - 1;
        int cols = char.Parse(temp[0]) - 'a';

        return new Position(rows, cols);
    }
    static void PlacingShipsSet(GameController gc, IPlayer player, List<IShip> gcShips) {
        var ships = gcShips.ToList();
        // gcShips.CopyTo(ships, 0);

        Console.WriteLine("Place your ships...");
        PrintPlayerInfo(gc, player);
        foreach (var ship in ships) {
            Console.Write($"Place your {ship.ToString()} at location (eg. A 1) > ");
            var pos = Console.ReadLine();

            Console.Write($"Direction (vertical/horizontal) (eg. v/h) > ");
            var inputOr = Console.ReadLine();
            ShipOrientation orientation = new();
            if (inputOr == "v") orientation = ShipOrientation.VERTICAL;
            else if (inputOr == "h") orientation = ShipOrientation.HORIZONTAL;

            bool accepted;
            accepted = gc.PlayerPlaceShip(player, ship, InputToPosition(pos), orientation);
            while (!accepted) {
                Console.WriteLine("Try again");
                Console.Write($"Place your {ship.ToString()} at location (eg. A 1) > ");
                pos = Console.ReadLine();

                Console.Write($"Direction (vertical/horizontal) (eg. v/h) > ");
                inputOr = Console.ReadLine();
                if (inputOr == "v") orientation = ShipOrientation.VERTICAL;
                else if (inputOr == "h") orientation = ShipOrientation.HORIZONTAL;
                accepted = gc.PlayerPlaceShip(player, ship, InputToPosition(pos), orientation);
            }
            PrintPlayerInfo(gc, player);
        }
    }
    static void PlacingShipsRandom(GameController gc, IPlayer player, List<IShip> gcShips) {
        Random rng = new();
        int boardRow = gc.GetPlayerBoard(player).GridShip.Items.GetLength(0);
        int boardCol = gc.GetPlayerBoard(player).GridShip.Items.GetLength(1);

        // var ships = new List<IShip>(gcShips);
        var ships = new List<IShip>();
        foreach (var ship in gcShips) {
            ships.Add(ship.Clone());
        }
        
        for (int i = 0; i < ships.Count; i++) {
            var pos = new Position(rng.Next(boardRow), rng.Next(boardCol));
            var rand = rng.Next(2);
            var orientation = new ShipOrientation();
            if (rand == 0) orientation = ShipOrientation.VERTICAL;
            else orientation = ShipOrientation.HORIZONTAL;

            bool accepted;
            accepted = gc.PlayerPlaceShip(player, ships[i], pos, orientation);
            while (!accepted) {
                pos = new Position(rng.Next(boardRow), rng.Next(boardCol));
                rand = rng.Next(2);
                if (rand == 0) orientation = ShipOrientation.VERTICAL;
                else orientation = ShipOrientation.HORIZONTAL;

                accepted = gc.PlayerPlaceShip(player, ships[i], pos, orientation);
            }
        }
    }
}