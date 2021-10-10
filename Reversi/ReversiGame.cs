using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Reversi
{
    public class ReversiGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        int xoff;
        int yoff;
        int scale;
        public int ToIndex(int x, int y)
        {
            return ( ( (x - xoff) / (map.Slots[0].texture.Width *scale ) )
                    +  ( (y - yoff) / (map.Slots[0].texture.Height *scale )) * map.Size );

            //return ((x - xoff) *scale) / (map.Slots[0].texture.Width * scale)
            //   + ((y - yoff) *scale) / (map.Slots[0].texture.Height * scale * map.Size);
        }

        Map map;
        public ReversiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // 800 x 480
            //xoff = (_graphics.PreferredBackBufferWidth / 2 - 4*32);
            xoff = 200;
            //yoff = (_graphics.PreferredBackBufferHeight / 2 - 4*32);
            yoff = 200;

            scale = 2;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            ResourceManager.squareTextureLight = Content.Load<Texture2D>("textures/square_green_light");
            ResourceManager.squareTextureDark = Content.Load<Texture2D>("textures/square_green_dark");
            ResourceManager.circleWhite = Content.Load<Texture2D>("textures/circle_white");
            ResourceManager.circleBlack = Content.Load<Texture2D>("textures/circle_black");

            SlotDisk.diskBlack.texture = ResourceManager.circleBlack;
            SlotDisk.diskWhite.texture = ResourceManager.circleWhite;

            font = Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (map == null)
                map = new Map(8);
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.N))
                map = new Map(8);

            var mouse = Mouse.GetState();

            //int xstart = map.Slots[0].x + xoff;
            //int xend = (map.Slots[map.Size * map.Size - 1].x 
            //    + map.Slots[map.Size * map.Size - 1].texture.Width)
            //    +xoff;
            //int ystart = map.Slots[0].y + yoff;
            //int yend = (map.Slots[map.Size * map.Size - 1].y 
            //    + map.Slots[map.Size * map.Size - 1].texture.Height)
            //    + yoff;
            int xstart = scale * map.Slots[0].x + xoff;
            int xend = scale * (map.Slots[map.Size * map.Size - 1].x
                * map.Slots[0].texture.Width)
                + xoff + scale * map.Slots[0].texture.Width;
            int ystart = scale * map.Slots[0].y + yoff;
            int yend = scale * (map.Slots[map.Size * map.Size - 1].y
                * map.Slots[0].texture.Height)
                + yoff + scale * map.Slots[0].texture.Height;


            if (mouse.X > xstart && mouse.Y > ystart && mouse.X <= xend && mouse.Y <= yend)
            {
                //int index = ((mouse.X - xoff) / map.Slots[0].texture.Width
                //    + (mouse.Y - yoff) / map.Slots[0].texture.Height * map.Size);
                int index = ToIndex(mouse.X, mouse.Y);

                if (index >= 0 && index < map.Size * map.Size)
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                        map.SetSlot(index, SlotDisk.diskBlack);
                    else if (mouse.RightButton == ButtonState.Pressed)
                        map.SetSlot(index, SlotDisk.diskWhite);
                }
            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            map.Draw(_spriteBatch,xoff,yoff,scale);

            var mouse = Mouse.GetState();

            int index = ToIndex(mouse.X, mouse.Y);

            int xstart = scale * map.Slots[0].x + xoff;
            int xend = scale * (map.Slots[map.Size * map.Size - 1].x
                * map.Slots[0].texture.Width)
                + xoff + scale * map.Slots[0].texture.Width;
            int ystart = scale * map.Slots[0].y + yoff;
            int yend = scale * (map.Slots[map.Size * map.Size - 1].y
                * map.Slots[0].texture.Height)
                + yoff + scale * map.Slots[0].texture.Height;

            _spriteBatch.DrawString(font, (mouse.X).ToString(), new Vector2(50, 50), Color.Black);
            _spriteBatch.DrawString(font, xstart.ToString(), new Vector2(50, 100), Color.Black);
            _spriteBatch.DrawString(font, xend.ToString(), new Vector2(50, 150), Color.Black);            

            _spriteBatch.DrawString(font, (mouse.Y).ToString(), new Vector2(150, 50), Color.Black);
            _spriteBatch.DrawString(font, ystart.ToString(), new Vector2(150, 100), Color.Black);
            _spriteBatch.DrawString(font, yend.ToString(), new Vector2(150, 150), Color.Black);

            _spriteBatch.DrawString(font, index.ToString(), new Vector2(250, 50), Color.Black);
            //_spriteBatch.DrawString(font, _graphics.PreferredBackBufferWidth.ToString(), new Vector2(300, 400), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
