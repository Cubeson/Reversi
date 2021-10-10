using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reversi
{
    class SlotDisk
    {
        public Texture2D texture { get; set; }

        public static SlotDisk diskBlack = new SlotDisk();
        public static SlotDisk diskWhite = new SlotDisk();
    }
    class SlotSquare : Drawable
    {
        public int x { get; set; }
        public int y { get; set; }
        public Texture2D texture { get; set; }
        public SlotDisk disk { get; set; }

        public SlotSquare(int x,int y, Texture2D texture)
        {
            this.disk = null;
            this.texture = texture;
            this.x = x;
            this.y = y;

        }

        public void Draw(SpriteBatch spriteBatch, int xoffset = 0, int yoffset = 0, int scale = 1)
        {
            var slotToDraw = new Rectangle(scale * x*texture.Width + xoffset,
                scale * y*texture.Height + yoffset,
                scale * texture.Width,
                scale * texture.Height);
            spriteBatch.Draw(texture, slotToDraw, Color.White);
            if (disk != null)
            {
                var diskToDraw = new Rectangle(scale * x*disk.texture.Width + xoffset,
                    scale * y * disk.texture.Height + yoffset,
                    scale * disk.texture.Width,
                    scale * disk.texture.Height);
                spriteBatch.Draw(disk.texture, diskToDraw, Color.White);
            }
            
        }
    }
}
