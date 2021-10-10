using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reversi
{
    interface Drawable
    {
        public void Draw(SpriteBatch spriteBatch, int xoffset = 0,int yoffset = 0, int scale = 1);
    }
}
