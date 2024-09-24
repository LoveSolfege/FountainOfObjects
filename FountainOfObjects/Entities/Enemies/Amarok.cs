using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;

namespace HiddenFountain.Entities.Enemies {
    internal class Amarok : Creature, ISensible{
        public Amarok(int row, int col) : base(row, col) { }

        public string Sense() {
            return GameStrings.AmarokSense;
        }
    }
}
