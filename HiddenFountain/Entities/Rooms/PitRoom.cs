using HiddenFountain.Constants;
using HiddenFountain.Interfaces;
using HiddenFountain.Models;

namespace HiddenFountain.Entities.Rooms {
    internal class PitRoom : Room, ISensible{
        public string Sense() {
            return GameStrings.PitSense;
        }
    }
}
