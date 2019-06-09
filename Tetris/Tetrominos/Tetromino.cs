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

        public Tetromino()
        {
            Rotations = new List<bool[,]> { RotationA, RotationB, RotationC, RotationD };
            CurrentRotation = Utility.Random.Next(4);
        }

        protected List<bool[,]> Rotations { get; set; }

        protected int CurrentRotation { get; set; }

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

        //public bool[,] PeekNextRotation()
        //{
        //    int maybeRotation = currentRotation >= rotations.Count - 1
        //        ? 0
        //        : ++currentRotation;

        //    return rotations[maybeRotation];
        //}
    }
}
