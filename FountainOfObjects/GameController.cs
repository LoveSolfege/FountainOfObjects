using FountainOfObjects.Entities;
using FountainOfObjects.Utilities;
using FountainOfObjects.Settings;
using FountainOfObjects.Enums;

namespace FountainOfObjects
{
    internal class GameController {
        private bool FountainEnabled { get; set; } = false;
        private string _causeOfDeath;
        private Level gameLevel;
        private Player player;
        private RoomType[,] cave;

        public void Run() {
            Console.Clear();
            PrintCurrentLocationInfo();
            while (player.IsAlive && !player.Won) {
                string choice = Utils.GetInput("What do you want to do? ", ColorSettings.ChoiceColor).ToLower();
                if (ChoiceToAction.TryGetValue(choice, out PlayerAction action)) {
                    SelectAction(action);
                    PrintCurrentLocationInfo();
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
                Utils.PrintColoredText("Fountain of Object was activated", ConsoleColor.DarkCyan);
            }
            else if(action == PlayerAction.DisableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
                FountainEnabled = false;
                Utils.PrintColoredText("Fountain of Object was deactivated", ConsoleColor.Blue);
            }
        }

        private void PrintResult() {
            if (player.Won)
            {
                Utils.ClearConsolePlaceHeader("Congratulations! You Win!", ConsoleColor.Green);
            }
            else {
                Utils.ClearConsolePlaceHeader(_causeOfDeath, ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private void PrintCurrentLocationInfo() {
            //prints player location at row/col
            Utils.PrintColoredText(player.ToString(), ColorSettings.MenuColor);

            //prints surrounings if any present
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

            if ( playerRoom == RoomType.Pit) {
                _causeOfDeath = "You fell into the pit and died.";
                player.IsAlive = false;
            }
            if (playerRoom == RoomType.Amarok) {
                _causeOfDeath = "You were eaten by Amarok.";
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
            {RoomType.FountainRoom, "You hear water dripping nearby, The Fountain of Objects is close!" },
            {RoomType.Amarok, "You can smell the rotten stench of an amarok in a nearby room." }
        };

        private Dictionary<RoomType, String> EnteringRoomText = new Dictionary<RoomType, String>() {
            {RoomType.Entrance, "You see light coming from the cavern entrance"},
            {RoomType.Empty, String.Empty},
            {RoomType.Pit, String.Empty },
            {RoomType.FountainRoom, "You hear water dripping in this room, The Fountain of Objects is here!" },
            {RoomType.Amarok, String.Empty}
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
