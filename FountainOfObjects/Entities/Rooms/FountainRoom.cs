using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;
using HiddenFountain.Utilities;

namespace HiddenFountain.Entities.Rooms {
    internal class FountainRoom : Room, ISensible, ITextOnEntering {
        public bool Enabled { get; private set; } = false;

        public void Toggle() {
            Enabled = !Enabled;
            NotifyToggle();
        }

        public void NotifyToggle() {
            Utils.PrintColoredText(
                Enabled ? GameStrings.FountainEnabled : GameStrings.FountainDisabled,
                Enabled ? ConsoleColor.DarkCyan : ConsoleColor.Blue);
        }

        public string Sense() {
            return GameStrings.FountainSense;
        }

        public string EnteringText() {
            return GameStrings.EnteringFountain;
        }
    }
}
