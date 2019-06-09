using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    static class Settings
    {
        public const int ConsoleWidth = 100;
        public const int ConsoleHeight = 50;

        public const int LineNumber = 21;       // FindAndReplace
        public const int ColumnNumber = 10;     // FindAndReplace

        public const int canvasOffsetX = (8 * ConsoleWidth - 10 * PixelSizeX) / 2;
        public const int canvasOffsetY = 50;

        public const int PixelSizeX = 30;
        public const int PixelSizeY = 30;

        public static int Level = LineNumber - 0;

        public static int HiScore = 0;
        public static int CurrentScore = 0;
    }
}
