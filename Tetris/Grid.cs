using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace Tetris
{
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

            newBoolData = (bool[,])gridToCopyFrom.BoolData.Clone();

            BoolData = newBoolData;
        }
        
        public void PutCurrentShapeOnBoolData()
        {
            PixelData.Clear();

            for (int line = 0; line < AbstractShape.CurrentTetrominoRotation.GetLength(0); line++)
                for (int column = 0; column < AbstractShape.CurrentTetrominoRotation.GetLength(1); column++)
                    if (AbstractShape.CurrentTetrominoRotation[line, column])
                        BoolData[line + AbstractShape.CurrentTetrominoPositionLine, column + AbstractShape.CurrentTetrominoPositionColumn] = true;
        }

        public void ConvertBoolDataToPixelData()
        {
            for (int line = 0; line < Settings.LineNumber; line++)
                for (int column = 0; column < Settings.ColumnNumber; column++)
                    if (BoolData[line, column])
                        PixelData.Add(new Pixel(line, column));
        }

        public void FillBoolDataWithRandomData(int startLine)
        {
            for (int line = startLine; line < Settings.LineNumber; line++)
                for (int column = 0; column < Settings.ColumnNumber; column++)
                    if (Utility.Random.Next(0, 2) == 1)
                        BoolData[line, column] = true;
        }

        public void DropFullLines()
        {
            List<bool[]> linesToKeep = new List<bool[]>();

            for (int line = 0; line < Settings.LineNumber; line++)
            {
                bool lineHasGaps = false;
                for (int column = 0; column < Settings.ColumnNumber; column++)
                    if (!BoolData[line, column])
                    {
                        lineHasGaps = true;
                        break;
                    }

                if (lineHasGaps)
                {
                    bool[] lineToKeep = new bool[Settings.ColumnNumber];
                    for (int column = 0; column < Settings.ColumnNumber; column++)
                        lineToKeep[column] = BoolData[line, column];

                    linesToKeep.Add((bool[])lineToKeep.Clone());
                }
            }

            int emptyLinesToAdd = BoolData.GetLength(0) - linesToKeep.Count;

            UpdateScores(emptyLinesToAdd);

            bool[,] newBoolData = new bool[Settings.LineNumber, Settings.ColumnNumber];

            for (int line = emptyLinesToAdd; line < Settings.LineNumber; line++)
                for (int column = 0; column < Settings.ColumnNumber; column++)
                    newBoolData[line, column] = linesToKeep[line - emptyLinesToAdd][column];

            BoolData = (bool[,])newBoolData.Clone();
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
