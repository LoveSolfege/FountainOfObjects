using HiddenFountain.Entities.Rooms;
using HiddenFountain.Models;
using HiddenFountain.Settings;

namespace HiddenFountain.GameLogic {
    internal class Level
    {
        private static readonly Random random = new();
        private static int WorldSize => DifficultySettings.WorldSize;
        public Room[,] LevelGrid { get; private set; }
        public Point StartingLocation { get; private set; }
        public FountainRoom Fountain { get; private set; }

        public Level()
        {
            LevelGrid = new Room[WorldSize, WorldSize];
            FillGrid();
        }

        private void FillGrid()
        {
            //place entrance 
            StartingLocation = RandomlyPlaceRoom<EntranceRoom>();
            //surround with empty rooms by manhattan distance of 2
            FillManhattanNeighbors<EmptyRoom>(StartingLocation, radius: 2);
            //place fountain surround with empty room 
            Point FountainRoom = RandomlyPlaceRoom<FountainRoom>();
            //assign fountain
            Fountain = (FountainRoom)LevelGrid[FountainRoom.Row, FountainRoom.Col];
            //surround with empty room by adjacent distance of 1
            FillAdjacentNeighbors<EmptyRoom>(FountainRoom, radius: 1);
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

            } while (LevelGrid[row, col] is not null);

            LevelGrid[row, col] = new T();
            return new Point(row, col);
        }

        private void FillWholeGrid<T>() where T : Room, new() {
            for (int i = 0; i < WorldSize; i++) {
                for (int j = 0; j < WorldSize; j++) {
                    if(LevelGrid[i, j] == null) {
                        LevelGrid[i, j] = new T();
                    }
                }
            }
        }

        private void FillManhattanNeighbors<T>(Point room, int radius) where T : Room, new(){
            List<Point> neighbors = GridManagar.GetManhattanNeighbors(LevelGrid, room, radius);
            CreateRoomFromList<T>(neighbors);
        }

        private void FillAdjacentNeighbors<T>(Point room, int radius) where T : Room, new() {
            List<Point> neighbors = GridManagar.GetAdjacentNeighbors(LevelGrid, room, radius);
            CreateRoomFromList<T>(neighbors);
        }

        private void CreateRoomFromList<T>(List<Point> rooms) where T : Room, new() {
            foreach (Point room in rooms) {
                LevelGrid[room.Row, room.Col] = new T();
            }
        }

        public void PrintLevelGrid() {
            for (int i = 0; i < WorldSize; i++) {
                for (int j = 0; j < WorldSize; j++) {
                    Room room = LevelGrid[i, j];
                    if (room != null) {
                        Console.Write(room.ToString() + " ");
                    }
                    else {
                        Console.Write("? "); // Uninitialized room
                    }
                }
                Console.WriteLine(); // New line for the next row
            }
        }
    }

}
