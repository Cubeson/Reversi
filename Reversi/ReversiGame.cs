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


        SlotSquare[] slots;
        SlotDisk diskBlack = new SlotDisk();
        SlotDisk diskWhite = new SlotDisk();

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
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int index = j + i * 8;

                    if (i % 2 == 0 && j % 2 == 0 || i % 2 == 1 && j % 2 == 1)
                        slots[index] = new SlotSquare(j * 16, i * 16, squareTextureDark);
                    else
                        slots[index] = new SlotSquare(j * 16, i * 16, squareTextureLight);
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


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
