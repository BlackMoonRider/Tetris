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
        static public byte lineSize = 21;
        static public byte columnSize = 10;

        public bool[,] BoolData { get; private set; }
        public List<Pixel> PixelData { get; private set; }

        public Grid()
        {
            BoolData = new bool[lineSize, columnSize];
            PixelData = new List<Pixel>();
        }

        public void FeedWithBoolData(Grid gridToCopyFrom)
        {
            bool[,] newBoolData = new bool[lineSize, columnSize];

            for (int x = 0; x < lineSize; x++)
                for (int y = 0; y < columnSize; y++)
                    newBoolData[x, y] = gridToCopyFrom.BoolData[x, y];

            BoolData = newBoolData;
        }
        
        public void PutCurrentShapeOnBoolData()
        {
            PixelData.Clear();
            for (int x = 0; x < CurrentShape.Rotation.GetLength(0); x++)
                for (int y = 0; y < CurrentShape.Rotation.GetLength(1); y++)
                {
                    if (CurrentShape.Rotation[x, y])
                        BoolData[x + CurrentShape.PositionLine, y + CurrentShape.PositionColumn] = true;
                }
        }

        public void ConvertBoolDataToPixelData()
        {
            //PixelData = new List<Pixel>();
            for (int x = 0; x < lineSize; x++)
                for (int y = 0; y < columnSize; y++)
                {
                    if (BoolData[x, y])
                        PixelData.Add(new Pixel(x, y));
                }
        }

        // Returns true if the current shape collides with the bool data
        public bool CheckCurrentShapeCollision()
        {
            bool result = false;

            for (int x = 0; x < CurrentShape.Rotation.GetLength(0); x++)
                for (int y = 0; y < CurrentShape.Rotation.GetLength(1); y++)
                {
                    if (CurrentShape.Rotation[x, y] && BoolData[x + CurrentShape.PositionLine, y + CurrentShape.PositionColumn])
                        result = true;
                }

            return result;
        }

        // Returns true if the current shape is out of the up or bottom of the screen 
        public bool CheckCurrentShapeOutOfScreenUpBottom()
        {
            if (CurrentShape.PositionLine + CurrentShape.Rotation.GetLength(0) >= lineSize ||
                CurrentShape.PositionLine < 0)
                return true;

            return false;
        }

        // Returns true if the current shape is out of the left or right side of the screen 
        //public bool CheckCurrentShapeOutOfScreenLeftRight()
        //{
        //    if (CurrentShape.PositionColumn + CurrentShape.Rotation.GetLength(1) >= columnSize ||
        //        CurrentShape.PositionColumn <= 0)
        //        return true;

        //    return false;
        //}

        public OutOfScreenProperties CheckCurrentShapeOutOfScreenLeftRight()
        {
            if (CurrentShape.PositionColumn + CurrentShape.Rotation.GetLength(1) >= columnSize)
                return OutOfScreenProperties.Left;
            if (CurrentShape.PositionColumn <= 0)
                return OutOfScreenProperties.Right;

            return OutOfScreenProperties.None;
        }

        public void FillDataBoolWithRandomData(int startLine)
        {
            for (int x = startLine; x < lineSize; x++)
                for (int y = 0; y < columnSize; y++)
                {
                    if (Program.random.Next(0, 2) == 1)
                        BoolData[x, y] = true;
                }

            // This adds an extra line with only one gap to test the DropFullLines() method

            for (int y = 0; y < columnSize; y++)
                BoolData[startLine - 1, y] = true;

            BoolData[startLine - 1, 4] = false;
        }

        public void DropFullLines() // Make this immutable (as well as the other methods)
        {
            List<bool[]> linesToKeep = new List<bool[]>();

            for (int line = 0; line < lineSize; line++)
            {
                bool lineHasGaps = false;
                for (int column = 0; column < columnSize; column++)
                {
                    if (!BoolData[line, column])
                    {
                        lineHasGaps = true;
                        break;
                    }
                }
                if (lineHasGaps)
                {
                    bool[] lineToKeep = new bool[columnSize];
                    for (int column = 0; column < columnSize; column++)
                    {
                        lineToKeep[column] = BoolData[line, column];
                    }
                    linesToKeep.Add((bool[])lineToKeep.Clone());
                }
            }

            int emptyLinesToAdd = BoolData.GetLength(0) - linesToKeep.Count;

            bool[,] newBoolData = new bool[lineSize, columnSize];

            for (int line = emptyLinesToAdd; line < lineSize; line++)
                for (int column = 0; column < columnSize; column++)
                {
                    newBoolData[line, column] = linesToKeep[line - emptyLinesToAdd][column];
                }

            for (int x = 0; x < lineSize; x++)
                for (int y = 0; y < columnSize; y++)
                    BoolData[x, y] = newBoolData[x, y];
        }
    }
}
