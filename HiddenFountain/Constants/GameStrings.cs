using System.Text;

namespace HiddenFountain.Constants {
    internal static class GameStrings {
        //text for game start 
        public static readonly string AskToSelectDifficulty = "In order to start the game, please select game difficulty";
        public static readonly string ListDifficultyOptions = "[1] Easy, [2] Medium, [3] Hard";
        public static readonly string AskForOption = "Option: ";
        public static readonly string InvalidDifficultySelection = "please select something between 1-2-3";

        //text for ingame choices
        public static readonly string AskWhatToDo = "What to do now? ";
        public static readonly string BadMoveChoice = "You don't seem to be able to do that";

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

        //causes of death
        public static readonly string DiedInPit = "You fell into the pit and died.";
        public static readonly string KilledByAmarok = "You were eaten by an Amarok";

        //player win
        public static readonly string Win = "Congratulations! You Won!";

        //player hit wall
        public static readonly string HitWall = "You hit the wall";

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

        //help menu text
        public static readonly string HelpText = new StringBuilder()
            .AppendLine("help test")
            .ToString();

        //json load error
        public static readonly string JsonLoadError = "Game settings file (appsettings.json) was renamed, deleted or not created yet,\nPress any key to create it.";

        //json created successfully
        public static readonly string JsonCreated = "Settings file created successfully.";
    }
}
