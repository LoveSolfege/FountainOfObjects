using FountainOfObjects.Entities;
using FountainOfObjects.Utilities;

namespace FountainOfObjects
{
    internal class GameController {
        private bool FountainEnabled { get; set; } = false;
        private Level gameLevel;
        private Player player;
        private RoomType[,] cave;

        public void Run() {
            while (true) {
                Console.Clear();
                while (player.IsAlive && !player.Won) {
                    Utils.PrintColoredText($"You are in the room at Row {player.Location.Row}, Column {player.Location.Col}", menuColor);
                    GetSurroundings();
                    string choice = Utils.GetInput("what do you want to do? ", choiceColor).ToLower();
                    if (ChoiceToAction.TryGetValue(choice, out Action action)) {
                        SelectAction(action);
                        Console.WriteLine();
                    }
                    else {
                        Console.WriteLine("think again...\n");
                    }
                }
                PrintResult();
            }
        }

        public GameController(LevelSize levelSize) {
            gameLevel = new Level(levelSize);
            cave = gameLevel.LevelGrid;
            player = new Player(gameLevel.StartingRow, gameLevel.StartingCol);
        }

        public void SelectAction(Action action) {
            if (MovementByActions.TryGetValue(action, out var movement)) {
                HandleMovement(movement.row, movement.column);
                Utils.PrintColoredText(EnteringRoomText[GetRoomType(player.Location.Row, player.Location.Col)], ConsoleColor.DarkBlue);
                CheckPlayerStatus();
            }
            else if (action == Action.EnableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
                FountainEnabled = true;
            }
            else if(action == Action.DisableFountain && GetRoomType(player.Location.Row, player.Location.Col) == RoomType.FountainRoom) {
                FountainEnabled = false;
            }
            else {
                throw new NotImplementedException();
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
            int row = player.Location.Row + rowDirection;
            int column = player.Location.Col + columnDirection;
            if (IsValidRoom(row, column)) {
                player.UpdatePosition(row, column);
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
