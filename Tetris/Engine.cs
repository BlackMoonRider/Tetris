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
        private readonly ConsoleGraphics consoleGraphics; // COLORS TO ENUM!
        Rectangle canvas;
        Rectangle screen;
        Grid grid;
        Grid currentGrid;
        Tetromino currentTetromino;
        int backupPositionLine;
        int backupPositionColumn;
        bool[,] backupRotation;
        int speedTracking;
        bool keypressIsAllowed;

        public Engine(ConsoleGraphics consoleGraphics)
        {
            this.consoleGraphics = consoleGraphics;
            this.canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFFFF0000);
            GameReset();
            SetTimer();
        }

        private void GameReset()
        {
            grid = new Grid();
            currentGrid = new Grid();
            ResetCurrentScore();
        }

        private void SetTimer()
        {
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (_, __) => keypressIsAllowed = true;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void DisableKeypress()
        {
            keypressIsAllowed = false;
        }

        public void DrawTitileScreen()
        {
            bool proceed = false;

            while (!proceed)
            {
                canvas.Render(consoleGraphics);
                consoleGraphics.DrawString("TETЯIS", "Times New Roman", 0xFFFFFF00, 150, 80, 100);
                consoleGraphics.DrawString(" PRESS SPACE TO START", "Consolas", 0xFFFFFF00, 230, 650, 20);
                consoleGraphics.DrawString("LOGO BY FREEPNG.RU   EDUCATIONAL PROJECT ©2019", "Consolas", 0xFFFFFF00, 225, 750, 10);
                ConsoleImage logo = consoleGraphics.LoadImage(@"logo.bmp");
                consoleGraphics.DrawImage(logo, 255, 300);
                consoleGraphics.FlipPages();

                if (Input.IsKeyDown(Keys.SPACE))
                    proceed = true;
            }

            System.Threading.Thread.Sleep(100);
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

            System.Threading.Thread.Sleep(100);
            return restartGame;
        }

        public void DrawBlackCanvas()
        {
            canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0xFF000000);
            canvas.Render(consoleGraphics);
        }

        public void DrawGameMenu()
        {
            bool proceed = false;

            int cursorStart = 250;
            int cursorOffset = 100;

            int cursorPosition = cursorStart;

            while (!proceed)
            {
                canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, 0x26262626);
                canvas.Render(consoleGraphics);
                consoleGraphics.DrawString("GAME SETTINGS", "Consolas", 0xFFFFFF00, 150, 70, 50);
                consoleGraphics.DrawString($"LEVEL: {Settings.LevelSelector}", "Consolas", 0xFFFFFF00, 250, cursorStart, 30);
                consoleGraphics.DrawString($"SPEED: {Settings.SpeedSelector}", "Consolas", 0xFFFFFF00, 250, cursorStart + cursorOffset, 30);
                consoleGraphics.DrawString($"SHAPES: {Settings.ShapeSetName}", "Consolas", 0xFFFFFF00, 250, cursorStart + cursorOffset * 2, 30);
                PlaceCursor();
                consoleGraphics.DrawString("PRESS UP OR DOWN TO NAVIGATE", "Consolas", 0xFFFFFF00, 150, 620, 20);
                consoleGraphics.DrawString("PRESS ENTER TO CHANGE SETTINGS", "Consolas", 0xFFFFFF00, 150, 650, 20);
                consoleGraphics.DrawString("PRESS SPACE TO START GAME", "Consolas", 0xFFFFFF00, 150, 680, 20);
                consoleGraphics.DrawString("PRESS ESC TO EXIT GAME", "Consolas", 0xFFFFFF00, 150, 710, 20);
                consoleGraphics.FlipPages();

                if (Input.IsKeyDown(Keys.SPACE))
                    proceed = true;
                if (Input.IsKeyDown(Keys.DOWN))
                    MoveCursorDown();
                if (Input.IsKeyDown(Keys.UP))
                    MoveCursorUp();
                if (Input.IsKeyDown(Keys.RETURN))
                    ChangeSettings();
                if (Input.IsKeyDown(Keys.ESCAPE))
                    Environment.Exit(0);
            }

            void PlaceCursor()
            {
                consoleGraphics.DrawString(">>", "Consolas", 0xFFFFFF00, 150, cursorPosition, 30);
            }

            void MoveCursorDown()
            {
                cursorPosition = cursorPosition >= cursorStart + (cursorOffset * 2)
                    ? cursorPosition = cursorStart
                    : cursorPosition + cursorOffset;
                Thread.Sleep(100);
            }

            void MoveCursorUp()
            {
                cursorPosition = cursorPosition <= cursorStart
                    ? cursorPosition = cursorStart + (cursorOffset * 2)
                    : cursorPosition - cursorOffset;
                Thread.Sleep(100);
            }

            void ChangeSettings()
            {
                if (cursorPosition == cursorStart + (cursorOffset * 0))
                {
                    if (Settings.LevelSelector >= 12)
                        Settings.LevelSelector = 0;
                    else
                        Settings.LevelSelector++;
                }

                else if (cursorPosition == cursorStart + (cursorOffset * 1))
                {
                    if (Settings.SpeedSelector >= 9)
                        Settings.SpeedSelector = 0;
                    else
                        Settings.SpeedSelector++;
                }

                else if (cursorPosition == cursorStart + (cursorOffset * 2))
                {
                    if (Settings.ShapeSet == ShapeSets.NotTooRough)
                        Settings.ShapeSet = ShapeSets.TooYoungToDie;
                    else
                        Settings.ShapeSet = ShapeSets.NotTooRough;
                }

                Thread.Sleep(30);
            }
        }

        public void SetLevel()
        {
            grid.FillBoolDataWithRandomData(Settings.Level);
        }

        public void RedrawInGameScreen()
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
            System.Threading.Thread.Sleep(10);
        }   

        public void SetCurrentTetromino()
        {
            int nextShape = Utility.Random.Next((int)Settings.ShapeSet);

            switch (nextShape)
            {
                case 0:
                    currentTetromino = new TetrominoO();
                    break;
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
                default:
                    currentTetromino = new Randomino3x4();
                    break;
            }

            Tetromino.CurrentTetrominoRotation = currentTetromino.GetCurrentRotation();
            Tetromino.CurrentTetrominoPositionLine = 0;
            Tetromino.CurrentTetrominoPositionColumn = 4;
        }

        public void PutCurrentTetrominoOnCurrentGrid()
        {
            CopyGridToCurrentGrid(); 
            currentGrid.PutCurrentShapeOnBoolData(); 
            currentGrid.ConvertBoolDataToPixelData();
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
        
        public void MoveCurrentShapeDown()
        {
            if (speedTracking > 0)
            {
                speedTracking--;
                return;
            }
            else
            {
                speedTracking = Settings.Speed;

                BackUpPositionAndRotation();

                Tetromino.CurrentTetrominoPositionLine++;

                if (IsCurrentShapeBeyondCanvasBottom() || DoesCurrentShapeCollideWithData())
                {
                    Tetromino.CurrentTetrominoCanMoveDown = false;
                    RestorePositionAndRotation();
                }
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
            BackUpPositionAndRotation();

            if (Input.IsKeyDown(Keys.LEFT) && keypressIsAllowed)
            {
                Tetromino.CurrentTetrominoPositionColumn--;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
                DisableKeypress();
            }

            else if (Input.IsKeyDown(Keys.RIGHT) && keypressIsAllowed)
            {
                Tetromino.CurrentTetrominoPositionColumn++;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
                DisableKeypress();
            }

            else if (Input.IsKeyDown(Keys.UP) && keypressIsAllowed)
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
                Thread.Sleep(50);
                DisableKeypress();
            }

            else if (Input.IsKeyDown(Keys.DOWN))
            {
                speedTracking = 0;
            }
        }

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
