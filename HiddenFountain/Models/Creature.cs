namespace HiddenFountain.Models {
    internal abstract class Creature {
        public int PositionRow { get; protected set; }
        public int PositionCol { get; protected set; }
        public (int Row, int Col) Position {
            get => (PositionRow, PositionCol); 
        }
        public bool IsAlive { get; protected set; } = true;

        public Creature(int row, int col) {
            PositionRow = row;
            PositionCol = col;
        }

        public virtual void Die() {
            IsAlive = false;
        }
    }
}
