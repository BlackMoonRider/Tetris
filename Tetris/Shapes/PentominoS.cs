using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class PentominoS : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, true, true },
            {false, true, false },
            {true, true, false },
        };
    }
}
