using HiddenFountain.Models;

namespace HiddenFountain.GameLogic {
    internal static class GridManagar {

        public static List<Point> GetManhattanNeighbors<T>(T[,] grid, Point location, int radius) {
            List<Point> neighbors = new();
            int rows = grid.GetLength(0);
            // Loop through the square of size (2r + 1) centered at (itemRow, itemCol)
            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    // Skip the central cell and check if Manhattan distance is within the radius
                    if (x == 0 && y == 0 || Math.Abs(x) + Math.Abs(y) > radius) {
                        continue;
                    }

                    int newItemRow = location.Row + x;
                    int newItemCol = location.Col + y;

                    if (IsValidRoom(newItemRow, newItemCol, rows)) {

                        Point neighbor = new Point(newItemRow, newItemCol);
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        public static bool IsValidRoom(int row, int col, int sizeX, int sizeY = 0) {
            int rows = sizeX;
            int cols = sizeY;

            if (sizeY == 0)
                cols = sizeX;

            if (row >= 0 && row < rows && col >= 0 && col < cols) {
                return true;
            }

            return false;

        }

    }

    
}
