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
    }
}
