using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class PentominoT : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, true, true },
            {false, true, false },
            {false, true, false },
        };
    }
}
