using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Heptomino2 : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, false, true },
            {true, true, true },
            {true, false, true },
        };
    }
}
