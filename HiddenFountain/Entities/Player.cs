using HiddenFountain.Models;

namespace HiddenFountain.Entities {
    internal class Player : Creature{
        public bool Won { get; private set; } = false;

        public Player(int row, int col) : base(row, col) {}

        public void UpdatePosition(int row, int col) {
            Position = new CavePoint(row, col);
        }

        public void MakeWin() {
            Won = true;
        }

        public string GetLocation() {
            return $"You are in the room at Row {Position.Row}, Column {Position.Col}";
        }

    }
}
