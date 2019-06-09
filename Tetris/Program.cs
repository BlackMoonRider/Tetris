using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;
using System.Threading;

namespace Tetris
{
    class Program
    {
        static public Random random = new Random();
        static void Main(string[] args)
        {
            Console.WindowWidth = Settings.ConsoleWidth;
            Console.WindowHeight = Settings.ConsoleHeight;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
            Console.Clear();

            ConsoleGraphics consoleGraphics = new ConsoleGraphics();
            Engine engine = new Engine(consoleGraphics);

            engine.DrawTitileScreen();

            Console.ReadKey();

            engine.DrawBlackCanvas();
            engine.SetLevel();
            engine.SetTimer();

            while (true)
            {
                engine.SetCurrentTetromino();
                engine.PutTetrominoOnCurrentGrid();
                engine.StartTimer();

                while (true)
                {
                    engine.BackUpPositionAndRotation();

                    engine.CheckKeyboardAgainstCanvas();
                    engine.CheckFallingDownAgainstCanvas();

                    engine.FeedCurrentGridWithBoolData();
                    engine.CheckCollisionAgainstBoolData();
                    engine.UpdateBoolAndPixelData();
                    engine.RedrawScreen();
                    if (engine.UpdateGrid())
                        break;
                }

            }


        }
    }
}
