using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Randomino3x4 : AbstractShape
    {
        protected override bool[,] RotationA { get; } = Utility.GetRandomArray3x4();
    }
}
