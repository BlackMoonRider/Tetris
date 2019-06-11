using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Hexomino6 : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, false, true, true, true },
            {true, true, true, false, false },
        };
    }
}
