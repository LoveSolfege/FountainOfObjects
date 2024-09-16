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
                Utils.PrintColoredText(RoomToText[GetRoomType(PlayerLocation.Row, PlayerLocation.Column)], ConsoleColor.DarkBlue);
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

        public void GetSurroundings() {

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

        public void CheckForDeath() {
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

        private Dictionary<RoomType, String> RoomToText = new Dictionary<RoomType, String>() {
            {RoomType.Entrance, "You see light coming from the cavern entrance"},
            {RoomType.Empty, String.Empty},
            {RoomType.Pit, "You fell into a pit and died" },
            {RoomType.FountainRoom, "You hear water dripping in this room, THe Fountain of Objects is here!" }
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
