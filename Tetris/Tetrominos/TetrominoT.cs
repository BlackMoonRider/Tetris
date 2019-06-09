using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoT : Tetromino
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, true, false },
            {true, true, true },
        };

        protected override bool[,] RotationB { get; } =
        {
            {true, false },
            {true, true },
            {true, false },
        };

        protected override bool[,] RotationC { get; } =
        {
            {true, true, true },
            {false, true, false },
        };

        protected override bool[,] RotationD { get; } =
        {
            {false, true },
            {true, true },
            {false, true },
        };
    }
}
