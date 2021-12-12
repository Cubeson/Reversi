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
        public Texture2D whiteRectangle;
        public SpriteFont font;
        
        public bool shouldExit = false;

        private static Color HexToColor(string hex)
        {
            List<byte> ls = new List<byte>();
            for(int i=0;i<8; i = i + 2)
            {
                string s = hex.Substring(i, 2);
                ls.Add(byte.Parse(s,System.Globalization.NumberStyles.HexNumber));
            }
            //                      Red   Green Blue  Alpha
            Color color = new Color(ls[0],ls[1],ls[2],ls[3]);
            return color;
        }
        /* https://coolors.co/d9ed92-b5e48c-99d98c-76c893-52b69a-34a0a4-168aad-1a759f-1e6091-184e77 */
        public readonly Color colorButton           = HexToColor("184E77FF");
        public readonly Color colorButtonOver       = HexToColor("1A759FFF");
        public readonly Color colorBackground       = HexToColor("99D98CFF");
        public readonly Color colorOptionsBack      = HexToColor("76C893FF");
        public readonly Color colorHighlightLegal   = Color.Blue * 0.33f;
        

    }
}
