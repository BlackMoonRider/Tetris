using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;
using System.Threading;

namespace Tetris
{
    class Engine
    {
        private readonly ConsoleGraphics consoleGraphics;
        Rectangle canvas;
        Rectangle screen;
        Grid grid;
        Grid currentGrid;
        Tetromino currentTetromino;
        int backupPositionLine;
        int backupPositionColumn;
        bool[,] backupRotation;

        public Engine(ConsoleGraphics consoleGraphics)
        {
            this.consoleGraphics = consoleGraphics;
            this.canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFFFF0000);
            GameReset();
        }

        private void GameReset()
        {
            grid = new Grid();
            currentGrid = new Grid();
            ResetCurrentScore();
        }

        public void DrawTitileScreen()
        {
            while (true)
            {
                canvas.Render(consoleGraphics);
                consoleGraphics.DrawString("TETЯIS", "Times New Roman", 0xFFFFFF00, 150, 80, 100);
                consoleGraphics.DrawString(" PRESS SPACE TO START", "Consolas", 0xFFFFFF00, 230, 650, 20);
                consoleGraphics.DrawString("LOGO BY FREEPNG.RU   EDUCATIONAL PROJECT ©2019", "Consolas", 0xFFFFFF00, 225, 750, 10);
                ConsoleImage logo = consoleGraphics.LoadImage(@"logo.bmp");
                consoleGraphics.DrawImage(logo, 255, 300);
                consoleGraphics.FlipPages();

                if (Input.IsKeyDown(Keys.SPACE))
                    break;
            }
        }

        public bool DrawGameOverScreen()
        {
            bool restartGame = false;

            while (!restartGame)
            {
                canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0x26262626);
                canvas.Render(consoleGraphics);
                consoleGraphics.DrawString("GAME\r\nOVER", "Consolas", 0xFFFFFF00, 230, 80, 100);
                consoleGraphics.DrawString("PRESS SPACE TO RESTART", "Consolas", 0xFFFFFF00, 230, 650, 20);
                consoleGraphics.DrawString("PRESS ESC TO EXIT GAME", "Consolas", 0xFFFFFF00, 230, 680, 20);
                consoleGraphics.FlipPages();

                if (Input.IsKeyDown(Keys.ESCAPE))
                    Environment.Exit(0);
                if (Input.IsKeyDown(Keys.SPACE))
                {
                    restartGame = true;
                    GameReset();
                    DrawBlackCanvas();
                }
            }

            return restartGame;
        }

        public void DrawBlackCanvas()
        {
            canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFF000000);
            canvas.Render(consoleGraphics);
        }

        public void SetLevel() // TODO
        {
            grid.FillBoolDataWithRandomData(Settings.Level);
        }

        public void RedrawScreen()
        {
            canvas = new Rectangle(Settings.canvasOffsetX + 0, Settings.canvasOffsetY + 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFF000000);
            screen = new Rectangle(Settings.canvasOffsetX + 0, Settings.canvasOffsetY + 0, Settings.ColumnNumber * Settings.PixelSizeY, Settings.LineNumber * Settings.PixelSizeX, 0xFF262626);
            canvas.Render(consoleGraphics);
            screen.Render(consoleGraphics);

            foreach (Pixel pixel in currentGrid.PixelData)
                pixel.Render(consoleGraphics);

            consoleGraphics.DrawString($"   SCORE: {Settings.CurrentScore}", "Consolas", 0xFFFFFF00, 230, 680, 20);
            consoleGraphics.DrawString($"HI-SCORE: {Settings.HiScore}", "Consolas", 0xFFFFFF00, 230, 710, 20);

            consoleGraphics.FlipPages();
            System.Threading.Thread.Sleep(100);
        }   

        public void SetCurrentTetromino()
        {
            // Create shape
            int nextShape = Utility.Random.Next(7);

            switch (nextShape)
            {
                case 1:
                    currentTetromino = new TetrominoT();
                    break;
                case 2:
                    currentTetromino = new TetrominoI();
                    break;
                case 3:
                    currentTetromino = new TetrominoJ();
                    break;
                case 4:
                    currentTetromino = new TetrominoL();
                    break;
                case 5:
                    currentTetromino = new TetrominoS();
                    break;
                case 6:
                    currentTetromino = new TetrominoZ();
                    break;
                default:
                    currentTetromino = new TetrominoO();
                    break;
            }

            // Set current shape and it's position
            Tetromino.CurrentTetrominoRotation = currentTetromino.GetCurrentRotation();
            Tetromino.CurrentTetrominoPositionLine = 0;
            Tetromino.CurrentTetrominoPositionColumn = 4;
        }

        public void PutCurrentTetrominoOnCurrentGrid()
        {
            CopyGridToCurrentGrid(); // Copy all the data from the permanent grid to the current grid
            currentGrid.PutCurrentShapeOnBoolData(); // Put a shape on the current grid
            currentGrid.ConvertBoolDataToPixelData(); // Create an image to render
        }

        public void CopyGridToCurrentGrid()
        {
            currentGrid.FeedWithBoolData(grid);
        }

        public void BackUpPositionAndRotation()
        {
            backupPositionLine = Tetromino.CurrentTetrominoPositionLine;
            backupPositionColumn = Tetromino.CurrentTetrominoPositionColumn;
            backupRotation = (bool[,])Tetromino.CurrentTetrominoRotation.Clone();
        }
        
        public void MoveCurrentShapeDown() // Refactor this to be single-responsible
        {
            BackUpPositionAndRotation();

            Tetromino.CurrentTetrominoPositionLine++; //Сдвинуть вниз

            if (IsCurrentShapeBeyondCanvasBottom() || DoesCurrentShapeCollideWithData()) //Коллизия?
            {
                Tetromino.CurrentTetrominoCanMoveDown = false; //Флаг
                RestorePositionAndRotation(); //Восстановить состояние
            }
        }

        public void RestorePositionAndRotation()
        {
            Tetromino.CurrentTetrominoPositionLine = backupPositionLine;
            Tetromino.CurrentTetrominoPositionColumn = backupPositionColumn;
            Tetromino.CurrentTetrominoRotation = (bool[,])backupRotation.Clone();
        }

        public void PutResultOnPermanentGrid()
        {
            currentGrid.DropFullLines();
            grid.FeedWithBoolData(currentGrid);
        }

        public bool DoesCurrentShapeCollideWithData()
        {
            bool doesCollide = false;

            for (int line = 0; line < Tetromino.CurrentTetrominoRotation.GetLength(0); line++)
            {
                for (int column = 0; column < Tetromino.CurrentTetrominoRotation.GetLength(1); column++)
                {
                    if (Tetromino.CurrentTetrominoRotation[line, column] && 
                        currentGrid.BoolData[line + Tetromino.CurrentTetrominoPositionLine, column + Tetromino.CurrentTetrominoPositionColumn])
                    {
                        doesCollide = true;
                        break;
                    }

                }
            }

            return doesCollide;
        }

        private bool IsCurrentShapeBeyondCanvasBottom()
        {
            bool isBeyond = false;

            if (Tetromino.CurrentTetrominoPositionLine + Tetromino.CurrentTetrominoRotation.GetLength(0) > Settings.LineNumber)
                isBeyond = true;

            return isBeyond;
        }

        public void CheckKeyboardInputAgainstCanvasAndData()
        {
            BackUpPositionAndRotation(); //Запомнить состояние

            if (Input.IsKeyDown(Keys.LEFT))
            {
                Tetromino.CurrentTetrominoPositionColumn--;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
            }

            else if (Input.IsKeyDown(Keys.RIGHT))
            {
                Tetromino.CurrentTetrominoPositionColumn++;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
            }

            else if (Input.IsKeyDown(Keys.UP))
            {
                currentTetromino.SetNextRotation();
                Tetromino.CurrentTetrominoRotation = currentTetromino.GetCurrentRotation();
                if (IsCurrentShapeBeyondCanvasBottom())
                {
                    currentTetromino.SetPreviousRotation();
                    RestorePositionAndRotation();
                }
                else
                {
                    while (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left)
                    {
                        Tetromino.CurrentTetrominoPositionColumn++;
                    }
                    while (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right)
                    {
                        Tetromino.CurrentTetrominoPositionColumn--;
                    }
                    if (DoesCurrentShapeCollideWithData())
                    {
                        currentTetromino.SetPreviousRotation();
                        RestorePositionAndRotation();
                    }
                }
            }
        }

        // Returns true if the current shape is out of the left or right side of the screen 
        private OutOfScreenProperties CheckCurrentShapeOutOfScreenLeftRight()
        {
            if (Tetromino.CurrentTetrominoPositionColumn + Tetromino.CurrentTetrominoRotation.GetLength(1) > Settings.ColumnNumber)
                return OutOfScreenProperties.Right;
            if (Tetromino.CurrentTetrominoPositionColumn < 0)
                return OutOfScreenProperties.Left;

            return OutOfScreenProperties.None;
        }

        private void ResetCurrentScore()
        {
            Settings.CurrentScore = 0;
        }
    }
}
