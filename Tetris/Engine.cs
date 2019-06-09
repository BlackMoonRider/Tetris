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
        System.Timers.Timer timer;
        ITetromino currentTetromino;
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
            
            canvas.Render(consoleGraphics);
            consoleGraphics.DrawString("TETЯIS", "Times New Roman", 0xFFFFFF00, 150, 80, 100);
            consoleGraphics.DrawString("PRESS ANY KEY TO START", "Consolas", 0xFFFFFF00, 230, 650, 20);
            consoleGraphics.DrawString("LOGO BY FREEPNG.RU   EDUCATIONAL PROJECT ©2019", "Consolas", 0xFFFFFF00, 225, 750, 10);
            ConsoleImage logo = consoleGraphics.LoadImage(@"logo.bmp");
            System.Threading.Thread.Sleep(100);
            consoleGraphics.DrawImage(logo, 255, 300);
            consoleGraphics.FlipPages();
        }

        public void DrawBlackCanvas()
        {
            canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFF000000);
            canvas.Render(consoleGraphics);
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

        public void SetLevel() // TODO
        {
            //grid.FillDataBoolWithRandomData(15);
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

        public void PlayOneFrame()
        {
            
        }

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void SetCurrentTetromino() // Turn to Factory
        {
            // Create shape
            currentTetromino = new TetrominoT();

            // Set current shape and it's position
            CurrentShape.Rotation = currentTetromino.GetCurrentRotation();
            CurrentShape.PositionLine = 0;
            CurrentShape.PositionColumn = 4;
            //CurrentShape.IsInTheAir = true;
        }

        public void PutTetrominoOnCurrentGrid()
        {
            currentGrid.FeedWithBoolData(grid);
            currentGrid.PutCurrentShapeOnBoolData();
            currentGrid.ConvertBoolDataToPixelData();
        }

        public void CheckKeyboardAgainstCanvas()
        {
            if (Input.IsKeyDown(Keys.LEFT))
            {
                CurrentShape.PositionColumn--;
                if (currentGrid.CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left)
                {
                    RestorePositionAndRotation();
                }
            }

            if (Input.IsKeyDown(Keys.RIGHT))
            {
                CurrentShape.PositionColumn++;
                if (currentGrid.CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right)
                {
                    RestorePositionAndRotation();
                }
            }

            if (Input.IsKeyDown(Keys.UP))
            {
                currentTetromino.SetNextRotation();
                CurrentShape.Rotation = currentTetromino.GetCurrentRotation();
                while (currentGrid.CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left)
                {
                    CurrentShape.PositionColumn++;
                }
                while (currentGrid.CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right)
                {
                    CurrentShape.PositionColumn--;
                }
            }
        }

        public void CheckFallingDownAgainstCanvas()
        {
            if (currentGrid.CheckCurrentShapeOutOfScreenUpBottom())
            {
                StopTimer();
                RestorePositionAndRotation();
            }
        }

        public void CheckCollisionAgainstBoolData()
        {
            if (currentGrid.CheckCurrentShapeCollision() && (Input.IsKeyDown(Keys.LEFT) || Input.IsKeyDown(Keys.RIGHT)))
            {
                RestorePositionAndRotation();
            }
            else if (currentGrid.CheckCurrentShapeCollision() && Input.IsKeyDown(Keys.UP))
            {
                //StopTimer();
                RestorePositionAndRotation();
                //CurrentShape.PositionLine--;
            }
            else if (currentGrid.CheckCurrentShapeCollision())
            {
                StopTimer();
                RestorePositionAndRotation();
                CurrentShape.PositionLine--;
            }

        }

        public void BackUpPositionAndRotation()
        {
            backupPositionLine = CurrentShape.PositionLine;
            backupPositionColumn = CurrentShape.PositionColumn;
            backupRotation = (bool[,])CurrentShape.Rotation.Clone();
        }

        public void RestorePositionAndRotation()
        {
            CurrentShape.PositionLine = backupPositionLine;
            CurrentShape.PositionColumn = backupPositionColumn;
            CurrentShape.Rotation = (bool[,])backupRotation.Clone();
            //backupRotation.CopyTo(CurrentShape.Rotation, 0);
        }

        public void FeedCurrentGridWithBoolData()
        {
            currentGrid.FeedWithBoolData(grid);
        }

        public void UpdateBoolAndPixelData()
        {
            currentGrid.PutCurrentShapeOnBoolData();
            currentGrid.DropFullLines(); // This line belongs to this position. You move it - you break everything.
            currentGrid.ConvertBoolDataToPixelData();
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
