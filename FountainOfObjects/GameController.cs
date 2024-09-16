using System.Data;
using Utilities;

namespace FountainOfObjects {
    internal class GameController {
        private Level gameLevel;
        private RoomType[,] cave;
        public (int Row, int Column) PlayerLocation;

        public bool PlayerAlive { get; private set; } = true;
        public bool PlayerWon { get; private set; } = false;
        public bool FountainEnabled { get; private set; } = false;

        public GameController(LevelSize levelSize) {
            gameLevel = new Level(levelSize);
            cave = gameLevel.LevelGrid;
            PlayerLocation = gameLevel.StartingLocation;
        }

        public void SelectAction(Action action) {
            if (MovementByActions.TryGetValue(action, out var movement)) {
                HandleMovement(movement.row, movement.column);
                Utils.PrintColoredText(EnteringRoomText[GetRoomType(PlayerLocation.Row, PlayerLocation.Column)], ConsoleColor.DarkBlue);
                CheckPlayerStatus();
            }
            else if (action == Action.EnableFountain && GetRoomType(PlayerLocation.Row, PlayerLocation.Column) == RoomType.FountainRoom) {
                FountainEnabled = true;
            }
            else if(action == Action.DisableFountain && GetRoomType(PlayerLocation.Row, PlayerLocation.Column) == RoomType.FountainRoom) {
                FountainEnabled = false;
            }
            else {
                throw new NotImplementedException();
            }
        }

        public void PrintResult() {
            if (PlayerWon)
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
                int newRow = PlayerLocation.Row + row;
                int newCol = PlayerLocation.Column + col;
                if(IsValidRoom(newRow, newCol)){
                    Utils.PrintColoredText(SensingRoomText[GetRoomType(newRow, newCol)], ConsoleColor.DarkBlue);
                }
            }
        }

        private void CheckPlayerStatus() {
            CheckForDeath();
            CheckForWin();
        }

        private void CheckForWin() {
            if(PlayerAlive && PlayerLocation == gameLevel.StartingLocation && FountainEnabled) {
                PlayerWon = true;
            }
        }

        private void CheckForDeath() {
            if(GetRoomType(PlayerLocation.Row, PlayerLocation.Column) == RoomType.Pit) {
                PlayerAlive = false;
            }
        }

        private void HandleMovement(int rowDirection, int columnDirection) {
            int row = PlayerLocation.Row + rowDirection;
            int column = PlayerLocation.Column + columnDirection;
            if (IsValidRoom(row, column)) {
                PlayerLocation = (row, column);
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
