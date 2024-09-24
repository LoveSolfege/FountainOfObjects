using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;
using HiddenFountain.Settings;
using HiddenFountain.Utilities;

namespace HiddenFountain.Entities.Rooms {
    internal class EntranceRoom : Room, ITextOnEntering{
        public void EnteringText() {
            Utils.PrintColoredText(GameStrings.EnteringEntrance, ColorSettings.EnteringColor);
        }
    }
}
