﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class CurrentShape // Move these static properties to the abstract Tetromino class
    {
        static public int PositionLine { get; set; }
        static public int PositionColumn { get; set; }
        static public bool[,] Rotation { get; set; }
        static public bool CanMoveDown { get; set; }
    }
}