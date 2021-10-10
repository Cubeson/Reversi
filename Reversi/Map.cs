using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    class Map : Drawable
    {
        public int Size { get; }
        public int BoundX { get; }
        public int BoundY { get; }
        public SlotSquare[] Slots { get; }

        public void SetSlot(int index, SlotDisk disk)
        {
            Slots[index].disk = disk;
        }

        public Map(int size)
        {
            if (size < 8)
                throw new ArgumentException("Map size cannot be lower than 8");

            Slots = new SlotSquare[size * size];
            this.Size = size;
            for(int i = 0; i < size; i++)
                for(int j = 0; j < size; j++)
                {
                    int index = j + i * 8;

                    if (i % 2 == 0 && j % 2 == 0 || i % 2 == 1 && j % 2 == 1)
                        Slots[index] = new SlotSquare(j, i, ResourceManager.squareTextureDark);
                    else
                        Slots[index] = new SlotSquare(j, i, ResourceManager.squareTextureLight);
                }
        }

        public void Draw(SpriteBatch spriteBatch, int xoffset = 0, int yoffset = 0, int scale = 1)
        {
            foreach(var slot in Slots)
            {
                slot.Draw(spriteBatch,xoffset,yoffset,scale);
            }
        }
    }
}
