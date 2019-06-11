using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    abstract class Tetromino
    {
        protected virtual bool[,] RotationA { get; }
        protected virtual bool[,] RotationB { get; set; }
        protected virtual bool[,] RotationC { get; set; }
        protected virtual bool[,] RotationD { get; set; }

        protected List<bool[,]> Rotations { get; }
        protected int CurrentRotation { get; set; }

        static public int CurrentTetrominoPositionLine { get; set; }
        static public int CurrentTetrominoPositionColumn { get; set; }
        static public bool[,] CurrentTetrominoRotation { get; set; }
        static public bool CurrentTetrominoCanMoveDown { get; set; }

        public Tetromino()
        {
            RotationD = RotateArrayClockwise(RotationA);
            RotationC = RotateArrayClockwise(RotationD);
            RotationB = RotateArrayClockwise(RotationC);
            Rotations = new List<bool[,]> { RotationA, RotationB, RotationC, RotationD };
            CurrentRotation = Utility.Random.Next(4);
        }

        bool[,] RotateArrayClockwise(bool[,] src) // Refactor and rename
        {
            int width;
            int height;
            bool[,] dst;

            width = src.GetUpperBound(0) + 1;
            height = src.GetUpperBound(1) + 1;
            dst = new bool[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = height - (row + 1);

                    dst[newCol, newRow] = src[col, row];
                }
            }

            return dst;
        }

        public bool[,] GetCurrentRotation()
        {
            return Rotations[CurrentRotation];
        }

        public void SetNextRotation()
        {
            CurrentRotation = CurrentRotation >= Rotations.Count - 1
                ? 0
                : ++CurrentRotation;
        }

        public void SetPreviousRotation()
        {
            CurrentRotation = CurrentRotation <= 0
                ? Rotations.Count - 1
                : --CurrentRotation;
        }
    }
}
