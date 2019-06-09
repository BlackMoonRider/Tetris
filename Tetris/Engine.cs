﻿using System;
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
        System.Timers.Timer timer;
        Tetromino currentTetromino;
        int backupPositionLine;
        int backupPositionColumn;
        bool[,] backupRotation;

        public Engine(ConsoleGraphics consoleGraphics)
        {
            this.consoleGraphics = consoleGraphics;
            this.canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFFFF0000);
            this.grid = new Grid();
            this.currentGrid = new Grid();
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

        public void DrawBlackCanvas()
        {
            canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFF000000);
            canvas.Render(consoleGraphics);
        }

        public void SetLevel() // TODO
        {
            grid.FillDataBoolWithRandomData(Settings.Level);
        }

        public void SetTimer()
        {
            timer = new System.Timers.Timer(400);
            timer.Elapsed += (_, ___) => CurrentShape.PositionLine++;
            //timer.Elapsed += (_, ___) =>
            //{
            //    BackUpPositionAndRotation();
            //    CurrentShape.PositionLine++;
            //    currentGrid.FeedWithBoolData(grid);
            //    if (currentGrid.CheckCurrentShapeCollision())
            //    {
            //        StopTimer();
            //        RestorePositionAndRotation();
            //    }
            //};
            timer.AutoReset = true;
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

        public void PutTetrominoOnCurrentGrid()
        {
            currentGrid.FeedWithBoolData(grid); // Copy all the data from the permanent grid to the current grid
            currentGrid.PutCurrentShapeOnBoolData(); // Put a shape on the current grid
            currentGrid.ConvertBoolDataToPixelData(); // Create an image to render
        }

        public void StartTimer()
        {
            timer.Start();
        }

        public void BackUpPositionAndRotation()
        {
            backupPositionLine = CurrentShape.PositionLine;
            backupPositionColumn = CurrentShape.PositionColumn;
            backupRotation = (bool[,])CurrentShape.Rotation.Clone();
        }

        public void CheckKeyboardAgainstCanvas()
        {
            if (Input.IsKeyDown(Keys.LEFT))
            {
                CurrentShape.PositionColumn--;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left)
                {
                    RestorePositionAndRotation();
                }
            }

            if (Input.IsKeyDown(Keys.RIGHT))
            {
                CurrentShape.PositionColumn++;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right)
                {
                    RestorePositionAndRotation();
                }
            }

            if (Input.IsKeyDown(Keys.UP))
            {
                currentTetromino.SetNextRotation();
                CurrentShape.Rotation = currentTetromino.GetCurrentRotation();
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

        // Returns true if the current shape is out of the left or right side of the screen 
        private OutOfScreenProperties CheckCurrentShapeOutOfScreenLeftRight()
        {
            if (CurrentShape.PositionColumn + CurrentShape.Rotation.GetLength(1) > Settings.ColumnNumber)
                return OutOfScreenProperties.Right;
            if (CurrentShape.PositionColumn < 0)
                return OutOfScreenProperties.Left;

            return OutOfScreenProperties.None;
        }

        public void CheckFallingDownAgainstCanvas()
        {
            if (CheckCurrentShapeOutOfScreenUpBottom())
            {
                StopTimer();
                RestorePositionAndRotation();
            }
        }

        // Returns true if the current shape is out of the up or bottom of the screen 
        private bool CheckCurrentShapeOutOfScreenUpBottom()
        {
            if (CurrentShape.PositionLine + CurrentShape.Rotation.GetLength(0) + 1 > Settings.LineNumber ||
                CurrentShape.PositionLine < 0)
                return true;

            return false;
        }

        public void FeedCurrentGridWithBoolData()
        {
            currentGrid.FeedWithBoolData(grid);
        }

        public void CheckCollisionAgainstBoolData()
        {
            if (CheckCurrentShapeCollision(currentGrid) && (Input.IsKeyDown(Keys.LEFT) || Input.IsKeyDown(Keys.RIGHT)))
            {
                RestorePositionAndRotation();
            }
            else if (CheckCurrentShapeCollision(currentGrid) && Input.IsKeyDown(Keys.UP))
            {
                //StopTimer();
                RestorePositionAndRotation();
                //CurrentShape.PositionLine--;
            }
            else if (CheckCurrentShapeCollision(currentGrid))
            {
                StopTimer();
                RestorePositionAndRotation();
                CurrentShape.PositionLine--;
            }

        }

        // Returns true if the current shape collides with the bool data
        private bool CheckCurrentShapeCollision(Grid currentGrid)
        {
            bool result = false;

            for (int line = 0; line < CurrentShape.Rotation.GetLength(0); line++)
                for (int column = 0; column < CurrentShape.Rotation.GetLength(1); column++)
                {
                    if (CurrentShape.Rotation[line, column] && currentGrid.BoolData[line + CurrentShape.PositionLine, column + CurrentShape.PositionColumn])
                        result = true;
                }

            return result;
        }

        public void UpdateBoolAndPixelData()
        {
            currentGrid.PutCurrentShapeOnBoolData();
            currentGrid.DropFullLines(); // This line belongs to this position. You move it - you break everything.
            currentGrid.ConvertBoolDataToPixelData();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void RestorePositionAndRotation()
        {
            CurrentShape.PositionLine = backupPositionLine;
            CurrentShape.PositionColumn = backupPositionColumn;
            CurrentShape.Rotation = (bool[,])backupRotation.Clone();
            //backupRotation.CopyTo(CurrentShape.Rotation, 0);
        }

        public bool UpdateGrid()
        {
            bool gridHasBeenUpdated = false; 

            if (timer.Enabled == false)
            {
                grid.FeedWithBoolData(currentGrid);
                gridHasBeenUpdated = true;
            }
            return gridHasBeenUpdated;

        }

    }
}
