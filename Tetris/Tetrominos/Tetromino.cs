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
        protected virtual bool[,] RotationB { get; }
        protected virtual bool[,] RotationC { get; }
        protected virtual bool[,] RotationD { get; }

        protected List<bool[,]> Rotations { get; set; }
        protected int CurrentRotation { get; set; }

        static public int CurrentTetrominoPositionLine { get; set; }
        static public int CurrentTetrominoPositionColumn { get; set; }
        static public bool[,] CurrentTetrominoRotation { get; set; }
        static public bool CurrentTetrominoCanMoveDown { get; set; }

        public Tetromino()
        {
            Rotations = new List<bool[,]> { RotationA, RotationB, RotationC, RotationD };
            CurrentRotation = Utility.Random.Next(4);
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
