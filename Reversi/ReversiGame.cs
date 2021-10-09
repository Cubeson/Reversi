using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Reversi
{
    public class ReversiGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        TextureWrapper squareTextureLight = new TextureWrapper();
        TextureWrapper squareTextureDark = new TextureWrapper();
        TextureWrapper circleWhite = new TextureWrapper();
        TextureWrapper circleBlack = new TextureWrapper();

        private SpriteFont font;

        SlotSquare[] slots;
        SlotDisk diskBlack = new SlotDisk();
        SlotDisk diskWhite = new SlotDisk();

        int xoff;
        int yoff;
        public ReversiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            diskBlack.color = circleBlack;
            diskWhite.color = circleWhite;
            slots = new SlotSquare[64];

            // 800 x 480
            xoff = (_graphics.PreferredBackBufferWidth / 2 - 4*32);
            yoff = (_graphics.PreferredBackBufferHeight / 2 - 4*32);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int index = j + i * 8;

                    if (i % 2 == 0 && j % 2 == 0 || i % 2 == 1 && j % 2 == 1)
                        slots[index] = new SlotSquare(j * 32 + xoff, i * 32 + yoff, squareTextureDark);
                    else
                        slots[index] = new SlotSquare(j * 32 + xoff, i * 32 + yoff, squareTextureLight);
                }
            }

            slots[3].disk = diskWhite;
            slots[16].disk = diskWhite;
            slots[40].disk = diskWhite;
            slots[42].disk = diskBlack;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            squareTextureLight.texture = Content.Load<Texture2D>("textures/square_green_light");
            squareTextureDark.texture = Content.Load<Texture2D>("textures/square_green_dark");
            circleWhite.texture = Content.Load<Texture2D>("textures/circle_white");
            circleBlack.texture = Content.Load<Texture2D>("textures/circle_black");
            font = Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            var mouse = Mouse.GetState();
            

            if(mouse.X >0 && mouse.Y > 0 && mouse.X < _graphics.PreferredBackBufferWidth && mouse.Y < _graphics.PreferredBackBufferHeight)
            {

                int xstart = slots[0].x;
                int xend = slots[63].x + slots[63].color.texture.Width*2;
                int ystart = slots[0].y;
                int yend = slots[63].y + slots[63].color.texture.Height*2;

                if(mouse.X > xstart && mouse.Y > ystart && mouse.X <= xend && mouse.Y <= yend)
                {
                    int index = (mouse.X - xoff) / 32 + (mouse.Y - yoff) / 32 * 8;
                    if (index >= 0 && index < 64)
                    {
                        if (mouse.LeftButton == ButtonState.Pressed)
                            slots[index].disk = diskBlack;
                        else if (mouse.RightButton == ButtonState.Pressed)
                            slots[index].disk = diskWhite;
                    }
                }

            }
            else
            {

            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach(var slot in slots)
            {
                slot.Draw(_spriteBatch);
            }
            var mouse = Mouse.GetState();
            //_spriteBatch.DrawString(font, _graphics.PreferredBackBufferHeight.ToString(), new Vector2(300, 300), Color.Black);
            //_spriteBatch.DrawString(font, _graphics.PreferredBackBufferWidth.ToString(), new Vector2(300, 400), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
