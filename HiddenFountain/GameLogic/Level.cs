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
            
            //place entrance and surround with empty rooms
            StartingLocation = RandomlyPlaceRoom<EntranceRoom>();
            FillNeighbors<EmptyRoom>(StartingLocation, radius: 2);
            //place entrance and surround with empty room 
            Point FountainRoom = RandomlyPlaceRoom<FountainRoom>();
            FillNeighbors<EmptyRoom>(FountainRoom, radius: 1);
            // Place Pits
            for (int i = 0; i < DifficultySettings.PitCount; i++)
            {
                _ = RandomlyPlaceRoom<PitRoom>();
            }
            //fill rest of the cave with empty rooms
            FillWholeGrid<EmptyRoom>();
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

        private void FillNeighbors<T>(Point room, int radius) where T : Room, new() {
            List<Point> neighbors = GridManagar.GetNeighbors(LevelGrid, room, radius);

            foreach (Point neighbor in neighbors) {
                LevelGrid[neighbor.Row,neighbor.Col] = new T();
            }
        }

    }

}
