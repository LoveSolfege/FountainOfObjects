using HiddenFountain.Models;

namespace HiddenFountain.GameLogic {
    internal static class GridManagar {

        public static List<CavePoint> GetNeighbors(int[,] grid, int itemRow, int itemCol, int radius) {

            List<CavePoint> neighbors = new();
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            // Loop through the square of size (2r + 1) centered at (itemRow, itemCol)
            for (int x = -radius; x <= radius; x++) {
                for (int y = -radius; y <= radius; y++) {
                    // Skip the central cell and check if Manhattan distance is within the radius
                    if (x == 0 && y == 0 || Math.Abs(x) + Math.Abs(y) > radius) {
                        continue;
                    }

                    // Calculate the new position
                    int newItemRow = itemRow + x;
                    int newItemCol = itemCol + y;

                    // Check if the new position is within grid bounds
                    if (newItemRow >= 0 
                        && newItemRow < rows 
                        && newItemCol >= 0 && newItemCol < cols) {

                        CavePoint neighbor = new CavePoint(newItemRow, newItemCol);
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }

    }
}
