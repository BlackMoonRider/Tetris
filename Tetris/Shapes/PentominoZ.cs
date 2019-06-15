using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class PentominoZ : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, true, false },
            {false, true, false },
            {false, true, true },
        };
    }
}
