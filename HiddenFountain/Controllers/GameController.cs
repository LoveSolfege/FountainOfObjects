using HiddenFountain.Commands;
using HiddenFountain.Constants;
using HiddenFountain.Entities;
using HiddenFountain.Entities.Rooms;
using HiddenFountain.Enums;
using HiddenFountain.Extensions;
using HiddenFountain.GameLogic;
using HiddenFountain.Interfaces;
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
        private static DateTime startTime;
        private static TimeSpan timeSpent;

        private GameController() {
            gameLevel = new Level();
            player = new Player(gameLevel.StartingLocation.Row, gameLevel.StartingLocation.Col);
        }

        public static GameController SelectGameDifficulty(IConfiguration config, ConsoleColor color = ConsoleColor.Gray) {
            Utils.ClearConsolePlaceHeader(GameStrings.AskToSelectDifficulty, color);
            Utils.PrintColoredText(GameStrings.ListDifficultyOptions, color);

            Difficulty diff = Difficulty.None;
            while (diff == Difficulty.None) {
                string choice = Utils.GetInput(GameStrings.AskForOption, ColorSettings.ChoiceColor).ToLower();
                diff = choice switch {
                    "1" => Difficulty.Easy,
                    "2" => Difficulty.Medium,
                    "3" => Difficulty.Hard,
                    _ => Difficulty.None
                };
                if(diff == Difficulty.None) {
                    Utils.PrintColoredText(GameStrings.InvalidDifficultySelection, ColorSettings.WarningColor);
                }
            }
            DifficultySettings.LoadDifficultySettings(config, diff);
            return new GameController();
        }

        public void Run()
        {
            startTime = DateTime.Now;
            Console.Clear();
            PrintCurrentInfo();
            while (player.IsAlive && !player.Won)
            {
                string choice = Utils.GetInput(GameStrings.AskWhatToDo, ColorSettings.ChoiceColor).ToLower();
                PlayerAction action = CommandManager.ExecuteCommand(choice);
                ProcessAction(action);
            }
            PrintResult();
        }

        private void PrintCurrentInfo() {
            Utils.PrintColoredText(player.GetLocation(), ColorSettings.MenuColor);
            EnteringSense();
            SenseSurround();
        }

        private void SenseSurround() {
            List<Point> surroundings = GetSurroundings();
            //shuffle neighbors to randomize the order of sensing
            surroundings.Shuffle();
            foreach (var point in surroundings) {
                Room room = gameLevel.GetRoomType(point.Row, point.Col);

                if (room is ISensible sensibleRoom) {
                    sensibleRoom.Sense(ColorSettings.SenseColor);
                }
            }
        }

        private void EnteringSense() {
            if (gameLevel.GetRoomType(player.Position.Row, player.Position.Col) is ITextOnEntering enteringRoom) {
                enteringRoom.EnteringText(ColorSettings.EnteringColor);
            }
        }

        private List<Point> GetSurroundings() {
            return GridManagar.GetManhattanNeighbors(gameLevel.LevelGrid, player.Position, 1);
        }

        private void ProcessAction(PlayerAction action) {
            if (MovementByAction.TryGetValue(action, out var movement)) {
                HandleMovement(movement.row, movement.column);
                PrintCurrentInfo();
                CheckPlayerStatus();
            }
            else if ((action == PlayerAction.ActivateFountain || action == PlayerAction.DeactivateFountain)
                && gameLevel.GetRoomType(player.Position.Row, player.Position.Col) is FountainRoom) {
                gameLevel.Fountain.Toggle();
            }
            else if(action == PlayerAction.Help) {
                Utils.PrintColoredText(GameStrings.HelpText, ColorSettings.HelpColor);
            }
            else {
                Utils.PrintColoredText(GameStrings.BadMoveChoice, ColorSettings.WarningColor);
            }
        }

        private static readonly Dictionary<PlayerAction, (int row, int column)> MovementByAction = new() {
            { PlayerAction.MoveUp, (-1, 0) },
            { PlayerAction.MoveDown, (1, 0) },
            { PlayerAction.MoveLeft, (0, -1) },
            { PlayerAction.MoveRight, (0, 1) },
        };

        private void HandleMovement(int rowDirection, int columnDirection) {
            int movementRow = player.Position.Row + rowDirection;
            int movementCol = player.Position.Col + columnDirection;
            int caveLength = gameLevel.LevelGrid.GetLength(0);

            if (GridManagar.IsValidRoom(movementRow, movementCol, caveLength)) {
                player.UpdatePosition(movementRow, movementCol);
            }
            else {
                Utils.PrintColoredText(GameStrings.HitWall, ColorSettings.WarningColor);
            }
        }

        private void CheckForWin() {
            if (player.IsAlive
                && player.Position == gameLevel.StartingLocation 
                && gameLevel.Fountain.Enabled) {
                player.MakeWin();
                timeSpent = startTime - DateTime.Now;
            }
        }

        private void CheckForDeath() {
            Room playerRoom = gameLevel.GetRoomType(player.Position.Row, player.Position.Col);

            if (playerRoom is PitRoom) {
                _causeOfDeath = GameStrings.DiedInPit;
                player.Die();
                timeSpent = DateTime.Now - startTime;
            }
        }

        private void CheckPlayerStatus() {
            CheckForDeath();
            CheckForWin();
        }

        private void PrintResult() {
            if (player.Won) {
                Utils.ClearConsolePlaceHeader(GameStrings.Win, ColorSettings.SuccessColor);
            }
            else {
                Utils.ClearConsolePlaceHeader(_causeOfDeath, ColorSettings.FailureColor);
            }
            Utils.PrintColoredText($"Time spent in the cave: {(int)timeSpent.TotalMinutes} minutes and {timeSpent.TotalSeconds:F1} seconds", ColorSettings.MenuColor);
            Console.ReadKey();
        }
    }
}
