﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace Tetris
{
    class Pixel
    {
        public int PositionLine { get; }
        public int PositionColumn { get; }
        public uint Color { get; }
        public int Width { get; }
        public int Height { get; }

        private const int shrinkWidth = 2;
        private const int shrinkHeight = 2;
        private const uint primaryColor = 0xFFFFFFFF;
        private const uint secondaryColor = 0xFFFF0000;

        public Pixel(int x, int y, int width = Settings.PixelSizeX, int height = Settings.PixelSizeY, uint color = primaryColor)
        {
            PositionColumn = x * width;
            PositionLine = y * height;
            Width = width;
            Height = height;
            Color = color;
        }

        public void Render(ConsoleGraphics graphics)
        {
            Rectangle pixel = new Rectangle(Settings.canvasOffsetX + PositionLine, Settings.canvasOffsetY + PositionColumn, Width, Height, Color);
            pixel.Render(graphics);
            Rectangle inner = new Rectangle(Settings.canvasOffsetX + PositionLine + shrinkWidth / 2, Settings.canvasOffsetY + PositionColumn + shrinkHeight / 2, Width - shrinkWidth, Height - shrinkHeight, secondaryColor);
            inner.Render(graphics);
        }
    }
}
