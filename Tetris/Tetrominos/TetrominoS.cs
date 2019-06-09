using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoS : Tetromino
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, true, true },
            {true, true, false },
        };

        protected override bool[,] RotationB { get; } =
        {
            {true, false },
            {true, true },
            {false, true },
        };

        protected override bool[,] RotationC { get; } =
        {
            {false, true, true },
            {true, true, false },
        };

        protected override bool[,] RotationD { get; } =
        {
            {true, false },
            {true, true },
            {false, true },
        };
    }
}
