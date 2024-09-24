using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;

namespace HiddenFountain.Entities.Enemies {
    internal class Maelstrom : Creature, ISensible {
        public Maelstrom(int row, int col) : base(row, col) { }

        public string Sense() {
            return GameStrings.MaelstromSense;
        }
    }
}
