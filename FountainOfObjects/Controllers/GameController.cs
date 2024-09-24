using HiddenFountain.Constants;
using HiddenFountain.Entities;
using HiddenFountain.Entities.Rooms;
using HiddenFountain.Enums;
using HiddenFountain.GameLogic;
using HiddenFountain.Models;
using HiddenFountain.Settings;
using HiddenFountain.Utilities;
using Microsoft.Extensions.Configuration;

namespace HiddenFountain.Controllers {
    internal class GameController
    {
        private string _causeOfDeath;
        private readonly Level gameLevel;
        private readonly Player player;
        private readonly Room[,] cave;

        private GameController(Difficulty diff) {
            gameLevel = new Level();
            cave = gameLevel.LevelGrid;
            player = new Player(gameLevel.StartingLocation.Row, gameLevel.StartingLocation.Col);
        }

        public static GameController SelectGameDifficulty(IConfiguration config, ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader(GameStrings.AskToSelectDifficulty, color);
            Utils.PrintColoredText(GameStrings.ListDifficultyOptions, color);

            Difficulty diff = Difficulty.None;
            while (diff == Difficulty.None) {
                string choice = Utils.GetInput(GameStrings.AskForOption, ColorSettings.ChoiceColor);
                diff = choice switch {
                    "1" => Difficulty.Easy,
                    "2" => Difficulty.Medium,
                    "3" => Difficulty.Hard,
                    _ => Difficulty.None
                };
                if(diff == Difficulty.None) {
                    Utils.PrintColoredText(GameStrings.InvalidSelection, ColorSettings.WarningColor);
                }
            }
            DifficultySettings.LoadDifficultySettings(config, diff);
            return new GameController(diff);
        }


        public void Run()
        {
            Console.Clear();
            PrintCurrentLocationInfo();
            while (player.IsAlive && !player.Won)
            {
                string choice = Utils.GetInput("What do you want to do? ", ColorSettings.ChoiceColor).ToLower();
                if (ChoiceToAction.TryGetValue(choice, out PlayerAction action))
                {
                    SelectAction(action);
                    PrintCurrentLocationInfo();
                }
                else
                {
                    Utils.PrintColoredText("Choice is unsuppported, command help to get help", ColorSettings.WarningColor);
                }
            }
            PrintResult();
        }

        

        public void SelectAction(PlayerAction action)
        {
            if (MovementByActions.TryGetValue(action, out var movement))
            {
                HandleMovement(movement.row, movement.column);
                Utils.PrintColoredText(EnteringRoomText[GetRoomType(player.PositionRow, player.PositionCol)], ColorSettings.SenseColor);
                CheckPlayerStatus();
            }
            else if (action == PlayerAction.EnableFountain && GetRoomType(player.PositionRow, player.PositionCol) == RoomType.FountainRoom)
            {
                FountainEnabled = true;
                Utils.PrintColoredText("Fountain of Object was activated", ConsoleColor.DarkCyan);
            }
            else if (action == PlayerAction.DisableFountain && GetRoomType(player.PositionRow, player.PositionCol) == RoomType.FountainRoom)
            {
                FountainEnabled = false;
                Utils.PrintColoredText("Fountain of Object was deactivated", ConsoleColor.Blue);
            }
        }

        private void PrintResult()
        {
            if (player.Won)
            {
                Utils.ClearConsolePlaceHeader("Congratulations! You Win!", ConsoleColor.Green);
            }
            else
            {
                Utils.ClearConsolePlaceHeader(_causeOfDeath, ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private void PrintCurrentLocationInfo()
        {
            //prints player location at row/col
            Utils.PrintColoredText(player.GetLocation(), ColorSettings.MenuColor);

            //prints surrounings if any present
            foreach (var (key, (row, col)) in MovementByActions)
            {
                int newRow = player.PositionRow + row;
                int newCol = player.PositionCol + col;
                if (IsValidRoom(newRow, newCol))
                {
                    Utils.PrintColoredText(SensingRoomText[GetRoomType(newRow, newCol)], ColorSettings.SenseColor);
                }
            }
        }

        private void CheckPlayerStatus()
        {
            CheckForDeath();
            CheckForWin();
        }

        private void CheckForWin()
        {
            if (player.IsAlive && player.Position == gameLevel.StartingLocation && FountainEnabled)
            {
                player.MakeWin();
            }
        }

        private void CheckForDeath()
        {
            Room playerRoom = GetRoomType(player.PositionRow, player.PositionCol);

            if (playerRoom is PitRoom) {
                _causeOfDeath = "You fell into the pit and died.";
                player.Die();
            }
        }

        private void HandleMovement(int rowDirection, int columnDirection)
        {
            int movementRow = player.PositionRow + rowDirection;
            int movementCol = player.PositionCol + columnDirection;

            if (IsValidRoom(movementRow, movementCol))
            {
                player.UpdatePosition(movementRow, movementCol);
            }
            else
            {
                Utils.PrintColoredText("You've hit the wall", ConsoleColor.DarkYellow);
            }
        }

        private bool IsValidRoom(int row, int column)
        {
            return row < cave.GetLength(0) && row >= 0 && column < cave.GetLength(1) && column >= 0;
        }

        private Room GetRoomType(int row, int column)
        {
            return gameLevel.LevelGrid[row, column];
        }

        private Dictionary<PlayerAction, (int row, int column)> MovementByActions = new Dictionary<PlayerAction, (int row, int column)>() {
            { PlayerAction.MoveUp, (-1, 0) },
            { PlayerAction.MoveDown, (1, 0) },
            { PlayerAction.MoveLeft, (0, -1) },
            { PlayerAction.MoveRight, (0, 1) },
        };

        private Dictionary<string, PlayerAction> ChoiceToAction = new Dictionary<string, PlayerAction>() {
            {"move up", PlayerAction.MoveUp },
            {"move down", PlayerAction.MoveDown },
            {"move left", PlayerAction.MoveLeft },
            {"move right", PlayerAction.MoveRight },
            {"enable fountain", PlayerAction.EnableFountain },
            {"disable fountain", PlayerAction.DisableFountain }
        };

    }
}
