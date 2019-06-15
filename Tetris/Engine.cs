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
        AbstractShape currentTetromino;
        int backupPositionLine;
        int backupPositionColumn;
        bool[,] backupRotation;
        int speedTracking;
        bool keypressIsAllowed;

        public Engine(ConsoleGraphics consoleGraphics)
        {
            this.consoleGraphics = consoleGraphics;
            this.canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, (uint)Colors.Red);
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
                consoleGraphics.DrawString("TETЯIS", "Times New Roman", (uint)Colors.Yellow, 150, 80, 100);
                consoleGraphics.DrawString(" PRESS SPACE TO START", "Consolas", (uint)Colors.Yellow, 230, 650, 20);
                consoleGraphics.DrawString("LOGO BY FREEPNG.RU   EDUCATIONAL PROJECT ©2019", "Consolas", (uint)Colors.Yellow, 225, 750, 10);
                ConsoleImage logo = consoleGraphics.LoadImage(@"logo.bmp");
                consoleGraphics.DrawImage(logo, 255, 300);
                consoleGraphics.FlipPages();

                if (Input.IsKeyDown(Keys.SPACE))
                    proceed = true;
            }

            Utility.SleepLong();
        }

        public bool DrawGameOverScreen()
        {
            bool restartGame = false;

            while (!restartGame)
            {
                canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, (uint)Colors.GreyAlpha);
                canvas.Render(consoleGraphics);
                consoleGraphics.DrawString("GAME\r\nOVER", "Consolas", (uint)Colors.Yellow, 230, 80, 100);
                consoleGraphics.DrawString("PRESS SPACE TO RESTART", "Consolas", (uint)Colors.Yellow, 230, 650, 20);
                consoleGraphics.DrawString("PRESS ESC TO EXIT GAME", "Consolas", (uint)Colors.Yellow, 230, 680, 20);
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

            Utility.SleepLong();
            return restartGame;
        }

        public void DrawBlackCanvas()
        {
            canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, (uint)Colors.Black);
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
                canvas = new Rectangle(0, 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, (uint)Colors.GreyAlpha);
                canvas.Render(consoleGraphics);
                consoleGraphics.DrawString("GAME SETTINGS", "Consolas", (uint)Colors.Yellow, 150, 70, 50);
                consoleGraphics.DrawString($"LEVEL: {Settings.LevelSelector}", "Consolas", (uint)Colors.Yellow, 250, cursorStart, 30);
                consoleGraphics.DrawString($"SPEED: {Settings.SpeedSelector}", "Consolas", (uint)Colors.Yellow, 250, cursorStart + cursorOffset, 30);
                consoleGraphics.DrawString($"SHAPES: {Settings.ShapeSetName}", "Consolas", (uint)Colors.Yellow, 250, cursorStart + cursorOffset * 2, 30);
                PlaceCursor();
                consoleGraphics.DrawString("PRESS UP OR DOWN TO NAVIGATE", "Consolas", (uint)Colors.Yellow, 150, 620, 20);
                consoleGraphics.DrawString("PRESS ENTER TO CHANGE SETTINGS", "Consolas", (uint)Colors.Yellow, 150, 650, 20);
                consoleGraphics.DrawString("PRESS SPACE TO START GAME", "Consolas", (uint)Colors.Yellow, 150, 680, 20);
                consoleGraphics.DrawString("PRESS ESC TO EXIT GAME", "Consolas", (uint)Colors.Yellow, 150, 710, 20);
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
                consoleGraphics.DrawString(">>", "Consolas", (uint)Colors.Yellow, 150, cursorPosition, 30);
            }

            void MoveCursorDown()
            {
                cursorPosition = cursorPosition >= cursorStart + (cursorOffset * 2)
                    ? cursorPosition = cursorStart
                    : cursorPosition + cursorOffset;
                Utility.SleepLong();
            }

            void MoveCursorUp()
            {
                cursorPosition = cursorPosition <= cursorStart
                    ? cursorPosition = cursorStart + (cursorOffset * 2)
                    : cursorPosition - cursorOffset;
                Utility.SleepLong();
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
                    if (Settings.ShapeSet == ShapeSets.Nightmare)
                        Settings.ShapeSet = ShapeSets.TooYoungToDie;
                    else if (Settings.ShapeSet == ShapeSets.TooYoungToDie)
                        Settings.ShapeSet = ShapeSets.NotTooRough;
                    else if (Settings.ShapeSet == ShapeSets.NotTooRough)
                        Settings.ShapeSet = ShapeSets.HurtMePlenty;
                    else if (Settings.ShapeSet == ShapeSets.HurtMePlenty)
                        Settings.ShapeSet = ShapeSets.UltraViolence;
                    else if (Settings.ShapeSet == ShapeSets.UltraViolence)
                        Settings.ShapeSet = ShapeSets.Nightmare;
                }

                Utility.SleepMiddle();
            }
        }

        public void SetLevel()
        {
            grid.FillBoolDataWithRandomData(Settings.Level);
        }

        public void RedrawInGameScreen()
        {
            canvas = new Rectangle(Settings.canvasOffsetX + 0, Settings.canvasOffsetY + 0, consoleGraphics.ClientWidth, consoleGraphics.ClientHeight, (uint)Colors.Black);
            screen = new Rectangle(Settings.canvasOffsetX + 0, Settings.canvasOffsetY + 0, Settings.ColumnNumber * Settings.PixelSizeY, Settings.LineNumber * Settings.PixelSizeX, (uint)Colors.Grey);
            canvas.Render(consoleGraphics);
            screen.Render(consoleGraphics);

            foreach (Pixel pixel in currentGrid.PixelData)
                pixel.Render(consoleGraphics);

            consoleGraphics.DrawString($"   SCORE: {Settings.CurrentScore}", "Consolas", (uint)Colors.Yellow, 230, 680, 20);
            consoleGraphics.DrawString($"HI-SCORE: {Settings.HiScore}", "Consolas", (uint)Colors.Yellow, 230, 710, 20);

            consoleGraphics.FlipPages();
            Utility.SleepShort();
        }   

        public void SetCurrentTetromino()
        {
            int nextShape = Utility.Random.Next((int)Settings.ShapeSet);

            switch (nextShape)
            {
                case 0:
                    currentTetromino = new TetrominoO(); // Too young to die
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
                    currentTetromino = new TetrominoS(); // Not too rough
                    break;
                case 6:
                    currentTetromino = new TetrominoZ();
                    break;
                case 7:
                    currentTetromino = new Monomino(); // Hurt me plenty
                    break;
                case 8:
                    currentTetromino = new Domino();
                    break;
                case 9:
                    currentTetromino = new TrominoI();
                    break;
                case 10:
                    currentTetromino = new TrominoL();
                    break;
                case 11:
                    currentTetromino = new PentominoS();
                    break;
                case 12:
                    currentTetromino = new PentominoZ();
                    break;
                case 13:
                    currentTetromino = new PentominoT();
                    break;
                case 14:
                    currentTetromino = new PentominoU();
                    break;
                case 15:
                    currentTetromino = new PentominoX();
                    break;
                case 16:
                    currentTetromino = new PentominoV(); // Ultra-Violence
                    break;
                case 17:
                    currentTetromino = new PentominoW();
                    break;
                case 18:
                    currentTetromino = new Hexomino1();
                    break;
                case 19:
                    currentTetromino = new Hexomino2();
                    break;
                case 20:
                    currentTetromino = new Hexomino3();
                    break;
                case 21:
                    currentTetromino = new Heptomino1();
                    break;
                case 22:
                    currentTetromino = new Nonomino();
                    break;
                case 23:
                    currentTetromino = new Octomino1();
                    break;
                case 24:
                    currentTetromino = new Randomino2x3(); // Nightmare
                    break;
                case 25:
                    currentTetromino = new Randomino3x3();
                    break;
                case 26:
                    currentTetromino = new Randomino3x4();
                    break;
                default:
                    currentTetromino = new Randomino4x4();
                    break;
            }

            AbstractShape.CurrentTetrominoRotation = currentTetromino.GetCurrentRotation();
            AbstractShape.CurrentTetrominoPositionLine = 0;
            AbstractShape.CurrentTetrominoPositionColumn = 4;
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
            backupPositionLine = AbstractShape.CurrentTetrominoPositionLine;
            backupPositionColumn = AbstractShape.CurrentTetrominoPositionColumn;
            backupRotation = (bool[,])AbstractShape.CurrentTetrominoRotation.Clone();
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

                AbstractShape.CurrentTetrominoPositionLine++;

                if (IsCurrentShapeBeyondCanvasBottom() || DoesCurrentShapeCollideWithData())
                {
                    AbstractShape.CurrentTetrominoCanMoveDown = false;
                    RestorePositionAndRotation();
                }
            }

            
        }

        public void RestorePositionAndRotation()
        {
            AbstractShape.CurrentTetrominoPositionLine = backupPositionLine;
            AbstractShape.CurrentTetrominoPositionColumn = backupPositionColumn;
            AbstractShape.CurrentTetrominoRotation = (bool[,])backupRotation.Clone();
        }

        public void PutResultOnPermanentGrid()
        {
            currentGrid.DropFullLines();
            grid.FeedWithBoolData(currentGrid);
        }

        public bool DoesCurrentShapeCollideWithData()
        {
            bool doesCollide = false;

            for (int line = 0; line < AbstractShape.CurrentTetrominoRotation.GetLength(0); line++)
            {
                for (int column = 0; column < AbstractShape.CurrentTetrominoRotation.GetLength(1); column++)
                {
                    if (AbstractShape.CurrentTetrominoRotation[line, column] && 
                        currentGrid.BoolData[line + AbstractShape.CurrentTetrominoPositionLine, column + AbstractShape.CurrentTetrominoPositionColumn])
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

            if (AbstractShape.CurrentTetrominoPositionLine + AbstractShape.CurrentTetrominoRotation.GetLength(0) > Settings.LineNumber)
                isBeyond = true;

            return isBeyond;
        }

        public void CheckKeyboardInputAgainstCanvasAndData()
        {
            BackUpPositionAndRotation();

            if (Input.IsKeyDown(Keys.LEFT) && keypressIsAllowed)
            {
                AbstractShape.CurrentTetrominoPositionColumn--;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
                DisableKeypress();
            }

            else if (Input.IsKeyDown(Keys.RIGHT) && keypressIsAllowed)
            {
                AbstractShape.CurrentTetrominoPositionColumn++;
                if (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right || DoesCurrentShapeCollideWithData())
                {
                    RestorePositionAndRotation();
                }
                DisableKeypress();
            }

            else if (Input.IsKeyDown(Keys.UP) && keypressIsAllowed)
            {
                currentTetromino.SetNextRotation();
                AbstractShape.CurrentTetrominoRotation = currentTetromino.GetCurrentRotation();
                if (IsCurrentShapeBeyondCanvasBottom())
                {
                    currentTetromino.SetPreviousRotation();
                    RestorePositionAndRotation();
                }
                else
                {
                    while (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Left)
                    {
                        AbstractShape.CurrentTetrominoPositionColumn++;
                    }
                    while (CheckCurrentShapeOutOfScreenLeftRight() == OutOfScreenProperties.Right)
                    {
                        AbstractShape.CurrentTetrominoPositionColumn--;
                    }
                    if (DoesCurrentShapeCollideWithData())
                    {
                        currentTetromino.SetPreviousRotation();
                        RestorePositionAndRotation();
                    }
                }
                Utility.SleepMiddle();
                DisableKeypress();
            }

            else if (Input.IsKeyDown(Keys.DOWN))
            {
                speedTracking = 0;
            }
        }

        private OutOfScreenProperties CheckCurrentShapeOutOfScreenLeftRight()
        {
            if (AbstractShape.CurrentTetrominoPositionColumn + AbstractShape.CurrentTetrominoRotation.GetLength(1) > Settings.ColumnNumber)
                return OutOfScreenProperties.Right;
            if (AbstractShape.CurrentTetrominoPositionColumn < 0)
                return OutOfScreenProperties.Left;

            return OutOfScreenProperties.None;
        }

        private void ResetCurrentScore()
        {
            Settings.CurrentScore = 0;
        }
    }
}
