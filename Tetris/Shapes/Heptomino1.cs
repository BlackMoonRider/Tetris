using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Heptomino1 : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, true, true },
            {true, true, true },
            {true, true, false },
        };
    }
}
