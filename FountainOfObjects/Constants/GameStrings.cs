using System.Text;

namespace HiddenFountain.Constants {
    internal static class GameStrings {
        //for main program
        public static readonly string AskToSelectDifficulty = "In order to start the game, please select game difficulty";
        public static readonly string ListDifficultyOptions = "[1] Easy, [2] Medium, [3] Hard";
        public static readonly string AskForOption = "Option: ";
        public static readonly string InvalidSelection = "please select something between 1-2-3";

        //Fountain activation/deactivation
        public static readonly string FountainEnabled = "The Hidden Fountain was activated";
        public static readonly string FountainDisabled = "The Hidden Fountain was deactivated";

        //text for sensing something
        public static readonly string AmarokSense = "You can smell the rotten stench of an amarok in a nearby room.";
        public static readonly string MaelstromSense = "You hear the growling and groaning of a maelstrom nearby.";
        public static readonly string FountainSense = "You hear water dripping nearby, The Hidden Fountain is close!";
        public static readonly string PitSense = "You feel a draft. There is a pit in a nearby room.";

        //text for entering something
        public static readonly string EnteringFountain = "You hear water dripping in this room, The Hidden Fountain is here!";
        public static readonly string EnteringEntrance = "You see light coming from the cavern entrance";

        //starting menu text
        public static readonly string WholeMenu = new StringBuilder()
            .AppendLine("Welcome to The Hidden Fountain!")
            .AppendLine("You will have to find and enable The Hidden Fountain")
            .AppendLine("And return back to the entrance")
            .AppendLine("On your way you will be hunted by different creatures")
            .AppendLine("And also natural obstacles suchs as pits.")
            .AppendLine("Available controls are:")
            .AppendLine("move [up, down, left, right], [enable, disable] fountain")
            .AppendLine("Write [help] to display help menu in game")
            .AppendLine("Press any key to begin")
            .ToString();

    }
}
