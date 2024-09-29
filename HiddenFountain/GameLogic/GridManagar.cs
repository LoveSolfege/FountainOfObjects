using HiddenFountain.Models;

namespace HiddenFountain.GameLogic {
    internal static class GridManagar {

        public static List<Point> GetManhattanNeighbors<T>(T[,] grid, Point location, int radius) {
            List<Point> neighbors = new();
            // Loop through the square of size (2r + 1) centered at (itemRow, itemCol)
            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    // Skip the central cell and check if Manhattan distance is within the radius
                    if (x == 0 && y == 0 || Math.Abs(x) + Math.Abs(y) > radius) {
                        continue;
                    }

                    int newItemRow = location.Row + x;
                    int newItemCol = location.Col + y;

                    if (IsValidRoom(grid,newItemRow, newItemCol )){

                        Point neighbor = new Point(newItemRow, newItemCol);
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

        public static List<Point> GetAdjacentNeighbors<T>(T[,] grid, Point location, int distance) {
            List<Point> neighbors = new();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int d = 1; d <= distance; d++) {
                // Left
                if (location.Col - d >= 0) {
                    neighbors.Add(new Point(location.Row, location.Col - d));
                }
                // Right
                if (location.Col + d < cols) {
                    neighbors.Add(new Point(location.Row, location.Col + d));
                }
                // Top
                if (location.Row - d >= 0) {
                    neighbors.Add(new Point(location.Row - d, location.Col));
                }
                // Bottom
                if (location.Row + d < rows) {
                    neighbors.Add(new Point(location.Row + d, location.Col));
                }
            }

            return neighbors;
        }

        public static bool IsValidRoom<T>(T[,] grid, int row, int col) {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            if (row >= 0 && row < rows && col >= 0 && col < cols) {
                return true;
            }

            return false;

        }

    }

    
}
