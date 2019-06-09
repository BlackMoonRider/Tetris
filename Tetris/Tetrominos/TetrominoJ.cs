using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoJ : Tetromino
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, false, false },
            {true, true, true },
        };

        protected override bool[,] RotationB { get; } =
        {
            {true, true },
            {true, false },
            {true, false },
        };

        protected override bool[,] RotationC { get; } =
        {
            {true, true, true },
            {false, false, true },
        };

        protected override bool[,] RotationD { get; } =
        {
            {false, true },
            {false, true },
            {true, true },
        };
    }
}
