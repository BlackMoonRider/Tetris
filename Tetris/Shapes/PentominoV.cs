using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class PentominoV : AbstractShape
    {
        protected override bool[,] RotationA { get; } =
        {
            {false, false, true },
            {false, false, true },
            {true, true, true },
        };
    }
}
