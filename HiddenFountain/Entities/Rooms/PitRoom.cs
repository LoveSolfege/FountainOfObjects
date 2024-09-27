using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;
using HiddenFountain.Settings;
using HiddenFountain.Utilities;

namespace HiddenFountain.Entities.Rooms {
    internal class PitRoom : Room, ISensible{
        public void Sense(ConsoleColor color) {
            Utils.PrintColoredText(GameStrings.PitSense, color);
        }
    }
}
