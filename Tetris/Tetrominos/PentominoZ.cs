using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class PentominoZ : Tetromino
    {
        protected override bool[,] RotationA { get; } =
{
            {true, false, false, false },
            {true, true, true, true },
            {false, false, false, true },
        };
    }
}
