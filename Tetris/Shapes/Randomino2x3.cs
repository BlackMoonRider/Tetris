using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Randomino2x3 : AbstractShape
    {
        protected override bool[,] RotationA { get; } = Utility.GetRandomArray2x3();
    }
}
