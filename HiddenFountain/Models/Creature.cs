namespace HiddenFountain.Models {
    internal abstract class Creature {
        public Point Position { get; protected set; }
        public bool IsAlive { get; protected set; } = true;

        public Creature(int row, int col) {
            Position = new Point(row, col);
        }

        public virtual void Die() {
            IsAlive = false;
        }
    }
}
