using FountainOfObjects.Enums;
using FountainOfObjects.Settings;

namespace FountainOfObjects {
    internal class Level {
        private static readonly Random random = new();
        public RoomType[,] LevelGrid { get; private set; }
        public int StartingRow { get; private set; }
        public int StartingCol { get; private set; }
        public (int Row , int Col) StartingLocation { get { return (StartingRow, StartingCol); } }
        private int WorldSize => DifficultySettings.WorldSize;
        private int PitCount => DifficultySettings.PitCount;
        private int AmarokCount => DifficultySettings.AmarokCount;

        public Level() {
            LevelGrid = new RoomType[WorldSize, WorldSize];
            FillGrid();
        }
            
        private void FillGrid() {
            // Place Entrance
            (StartingRow, StartingCol) = PlaceRoom(RoomType.Entrance);

            // Place Fountain Room
            PlaceRoom(RoomType.FountainRoom);

            // Place Pits
            for (int i = 0; i < PitCount; i++) {
                PlaceRoom(RoomType.Pit);
            }

            // Place Amaroks
            for (int i = 0; i < AmarokCount; i++) {
                PlaceRoom(RoomType.Amarok);
            }
        }

        private (int row, int col) PlaceRoom(RoomType roomType) {
            int row, col;
            do {
                row = random.Next(WorldSize);
                col = random.Next(WorldSize);
            } while (LevelGrid[row, col] != RoomType.Empty);

            LevelGrid[row, col] = roomType;
            return (row, col);
        }

    }

}
