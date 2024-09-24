using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;

namespace HiddenFountain.Entities.Rooms {
    internal class EntranceRoom : Room, ITextOnEntering {
        public string EnteringText() {
            return GameStrings.EnteringEntrance;
        }
    }
}
