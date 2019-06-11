using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Octomino5 : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, true, false, true },
            {true, true, true, true },
            {true, false, true, false },
        };
    }
}
