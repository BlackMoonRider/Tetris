using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Domino : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, true },
        };
    }
}
