using HiddenFountain.Entities.Rooms;
using HiddenFountain.Models;
using HiddenFountain.Settings;

namespace HiddenFountain.GameLogic {
    internal class Level
    {
        private static readonly Random random = new();
        public readonly FountainRoom fountain;
        private static int WorldSize => DifficultySettings.WorldSize;
        public Room[,] LevelGrid { get; private set; }
        public Point StartingLocation { get; private set; }

        public Level()
        {
            LevelGrid = new Room[WorldSize, WorldSize];
            fountain = new FountainRoom();
            FillGrid();
        }

        private void FillGrid()
        {
            //fill cave with empty rooms
            FillWholeGrid<EmptyRoom>();
            //place entrance and fountain
            StartingLocation = RandomlyPlaceRoom<EntranceRoom>();
            _ = RandomlyPlaceRoom<FountainRoom>();
            // Place Pits
            for (int i = 0; i < DifficultySettings.PitCount; i++)
            {
                _ = RandomlyPlaceRoom<PitRoom>();
            }
        }

        public Room GetRoomType(int row, int column) {
            return LevelGrid[row, column];
        }

        private Point RandomlyPlaceRoom<T>() where T : Room, new(){

            int row, col;
            do
            {
                row = random.Next(WorldSize);
                col = random.Next(WorldSize);

            } while (LevelGrid[row, col] is not EmptyRoom);

            LevelGrid[row, col] = new T();
            return new Point(row, col);
        }

        private void FillWholeGrid<T>() where T : Room, new() {
            for (int i = 0; i < WorldSize; i++) {
                for (int j = 0; j < WorldSize; j++) {
                        LevelGrid[i, j] = new T();
                }
            }
        }

        private void FillNeighbors<T>() where T : Room, new() { 
            
        }

    }

}
