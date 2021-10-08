using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reversi
{

    //enum SlotDisk : sbyte
    //{
    //    WHITE = 0,
    //    BLACK = 1,
    //    EMPTY = -1
    //}
    class SlotDisk
    {
        public TextureWrapper color { get; set; }
    }
    class SlotSquare
    {
        public int x { get; set; }
        public int y { get; set; }
        public TextureWrapper color { get; set; }
        public SlotDisk disk { get; set; }

        public SlotSquare(int x,int y, TextureWrapper color)
        {
            this.disk = null;
            this.color = color;
            this.x = x;
            this.y = y;

        }

        public void Draw(SpriteBatch spriteBatch, int xoff = 0, int yoff = 0)
        {
            var slotToDraw = new Rectangle(x+xoff, y+yoff, color.texture.Width, color.texture.Height);
            spriteBatch.Draw(color.texture, slotToDraw, Color.White);
            if (disk != null)
            {
                var diskToDraw = new Rectangle(x + xoff, y+ yoff, disk.color.texture.Width, disk.color.texture.Height);
                spriteBatch.Draw(disk.color.texture, diskToDraw, Color.White);
            }
            
        }
    }
}
