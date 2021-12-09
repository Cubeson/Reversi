using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    class Resources
    {
        /* Cannot be changed in options menu */
        public readonly int offsetX = 60;
        public readonly int offsetY = 30;
        public int step;
        public int startX, startY;
        public Dictionary<Point, Point> coordTranslator;

        public Texture2D squareTextureLight, squareTextureDark;
        public Texture2D circleWhite, circleBlack;
        public SpriteFont font;
        
        public bool shouldExit = false;
    }
}
