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
        private static Player player;
        private readonly Level gameLevel;

        private GameController(Difficulty diff) {
            gameLevel = new Level();
            player = new Player(gameLevel.StartingLocation.Row, gameLevel.StartingLocation.Col);
        }

        public static GameController SelectGameDifficulty(IConfiguration config, ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader(GameStrings.AskToSelectDifficulty, color);
            Utils.PrintColoredText(GameStrings.ListDifficultyOptions, color);

            Difficulty diff = Difficulty.None;
            while (diff == Difficulty.None) {
                string choice = Utils.GetInput(GameStrings.AskForOption, ColorSettings.ChoiceColor).ToLower();
                //if (choice == "exit") return null;
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
                string choice = Utils.GetInput(GameStrings.WhatToDo, ColorSettings.ChoiceColor).ToLower();
                if (ChoiceToAction.TryGetValue(choice, out PlayerAction action))
                {
                    SelectAction(action);
                    PrintCurrentLocationInfo();
                }
                else
                {
                    Utils.PrintColoredText(GameStrings.BadChoice, ColorSettings.WarningColor);
                }
            }
            PrintResult();
        }

        public void SelectAction(PlayerAction action)
        {
            if (MovementByAction.TryGetValue(action, out var movement))
            {
                HandleMovement(movement.row, movement.column);
                Utils.PrintColoredText(EnteringRoomText[GetRoomType(player.PositionRow, player.PositionCol)], ColorSettings.SenseColor);
                CheckPlayerStatus();
            }
            else if (action == PlayerAction.EnableFountain || action == PlayerAction.DisableFountain && GetRoomType(player.PositionRow, player.PositionCol) is FountainRoom)
            {
                gameLevel.fountain.Toggle();
            }
        }

        private void PrintCurrentLocationInfo()
        {
            //prints player location at row/col
            Utils.PrintColoredText(player.GetLocation(), ColorSettings.MenuColor);

            //prints surrounings if any present
            foreach (var (key, (row, col)) in MovementByAction)
            {
                int newRow = player.PositionRow + row;
                int newCol = player.PositionCol + col;
                if (gameLevel.IsValidRoom(newRow, newCol))
                {
                    Utils.PrintColoredText(SensingRoomText[GetRoomType(newRow, newCol)], ColorSettings.SenseColor);
                }
            }
        }

        private void CheckForDeath()
        {
            Room playerRoom = gameLevel.GetRoomType(player.PositionRow, player.PositionCol);

            if (playerRoom is PitRoom) {
                _causeOfDeath = GameStrings.DiedInPit;
                player.Die();
            }
        }

        private Dictionary<string, PlayerAction> ChoiceToAction = new Dictionary<string, PlayerAction>() {
            {"move up", PlayerAction.MoveUp },
            {"move down", PlayerAction.MoveDown },
            {"move left", PlayerAction.MoveLeft },
            {"move right", PlayerAction.MoveRight },
            {"enable fountain", PlayerAction.EnableFountain },
            {"disable fountain", PlayerAction.DisableFountain }
        };





        private static readonly Dictionary<PlayerAction, (int row, int column)> MovementByAction = new() {
            { PlayerAction.MoveUp, (-1, 0) },
            { PlayerAction.MoveDown, (1, 0) },
            { PlayerAction.MoveLeft, (0, -1) },
            { PlayerAction.MoveRight, (0, 1) },
        };

        private void HandleMovement(int rowDirection, int columnDirection) {
            int movementRow = player.PositionRow + rowDirection;
            int movementCol = player.PositionCol + columnDirection;

            if (gameLevel.IsValidRoom(movementRow, movementCol)) {
                player.UpdatePosition(movementRow, movementCol);
            }
            else {
                Utils.PrintColoredText(GameStrings.HitWall, ConsoleColor.DarkYellow);
            }
        }

        private void CheckForWin() {
            if (player.IsAlive && player.Position == gameLevel.StartingLocation && gameLevel.fountain.Enabled) {
                player.MakeWin();
            }
        }
        private void CheckPlayerStatus() {
            CheckForDeath();
            CheckForWin();
        }

        private void PrintResult() {
            if (player.Won) {
                Utils.ClearConsolePlaceHeader(GameStrings.Win, ConsoleColor.Green);
            }
            else {
                Utils.ClearConsolePlaceHeader(_causeOfDeath, ConsoleColor.Red);
            }
            Console.ReadKey();
        }
    }
}
