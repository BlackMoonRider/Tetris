using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace Tetris
{
    class Pixel
    {
        public int PositionLine { get; set; }
        public int PositionColumn { get; set; }
        public uint Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Pixel(int x, int y, int w = Settings.PixelSizeX, int h = Settings.PixelSizeY, uint color = 0xFFFFFFFF)
        {
            PositionColumn = x * w;
            PositionLine = y * h;
            Width = w;
            Height = h;
            Color = color;
        }

        public void Render(ConsoleGraphics graphics)
        {
            Rectangle pixel = new Rectangle(Settings.canvasOffsetX + PositionLine, Settings.canvasOffsetY + PositionColumn, Width, Height, Color);
            pixel.Render(graphics);
            Rectangle inner = new Rectangle(Settings.canvasOffsetX + PositionLine + 1, Settings.canvasOffsetY + PositionColumn + 1, Width - 2, Height - 2, 0xFFFF0000);
            inner.Render(graphics);
        }

    }
}
