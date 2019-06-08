using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace Tetris
{
    class Rectangle
    {
        private int 
            positionX,
            positionY,
            width,
            height;

        private uint color;

        public Rectangle(int x, int y, int w, int h, uint color)
        {
            positionX = x;
            positionY = y;
            width = w;
            height = h;
            this.color = color;
        }

        public void Render(ConsoleGraphics graphics)
        {
            graphics.FillRectangle(color, positionX, positionY, width, height);
        }
    }
}
