﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class TetrominoZ : Tetromino
    {
        protected override bool[,] RotationA { get; } =
        {
            {true, true, false },
            {false, true, true },
        };
    }
}
