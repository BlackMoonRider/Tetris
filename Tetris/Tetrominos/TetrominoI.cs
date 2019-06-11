using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoI : Tetromino
    {
        protected override bool[,] RotationA { get; } =
        {
            {true},
            {true},
            {true},
            {true},
        };
    }
}
