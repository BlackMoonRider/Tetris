using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class Utility
    {
        public static Random Random = new Random();
        public static bool[,] GetRandomArray2x3()
        {
            bool[,] result = new bool[2, 3];

            for (int line = 0; line < result.GetLength(0); line++)
                for (int column = 0; column < result.GetLength(1); column++)
                    result[line, column] = Random.Next(2) == 1 ? true : false;

            return result;
        }

        public static bool[,] GetRandomArray3x3()
        {
            bool[,] result = new bool[3, 3];

            for (int line = 0; line < result.GetLength(0); line++)
                for (int column = 0; column < result.GetLength(1); column++)
                    result[line, column] = Random.Next(2) == 1 ? true : false;

            return result;
        }

        public static bool[,] GetRandomArray3x4()
        {
            bool[,] result = new bool[3, 4];

            for (int line = 0; line < result.GetLength(0); line++)
                for (int column = 0; column < result.GetLength(1); column++)
                    result[line, column] = Random.Next(2) == 1 ? true : false;

            return result;
        }

        public static bool[,] GetRandomArray4x4()
        {
            bool[,] result = new bool[4, 4];

            for (int line = 0; line < result.GetLength(0); line++)
                for (int column = 0; column < result.GetLength(1); column++)
                    result[line, column] = Random.Next(2) == 1 ? true : false;

            return result;
        }

        public static bool[,] GetRandomArray5x5()
        {
            bool[,] result = new bool[5, 5];

            for (int line = 0; line < result.GetLength(0); line++)
                for (int column = 0; column < result.GetLength(1); column++)
                    result[line, column] = Random.Next(2) == 1 ? true : false;

            return result;
        }
    }
}
