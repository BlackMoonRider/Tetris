using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    abstract class AbstractShape
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

        public AbstractShape()
        {
            RotationD = RotateArray(RotationA);
            RotationC = RotateArray(RotationD);
            RotationB = RotateArray(RotationC);
            Rotations = new List<bool[,]> { RotationA, RotationB, RotationC, RotationD };
            CurrentRotation = Utility.Random.Next(4);
        }

        bool[,] RotateArray(bool[,] array)
        {
            int lineNumber = array.GetUpperBound(1) + 1;
            int columnNumber = array.GetUpperBound(0) + 1;
            bool[,] result = new bool[lineNumber, columnNumber];

            for (int line = 0; line < lineNumber; line++)
            {
                for (int column = 0; column < columnNumber; column++)
                {
                    int newLine;
                    int newColumn;

                    newLine = column;
                    newColumn = lineNumber - (line + 1);

                    result[newColumn, newLine] = array[column, line];
                }
            }

            return result;
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
