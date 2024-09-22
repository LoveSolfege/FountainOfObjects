﻿using FountainOfObjects.Entities;
using FountainOfObjects.Utilities;
using FountainOfObjects.Settings;

namespace FountainOfObjects
{
    internal class GameController {
        private bool FountainEnabled { get; set; } = false;
        private Level gameLevel;
        private Player player;
        private RoomType[,] cave;

        public void Run() {
            Console.Clear();
            while (player.IsAlive && !player.Won) {
                Utils.PrintColoredText(player.ToString(), ColorSettings.MenuColor);
                GetSurroundings();
                string choice = Utils.GetInput("What do you want to do? ", ColorSettings.ChoiceColor).ToLower();
                if (ChoiceToAction.TryGetValue(choice, out Action action)) {
                    SelectAction(action);
                    Console.WriteLine();
                }
                else {
                    Utils.PrintColoredText("Choice is unsuppported, command help to get help", ColorSettings.WarningColor);
                }
            }
            PrintResult();
        }

        public GameController(Difficulty diff) {
            gameLevel = new Level(diff);
            cave = gameLevel.LevelGrid;
            player = new Player(gameLevel.StartingRow, gameLevel.StartingCol);
        }


        public void SelectAction(Action action) {
            if (MovementByActions.TryGetValue(action, out var movement)) {
                HandleMovement(movement.row, movement.column);
                Utils.PrintColoredText(EnteringRoomText[GetRoomType(player.Location.Row, player.Location.Col)], ColorSettings.SenseColor);
                CheckPlayerStatus();
            }
            else if (action == Action.EnableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
                FountainEnabled = true;
            }
            else if(action == Action.DisableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
                FountainEnabled = false;
            }
        }

        public void PrintResult() {
            if (player.Won)
            {
                Utils.ClearConsolePlaceHeader("Congratulations! You Win!", ConsoleColor.Green);
            }
            else {
                Utils.ClearConsolePlaceHeader("Game Over! you died.", ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        public void GetSurroundings() {
            foreach (var (key, (row, col)) in MovementByActions) {
                int newRow = player.Location.Row + row;
                int newCol = player.Location.Col + col;
                if(IsValidRoom(newRow, newCol)){
                    Utils.PrintColoredText(SensingRoomText[GetRoomType(newRow, newCol)], ColorSettings.SenseColor);
                }
            }
        }

        private void CheckPlayerStatus() {
            CheckForDeath();
            CheckForWin();
        }

        private void CheckForWin() {
            if(player.IsAlive && player.Location == gameLevel.StartingLocation && FountainEnabled) {
                player.Won = true;
            }
        }

        private void CheckForDeath() {
            if(GetRoomType(player.Location.Row, player.Location.Col) == RoomType.Pit) {
                player.IsAlive = false;
            }
        }

        private void HandleMovement(int rowDirection, int columnDirection) {
            int movementRow = player.Location.Row + rowDirection;
            int movementCol = player.Location.Col + columnDirection;

            if (IsValidRoom(movementRow, movementCol)) {
                player.UpdatePosition(movementRow, movementCol);
            }
            else {
                Utils.PrintColoredText("You've hit the wall", ConsoleColor.DarkYellow);
            }
        }

        private bool IsValidRoom(int row, int column) {
            return row < cave.GetLength(0) && row >= 0 && column < cave.GetLength(1) && column >= 0;
        }

        private RoomType GetRoomType(int row, int column) { 
            return gameLevel.LevelGrid[row, column];
        }


        private Dictionary<RoomType, String> SensingRoomText = new Dictionary<RoomType, String>() {
            {RoomType.Entrance, String.Empty},
            {RoomType.Empty, String.Empty},
            {RoomType.Pit, "You feel a draft. There is a pit in a nearby room." },
            {RoomType.FountainRoom, "You hear water dripping nearby, The Fountain of Objects is close!" }
        };

        private Dictionary<RoomType, String> EnteringRoomText = new Dictionary<RoomType, String>() {
            {RoomType.Entrance, "You see light coming from the cavern entrance"},
            {RoomType.Empty, String.Empty},
            {RoomType.Pit, "You fell into a pit and died" },
            {RoomType.FountainRoom, "You hear water dripping in this room, The Fountain of Objects is here!" }
        };

        private Dictionary<Action, (int row, int column)> MovementByActions = new Dictionary<Action, (int row, int column)>() {
            { Action.MoveUp, (-1, 0) },
            { Action.MoveDown, (1, 0) },
            { Action.MoveLeft, (0, -1) },
            { Action.MoveRight, (0, 1) },
        };

        private Dictionary<String, Action> ChoiceToAction = new Dictionary<String, Action>() {
            {"move up", Action.MoveUp },
            {"move down", Action.MoveDown },
            {"move left", Action.MoveLeft },
            {"move right", Action.MoveRight },
            {"enable fountain", Action.EnableFountain },
            {"disable fountain", Action.DisableFountain }
        };

    }

    public enum Action {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        EnableFountain,
        DisableFountain
    }
}
