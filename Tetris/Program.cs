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

            engine.DrawGameMenu();
            engine.DrawBlackCanvas();
            engine.SetLevel();

            while (true)
            {
                engine.SetCurrentTetromino();

                AbstractShape.CurrentTetrominoCanMoveDown = true;

                if (engine.DoesCurrentShapeCollideWithData())
                {
                    if (engine.DrawGameOverScreen())
                    {
                        engine.DrawGameMenu();
                        engine.DrawBlackCanvas();
                        engine.SetLevel();
                        continue;
                    }
                }

                while (AbstractShape.CurrentTetrominoCanMoveDown)
                {

                    engine.CopyGridToCurrentGrid();
                    engine.CheckKeyboardInputAgainstCanvasAndData();
                    engine.MoveCurrentShapeDown();
                    engine.PutCurrentTetrominoOnCurrentGrid();
                    engine.RedrawInGameScreen();
                }

                engine.PutResultOnPermanentGrid();

            }
        }
    }
}
