using HiddenFountain.Entities.Rooms;
using HiddenFountain.Models;
using HiddenFountain.Settings;

namespace HiddenFountain.GameLogic {
    internal class Level
    {
        private static readonly Random random = new();
        private int _startingRow = 0;
        private int _startingCol = 0;
        private int WorldSize => DifficultySettings.WorldSize;
        private int PitCount => DifficultySettings.PitCount;
        public Room[,] LevelGrid { get; private set; }
        public (int Row, int Col) StartingLocation {
            get => (_startingRow, _startingCol); 
            private set {
                _startingRow = value.Row;
                _startingCol = value.Col;
            }
        }

        public Level()
        {
            LevelGrid = new Room[WorldSize, WorldSize];
            FillGrid();
        }

        private void FillGrid()
        {
            //fill grid with  rooms
            Room empty = new EmptyRoom();
            Room entrance = new EntranceRoom();
            Room fountain = new FountainRoom();
            Room pit = new PitRoom();

            FillGrid(empty);
            StartingLocation = RandomlyPlaceRoom(entrance);
            RandomlyPlaceRoom(fountain);

            // Place Pits
            for (int i = 0; i < PitCount; i++)
            {
                RandomlyPlaceRoom(pit);
            }
        }

        private (int row, int col) RandomlyPlaceRoom(Room room)
        {
            int row, col;
            do
            {
                row = random.Next(WorldSize);
                col = random.Next(WorldSize);
            } while (LevelGrid[row, col] is EmptyRoom);

            LevelGrid[row, col] = room;
            return (row, col);
        }

        private void FillGrid(Room room) {
            for (int i = 0; i < WorldSize; i++) {
                for (int j = 0; j < WorldSize; j++) {
                    LevelGrid[i, j] = room;
                }
            }
        }

    }

}
