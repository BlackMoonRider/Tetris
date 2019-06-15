using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Octomino1 : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, true, true },
            {true, false, true },
            {true, true, true },
        };
    }
}
