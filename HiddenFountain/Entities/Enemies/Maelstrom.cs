using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;
using HiddenFountain.Settings;
using HiddenFountain.Utilities;

namespace HiddenFountain.Entities.Enemies {
    internal class Maelstrom : Creature, ISensible {
        public Maelstrom(int row, int col) : base(row, col) { }

        public void Sense() {
            Utils.PrintColoredText(GameStrings.AmarokSense, ColorSettings.SenseColor);
        }
    }
}
