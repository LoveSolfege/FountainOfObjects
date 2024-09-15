namespace FountainOfObjects {
    internal class Level {
        private static readonly Random random = new();
        public RoomType[,] LevelGrid { get; private set; }
        public (int,int) Entrance { get; private set; }

        public Level(LevelSize levelSize) { 
            LevelGrid = LevelGenerator[levelSize];
            FillGrid();
        }
            
        private void FillGrid() {
            int rows = LevelGrid.GetLength(0);
            int columns = LevelGrid.GetLength(1);

            (int entranceRow, int entranceCol) = GetRandomPosition(rows, columns);
            (int fountainRow, int fountainCol) = GetRandomUniquePosition(rows, columns, entranceRow, entranceCol);

            LevelGrid[entranceRow, entranceCol] = RoomType.Entrance;
            LevelGrid[fountainRow, fountainCol] = RoomType.FountainRoom;

            for (int r = 0; r < rows; r++){
                for (int c = 0; c < columns; c++) {
                    if (LevelGrid[r,c] == RoomType.Empty) {
                        LevelGrid[r, c] = GetRandomRoom();
                    }
                }
            }

            Entrance = (entranceRow, entranceCol);
        }

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


        private Dictionary<LevelSize, RoomType[,]> LevelGenerator = new Dictionary<LevelSize, RoomType[,]>() {
            { LevelSize.Small, new RoomType[4,4] },
            { LevelSize.Medium, new RoomType[6,6] },
            { LevelSize.Large, new RoomType[8,8] }
        };

    }

    public enum RoomType {
        Empty,
        Entrance,
        FountainRoom,
        Pit
    }

    public enum LevelSize {
        Small,
        Medium,
        Large
    }

    
}
