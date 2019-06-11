using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoL : Tetromino
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, false, true },
            {true, true, true },
        };

        protected override bool[,] RotationB { get; set; } =
        {
            {true, false },
            {true, false },
            {true, true },
        };
    }
}
