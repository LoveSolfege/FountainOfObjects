using FountainOfObjects.Settings;
using System.Diagnostics;

namespace FountainOfObjects {
    internal class Level {
        private static readonly Random random = new();
        public int WorldSize { get; private set; }
        public int PitCount { get; private set; }
        public int MaelstromCount { get; private set; }
        public int AmarokCount { get; private set; }


        public RoomType[,] LevelGrid { get; private set; }
        public int StartingRow { get; private set; }
        public int StartingCol { get; private set; }
        public (int Row , int Col) StartingLocation { get { return (StartingRow, StartingCol); } }

        

        public Level(Difficulty diff) { 
            SetDifficulty(diff);
            LevelGrid = new RoomType[WorldSize, WorldSize];
            FillGrid();

            Debug.WriteLine($"world size {WorldSize} Pit Count {PitCount} MaelstromCount { MaelstromCount} Amarok Count {AmarokCount}");
        }
            
        private void FillGrid() {
            int rows = LevelGrid.GetLength(0);
            int columns = LevelGrid.GetLength(1);

            (StartingRow, StartingCol) = GetRandomPosition(rows, columns);
            (int fountainRow, int fountainCol) = GetRandomUniquePosition(rows, columns, StartingRow, StartingCol);

            LevelGrid[StartingRow, StartingCol] = RoomType.Entrance;
            LevelGrid[fountainRow, fountainCol] = RoomType.FountainRoom;

            for (int r = 0; r < rows; r++){
                for (int c = 0; c < columns; c++) {
                    if (LevelGrid[r,c] == RoomType.Empty) {
                        LevelGrid[r, c] = GetRandomRoom();
                    }
                }
            }
        }

        private void SetDifficulty(Difficulty diff) {
            string gameSize = DifficultyToString[diff];

            WorldSize = DifficultySettings.GetCount(ConfigurationManager.Configuration, nameof(WorldSize), gameSize);
            PitCount = DifficultySettings.GetCount(ConfigurationManager.Configuration, nameof(PitCount), gameSize);
            MaelstromCount = DifficultySettings.GetCount(ConfigurationManager.Configuration, nameof(MaelstromCount), gameSize);
            AmarokCount = DifficultySettings.GetCount(ConfigurationManager.Configuration, nameof(AmarokCount), gameSize);
        }

        private Dictionary<Difficulty, String> DifficultyToString = new() {
            { Difficulty.Easy, "SmallWorld"},
            { Difficulty.Medium, "MediumWorld" },
            { Difficulty.Hard, "LargeWorld" }
        };


        private (int, int) GetRandomPosition(int rows, int columns) { 
            int row = random.Next(0, rows);
            int column = random.Next(0, columns);
            return (row, column);
        }

        private (int, int) GetRandomUniquePosition(int rows, int columns, int excludedRow, int excludedColumn) {
            int row, column;
            do {
                (row,  column) = GetRandomPosition(rows, columns);
            } while (row == excludedRow && column == excludedColumn);

            return (row, column);
        }

        private RoomType GetRandomRoom() {
            int value = random.Next(0, 100);

            if (value < 80)
                return RoomType.Empty;
            else
                return RoomType.Pit;
        }

    }

    public enum RoomType {
        Empty,
        Entrance,
        FountainRoom,
        Pit
    }

    public enum Difficulty {
        Easy,
        Medium,
        Hard
    }

}
