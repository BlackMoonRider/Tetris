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

        public const int LineNumber = 21;  
        public const int ColumnNumber = 10;

        public const int canvasOffsetX = (8 * ConsoleWidth - 10 * PixelSizeX) / 2;
        public const int canvasOffsetY = 50;

        public const int PixelSizeX = 30;
        public const int PixelSizeY = 30;

        public static int LevelSelector = 0;
        public static int Level => LineNumber - LevelSelector;


        public static int SpeedSelector = 0;

        public static int Speed => 10 - SpeedSelector;

        public static int ShapeSet = 0; // Use enum here

        public static int HiScore = 0;
        public static int CurrentScore = 0;
    }
}
