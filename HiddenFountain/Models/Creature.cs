namespace HiddenFountain.Models {
    internal abstract class Creature {
        public CavePoint Position { get; protected set; }
        public bool IsAlive { get; protected set; } = true;

        public Creature(int row, int col) {
            Position = new CavePoint(row, col);
        }

        public virtual void Die() {
            IsAlive = false;
        }
    }
}
