using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reversi
{
    internal class Square
    {
        /// <summary>
        /// Determines the X-coordinate of a board.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Determines the Y-coordinate of a board.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Determines the disk placed on a square.
        /// </summary>
        public char Disk { get; set; }

        /// <summary>
        /// Indicates if a disk is not placed on a square yet.
        /// </summary>
        public bool IsEmpty { get; set; }

        public Square(int x, int y, char color = '\0')
        {
            this.X = x;
            this.Y = y;
            this.Place(color);
        }

        public void Place(char color)
        {
            this.Disk = color;
            this.IsEmpty = color.Equals('\0');
        }
    }
}
