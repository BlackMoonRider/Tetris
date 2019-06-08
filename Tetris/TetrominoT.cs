using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoT : ITetromino
    {
        private bool[,] rotationA =
        {
            {false, true, false },
            {true, true, true },
        };

        private bool[,] rotationB =
        {
            {true, false },
            {true, true },
            {true, false },
        };

        private bool[,] rotationC =
        {
            {true, true, true },
            {false, true, false },
        };

        private bool[,] rotationD =
        {
            {false, true },
            {true, true },
            {false, true },
        };

        List<bool[,]> rotations;

        int currentRotation;

        public TetrominoT()
        {
            rotations = new List<bool[,]> { rotationA, rotationB, rotationC, rotationD };
            currentRotation = Utility.Random.Next(4);
        }

        public bool[,] GetCurrentRotation()
        {
            return rotations[currentRotation];
        }

        public void SetNextRotation()
        {
            currentRotation = currentRotation >= rotations.Count - 1
                ? 0
                : ++currentRotation;
        }
    }
}
