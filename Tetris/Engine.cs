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
                System.Threading.Thread.Sleep(100);
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
            grid.FillDataBoolWithRandomData(Settings.Level);
        }

        public void RedrawScreen()
        {
            canvas = new Rectangle(Settings.canvasOffsetX + 0, Settings.canvasOffsetY + 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFF000000);
            screen = new Rectangle(Settings.canvasOffsetX + 0, Settings.canvasOffsetY + 0, Grid.columnSize * Settings.PixelSizeY, Grid.lineSize * Settings.PixelSizeX, 0xFF262626);
            canvas.Render(consoleGraphics);
            screen.Render(consoleGraphics);

            foreach (Pixel pixel in currentGrid.PixelData)
                pixel.Render(consoleGraphics);
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
            CurrentShape.Rotation = currentTetromino.GetCurrentRotation();
            CurrentShape.PositionLine = 0;
            CurrentShape.PositionColumn = 4;
            //CurrentShape.IsInTheAir = true;
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
            backupPositionLine = CurrentShape.PositionLine;
            backupPositionColumn = CurrentShape.PositionColumn;
            backupRotation = (bool[,])CurrentShape.Rotation.Clone();
        }
        
        public void MoveCurrentShapeDown() // Refactor this to be single-responsible
        {
            BackUpPositionAndRotation();

            CurrentShape.PositionLine++; //Сдвинуть вниз

            if (IsCurrentShapeBeyondCanvasBottom() || DoesCurrentShapeCollideWithData()) //Коллизия?
            {
                CurrentShape.CanMoveDown = false; //Флаг
                RestorePositionAndRotation(); //Восстановить состояние
            }
        }

        public void UpdateBoolAndPixelData()
        {
            currentGrid.PutCurrentShapeOnBoolData();
            currentGrid.DropFullLines(); // This line belongs to this position. You move it - you break everything.
            currentGrid.ConvertBoolDataToPixelData();
        }

        public void RestorePositionAndRotation()
        {
            CurrentShape.PositionLine = backupPositionLine;
            CurrentShape.PositionColumn = backupPositionColumn;
            CurrentShape.Rotation = (bool[,])backupRotation.Clone();
            //backupRotation.CopyTo(CurrentShape.Rotation, 0);
        }

        public void PutResultOnPermanentGrid()
        {
            currentGrid.DropFullLines();
            grid.FeedWithBoolData(currentGrid);
        }

        public bool DoesCurrentShapeCollideWithData()
        {
            bool doesCollide = false;

            for (int line = 0; line < CurrentShape.Rotation.GetLength(0); line++)
            {
                for (int column = 0; column < CurrentShape.Rotation.GetLength(1); column++)
                {
                    if (CurrentShape.Rotation[line, column] && 
                        currentGrid.BoolData[line + CurrentShape.PositionLine, column + CurrentShape.PositionColumn])
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

            if (CurrentShape.PositionLine + CurrentShape.Rotation.GetLength(0) > Settings.LineNumber)
                isBeyond = true;

            return isBeyond;
        }


        public void CheckKeyboardInputAgainstCanvasAndData()
        {
            BackUpPositionAndRotation(); //Запомнить состояние

            if (Input.IsKeyDown(Keys.LEFT))
            {
                CurrentShape.PositionColumn--;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
            }

            else if (Input.IsKeyDown(Keys.RIGHT))
            {
                CurrentShape.PositionColumn++;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
            }

            else if (Input.IsKeyDown(Keys.UP))
            {
                currentTetromino.SetNextRotation();
                CurrentShape.Rotation = currentTetromino.GetCurrentRotation();
                if (DoesCurrentShapeCollideWithData() || IsCurrentShapeBeyondCanvasBottom())
                {
                    currentTetromino.SetPreviousRotation();
                    RestorePositionAndRotation();
                }
                else
                {
                    while (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left)
                    {
                        CurrentShape.PositionColumn++;
                    }
                    while (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right)
                    {
                        CurrentShape.PositionColumn--;
                    }
                }
            }
        }

        // Returns true if the current shape is out of the left or right side of the screen 
        private OutOfScreenProperties CheckCurrentShapeOutOfScreenLeftRight()
        {
            if (CurrentShape.PositionColumn + CurrentShape.Rotation.GetLength(1) > Settings.ColumnNumber)
                return OutOfScreenProperties.Right;
            if (CurrentShape.PositionColumn < 0)
                return OutOfScreenProperties.Left;

            return OutOfScreenProperties.None;
        }

    }
}
