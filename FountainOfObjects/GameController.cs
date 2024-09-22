using FountainOfObjects.Entities;
using FountainOfObjects.Utilities;
using FountainOfObjects.Settings;
using FountainOfObjects.Enums;

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
                if (ChoiceToAction.TryGetValue(choice, out PlayerAction action)) {
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
            gameLevel = new Level();
            cave = gameLevel.LevelGrid;
            player = new Player(gameLevel.StartingRow, gameLevel.StartingCol);
        }


        public void SelectAction(PlayerAction action) {
            if (MovementByActions.TryGetValue(action, out var movement)) {
                HandleMovement(movement.row, movement.column);
                Utils.PrintColoredText(EnteringRoomText[GetRoomType(player.Location.Row, player.Location.Col)], ColorSettings.SenseColor);
                CheckPlayerStatus();
            }
            else if (action == PlayerAction.EnableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
                FountainEnabled = true;
            }
            else if(action == PlayerAction.DisableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
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
            RoomType playerRoom = GetRoomType(player.Location.Row, player.Location.Col);

            if ( playerRoom == RoomType.Pit || playerRoom == RoomType.Amarok) {
                player.IsAlive = false;
            }

            //add cause of death pls
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

        private Dictionary<PlayerAction, (int row, int column)> MovementByActions = new Dictionary<PlayerAction, (int row, int column)>() {
            { PlayerAction.MoveUp, (-1, 0) },
            { PlayerAction.MoveDown, (1, 0) },
            { PlayerAction.MoveLeft, (0, -1) },
            { PlayerAction.MoveRight, (0, 1) },
        };

        private Dictionary<String, PlayerAction> ChoiceToAction = new Dictionary<String, PlayerAction>() {
            {"move up", PlayerAction.MoveUp },
            {"move down", PlayerAction.MoveDown },
            {"move left", PlayerAction.MoveLeft },
            {"move right", PlayerAction.MoveRight },
            {"enable fountain", PlayerAction.EnableFountain },
            {"disable fountain", PlayerAction.DisableFountain }
        };

    }
}
