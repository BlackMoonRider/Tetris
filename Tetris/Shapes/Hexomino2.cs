using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Hexomino2 : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, false, false },
            {true, true, false },
            {true, true, true },
        };
    }
}
