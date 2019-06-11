using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Octomino6 :AbstractShape 
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, false, true, false },
            {true, true, true, false },
            {false, true, true, true },
            {false, true, false, false },
        };
    }
}
