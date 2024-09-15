using Utilities;

namespace FountainOfObjects {
    internal class Program {
        static void Main(string[] args) {
            PrintMenuAndWait(ConsoleColor.Magenta);



        }

        static public void PrintMenuAndWait(ConsoleColor color) {
            Utils.PrintColoredText("Welcome to the Fountain of objects game", color);
            Utils.PrintColoredText("You will have to find and enable Fountain of objects", color);
            Utils.PrintColoredText("Available controls are:", color);
            Utils.PrintColoredText("move [up, down, left, right], [enable, disable] fountain]", color);
            Utils.PrintColoredText("Press any key to begin", color);
            Console.ReadKey();
        }
    }
}
