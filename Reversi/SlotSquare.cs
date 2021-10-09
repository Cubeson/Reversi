using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reversi
{
    class SlotDisk
    {
        public TextureWrapper color { get; set; }
    }
    class SlotSquare : Drawable
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

        public void Draw(SpriteBatch spriteBatch)
        {
            var slotToDraw = new Rectangle(x, y, color.texture.Width * 2, color.texture.Height * 2);
            spriteBatch.Draw(color.texture, slotToDraw, Color.White);
            if (disk != null)
            {
                var diskToDraw = new Rectangle(x, y, disk.color.texture.Width * 2, disk.color.texture.Height * 2);
                spriteBatch.Draw(disk.color.texture, diskToDraw, Color.White);
            }
            
        }
    }
}
