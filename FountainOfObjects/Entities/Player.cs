namespace FountainOfObjects.Entities {
    internal class Player {
        private (int Row, int Col) location;
        public (int Row, int Col) Location { get => location; }
        public bool IsAlive { get; set; } = true;
        public bool Won { get; set; } = false;

        public Player(int row, int col) {
            location.Row = row;
            location.Col = col;
        }

        public void UpdatePosition(int row, int col) { 
            location.Row += row;
            location.Col += col;
        }

        public void SetDead() {
            IsAlive = false;
        }

        public override string ToString() {
            return $"You are in the room at Row {Location.Row}, Column {Location.Col}";
        }

    }
}
