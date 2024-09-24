using HiddenFountain.Settings;
using HiddenFountain.Utilities;

namespace HiddenFountain.Interfaces {
    internal interface ITextOnEntering {
        void EnteringText(string text) {
            Utils.PrintColoredText(text, ColorSettings.EnteringColor);
        }
    }
}
