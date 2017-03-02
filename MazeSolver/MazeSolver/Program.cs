using MazeSolver.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class Program
    {
        private static GameHelper gameHelper { get; set; }

        static void Main(string[] args)
        {
            string input = "";
            while (input != "quit" && input != "exit" && input != "return")
            {
                Console.Clear();
                gameHelper = new GameHelper();

            gameHelper = new GameHelper();

            Difficulty difficulty = chooseDifficulty();
            bool mode = isManualMode();
            gameHelper.initNewGame(difficulty, "Player1");
            Console.WriteLine("\n/* Partie " + difficulty + " commencée en " + (mode ? "manuel" : "automatique") + "*\\\n");

                displayMap(gameHelper.getVisiblesCells());
                System.Threading.Thread.Sleep(500);


            if (mode)
            {
                runManualMode();
            }
            else
            {
                runAutoMode();
            }

                gameHelper.closeGame();
                Console.Clear();
                Console.WriteLine("Tapez exit pour quitter le jeu, ou autre chose pour commencer une nouvelle partie.");
                input = Console.ReadLine();
            }
        }

        private static bool displayMap(string[][] map)
        {
            Console.Clear();
            bool isFinished = false;
            foreach (string[] x in map)
            {
                string tmpString = "";
                foreach (string xy in x)
                {
                    if (xy == "?")
                    {
                        isFinished = true;
                    }
                    tmpString += " " + xy;
                }
                Console.WriteLine("Nouvelle ligne : " + tmpString);
            }
            return isFinished;
        }

        private static Difficulty chooseDifficulty()
        {
            Console.WriteLine("Difficulté ?");
            Console.WriteLine(" 1 : Easy");
            Console.WriteLine(" 2 : Medium");
            Console.WriteLine(" 3 : Hard");
            Console.WriteLine(" 4 : Extreme");
            string input;
            do
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return Difficulty.Easy;
                    case "2":
                        return Difficulty.Medium;
                    case "3":
                        return Difficulty.Hard;
                    case "4":
                        return Difficulty.Extreme;
                }
            } while (input != "1" &&
                input != "2" &&
                input != "3" &&
                input != "4");
            return Difficulty.Extreme;
        }

        private static bool isManualMode()
        {
            Console.WriteLine("Mode manuel ?");
            Console.WriteLine(" 1 : Manuel");
            Console.WriteLine(" 2 : Auto");
            string input;
            do
            {
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        return true;
                    case "2":
                        return false;
                }
            } while (input != "1" && input != "2");
            return true;
        }


        private static void runManualMode()
        {
            string input = Console.ReadLine();
            while (input != "quit" && input != "exit" && input != "return")
            {
                bool isMoved=false;
                switch (input.LastOrDefault())
                {
                    case 'd':
                        isMoved = gameHelper.move(Direction.Right);
                        break;
                    case 'q':
                        isMoved = gameHelper.move(Direction.Left);
                        break;
                    case 'z':
                        isMoved = gameHelper.move(Direction.Up);
                        break;
                    case 's':
                        isMoved = gameHelper.move(Direction.Down);
                        break;
                    default:
                        Console.WriteLine("Veuillez entrer un input valide");
                        break;
                }
                if (isMoved)
                {
                    if (displayMap(gameHelper.getVisiblesCells()))
                    {
                        Console.WriteLine("Fin du labyrinthe trouvée : ");
                        Console.WriteLine(gameHelper.getEndTag());
                    }
                }

                System.Threading.Thread.Sleep(500);
                input = Console.ReadLine();
            }

        }

        private static void runAutoMode()
        {
            Bot bot = new Bot(gameHelper);
            bool endIsNear = false;
            do
            {
                Direction tmpDirection = bot.chooseDirection();
                displayMap(gameHelper.getVisiblesCells());
                autoStraightMove(tmpDirection, bot);

                System.Threading.Thread.Sleep(500);
            } while (!endIsNear);
        }
        //TODO 
        private static void autoStraightMove(Direction direction, Bot bot)
        {
            
            bool isWallInFront = gameHelper.move(direction);
            bool isACarrefour = false;
            while (!isWallInFront && !isACarrefour)
            {

                System.Threading.Thread.Sleep(500);
                isWallInFront = gameHelper.move(direction);
                isACarrefour = gameHelper.getNumberOfWay() > 2;
                Console.WriteLine("\nBefore display map " + gameHelper.getNumberOfWay());
                if (displayMap(gameHelper.getVisiblesCells()))
                {
                    Console.WriteLine("\nFin du labyrinthe trouvée : ");
                    Console.WriteLine(gameHelper.getEndTag());

                }
                Console.ReadLine();
            }
        }
    }
}