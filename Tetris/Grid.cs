using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace Tetris
{
    public enum OutOfScreenProperties
    {
        None,
        Left,
        Right,
    }

    class Grid
    {
        public bool[,] BoolData { get; private set; }
        public List<Pixel> PixelData { get; private set; }

        public Grid()
        {
            BoolData = new bool[Settings.LineNumber, Settings.ColumnNumber];
            PixelData = new List<Pixel>();
        }

        public void FeedWithBoolData(Grid gridToCopyFrom)
        {
            bool[,] newBoolData = new bool[Settings.LineNumber, Settings.ColumnNumber];

            for (int x = 0; x < Settings.LineNumber; x++)
                for (int y = 0; y < Settings.ColumnNumber; y++)
                    newBoolData[x, y] = gridToCopyFrom.BoolData[x, y];

            BoolData = newBoolData;
        }
        
        public void PutCurrentShapeOnBoolData()// This may crash if you press DOWN
        {
            PixelData.Clear();
            for (int x = 0; x < Tetromino.CurrentTetrominoRotation.GetLength(0); x++)
                for (int y = 0; y < Tetromino.CurrentTetrominoRotation.GetLength(1); y++)
                {
                    if (Tetromino.CurrentTetrominoRotation[x, y])
                        BoolData[x + Tetromino.CurrentTetrominoPositionLine, y + Tetromino.CurrentTetrominoPositionColumn] = true;
                }
        }

        public void ConvertBoolDataToPixelData()
        {
            for (int x = 0; x < Settings.LineNumber; x++)
                for (int y = 0; y < Settings.ColumnNumber; y++)
                {
                    if (BoolData[x, y])
                        PixelData.Add(new Pixel(x, y));
                }
        }

        public void FillDataBoolWithRandomData(int startLine)
        {
            for (int x = startLine; x < Settings.LineNumber; x++)
                for (int y = 0; y < Settings.ColumnNumber; y++)
                {
                    if (Program.random.Next(0, 2) == 1)
                        BoolData[x, y] = true;
                }

            // This adds an extra line with only one gap to test the DropFullLines() method
            // This is for debug purposes

            //for (int y = 0; y < columnSize; y++)
            //    BoolData[startLine - 1, y] = true;

            //BoolData[startLine - 1, 4] = false;
        }

        public void DropFullLines() // Make this immutable (as well as the other methods)
        {
            List<bool[]> linesToKeep = new List<bool[]>();

            for (int line = 0; line < Settings.LineNumber; line++)
            {
                bool lineHasGaps = false;
                for (int column = 0; column < Settings.ColumnNumber; column++)
                {
                    if (!BoolData[line, column])
                    {
                        lineHasGaps = true;
                        break;
                    }
                }
                if (lineHasGaps)
                {
                    bool[] lineToKeep = new bool[Settings.ColumnNumber];
                    for (int column = 0; column < Settings.ColumnNumber; column++)
                    {
                        lineToKeep[column] = BoolData[line, column];
                    }
                    linesToKeep.Add((bool[])lineToKeep.Clone());
                }
            }

            int emptyLinesToAdd = BoolData.GetLength(0) - linesToKeep.Count;
            UpdateScores(emptyLinesToAdd);

            bool[,] newBoolData = new bool[Settings.LineNumber, Settings.ColumnNumber];

            for (int line = emptyLinesToAdd; line < Settings.LineNumber; line++)
                for (int column = 0; column < Settings.ColumnNumber; column++)
                {
                    newBoolData[line, column] = linesToKeep[line - emptyLinesToAdd][column];
                }

            for (int x = 0; x < Settings.LineNumber; x++)
                for (int y = 0; y < Settings.ColumnNumber; y++)
                    BoolData[x, y] = newBoolData[x, y];
        }

        private void UpdateScores(int emptyLinesToAdd)
        {
            int pointsToAdd = emptyLinesToAdd * 100;
            Settings.CurrentScore += pointsToAdd;
            if (Settings.CurrentScore > Settings.HiScore)
                Settings.HiScore = Settings.CurrentScore;
        }
    }
}
