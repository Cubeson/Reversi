using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Reversi
{
    public class ReversiGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D squareTextureLight;
        Texture2D squareTextureDark;
        Texture2D squareTextureAlternating;
        Texture2D circleWhite;
        Texture2D circleBlack;
        

        public ReversiGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            squareTextureLight = Content.Load<Texture2D>("textures/square_green_light");
            squareTextureDark = Content.Load<Texture2D>("textures/square_green_dark");
            circleWhite = Content.Load<Texture2D>("textures/circle_black");
            circleBlack = Content.Load<Texture2D>("textures/circle_white");
            
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

            bool alternate = false;
            int tx_width = 16;
            int tx_height = 16;
            for(int x = 0; x< 8; x = x + 1)
            {
                for(int y = 0; y<8; y = y + 1)
                {
                    var square_green = new Rectangle(x* tx_width, y* tx_height, tx_width, tx_height);
                    if (alternate)
                    {
                        alternate = false;
                        squareTextureAlternating = squareTextureDark;
                    }
                    else
                    {
                        alternate = true;
                        squareTextureAlternating = squareTextureLight;
                    }

                    _spriteBatch.Draw(squareTextureAlternating, square_green, Color.White);
                }

            }
            var circle = new Rectangle(4 * tx_width, 3 * tx_height, tx_width, tx_height);
            _spriteBatch.Draw(circleBlack, circle, Color.White);



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
