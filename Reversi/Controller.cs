using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reversi
{
    internal class Controller : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D squareTextureLight, squareTextureDark;
        private Texture2D circleWhite, circleBlack;

        private Game game;
        private SpriteFont font;
        private bool shouldUpdate = true;
        private int boardSize = 8;
        private readonly int offsetX = 60;
        private readonly int offsetY = 30;
        private int dimension, startX, startY, step;
        private Dictionary<Point, Point> coordTranslator;

        private Stack<IMenu> menuStack;

        private KeyboardState lastKeyboardState;

        public Controller()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = true;
        }

        public void OnResize(object sender, EventArgs e)
        {
            this.shouldUpdate = true;
        }

        protected override void Initialize()
        {
            // TODO: Menu screen with options?
            Player playerA = new Player(0, "A");
            Player playerB = new Player(1, "B");
            bool traditional = true;
            this.dimension = 8;

            this.game = new Game();
            this.game.Init(new Player[] { playerA, playerB }, this.dimension, traditional);
            this.game.NewGame();

            this.menuStack = new Stack<IMenu>();

            this.menuStack.Push(new Menu(new MenuComponent[] { MenuComponents.NewGame, MenuComponents.Options, MenuComponents.Exit }));

            // Default screen size: 800 x 480

            this.coordTranslator = new Dictionary<Point, Point>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.squareTextureLight = Content.Load<Texture2D>("textures/square_green_light");
            this.squareTextureDark = Content.Load<Texture2D>("textures/square_green_dark");
            this.circleWhite = Content.Load<Texture2D>("textures/circle_white");
            this.circleBlack = Content.Load<Texture2D>("textures/circle_black");
            this.font = Content.Load<SpriteFont>("File");
        }

        protected override void Update(GameTime gameTime)
        {
            int totalWidth = _graphics.PreferredBackBufferWidth;
            int totalHeight = _graphics.PreferredBackBufferHeight;

            if (this.shouldUpdate == true )
            {
                // Update board positions if user has changed the screen size..
                int width = totalWidth - this.offsetX * 2;
                int height = totalHeight - this.offsetY * 2;

                int count = width < height ? width : height;
                this.startX = (totalWidth - count) / 2;
                this.startY = (totalHeight - count) / 2;
                this.step = count / this.dimension;

                // Then translate the coordinates to access squares faster.
                this.coordTranslator.Clear();
                for (int x = 0; x < this.dimension; x++)
                {
                    for (int y = 0; y < this.dimension; y++)
                    {
                        int tx = this.startX + this.step * x;
                        int ty = this.startY + this.step * y;
                        this.coordTranslator.Add(new Point(x, y), new Point(tx, ty));
                    }
                }
                this.shouldUpdate = false;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            var mouse = Mouse.GetState();
            if ((mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
                && mouse.X > this.startX && mouse.Y > this.startY && mouse.X < this.startX +
                this.dimension * this.step && mouse.Y < this.startY + this.dimension * this.step)
            {
                // Make a move...
                // TODO: Validate move using IsLegal
                var pos = this.coordTranslator.First(p =>
                    mouse.X >= p.Value.X && mouse.Y >= p.Value.Y
                    && mouse.X <= p.Value.X + this.step &&
                    mouse.Y <= p.Value.Y + this.step).Key;
                char color = (mouse.LeftButton == ButtonState.Pressed) ? 'W' : 'B';
                if (!color.Equals('\0')) this.game.MakeMove(pos.X, pos.Y, color);
            }

            var keyboard = Keyboard.GetState();

            if(menuStack.Peek().getWindow() != null)
            {
                if (keyboard != lastKeyboardState && keyboard.IsKeyDown(Keys.Enter))
                {
                    menuStack.Peek().closeWindow();
                }
                    
            }
            else
            {
                if (keyboard != lastKeyboardState && keyboard.IsKeyDown(Keys.Down))
                {
                    menuStack.Peek().IndexUp();
                }
                if (keyboard != lastKeyboardState && keyboard.IsKeyDown(Keys.Up))
                {
                    menuStack.Peek().IndexDown();
                }

                if (keyboard != lastKeyboardState && keyboard.IsKeyDown(Keys.Enter))
                {
                    if (menuStack.Peek().getCurrentComponent() == MenuComponents.Exit)
                    {
                        if (menuStack.Count() <= 1)
                            Exit();
                        else menuStack.Pop();
                    }
                    else if (menuStack.Peek().getCurrentComponent() == MenuComponents.Options)
                    {
                        menuStack.Push(new Menu(new MenuComponent[] { MenuComponents.BoardSize, MenuComponents.Exit }));
                        var bsize = menuStack.Peek().getMenuComponents()[0];
                        bsize.AdditionalMessage = new string(" : " + boardSize.ToString());
                    }
                    else if (menuStack.Peek().getCurrentComponent() == MenuComponents.NewGame)
                    {
                        this.game.NewGame();
                    }
                    else if (menuStack.Peek().getCurrentComponent() == MenuComponents.BoardSize)
                    {
                        menuStack.Peek().setWindow(new Window(_graphics.PreferredBackBufferWidth/2,
                            _graphics.PreferredBackBufferHeight/2
                            ,100,60,
                            boardSize.ToString()));
                    }
                }
            }

            

            lastKeyboardState = keyboard;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            int totalWidth = _graphics.PreferredBackBufferWidth;
            int totalHeight = _graphics.PreferredBackBufferHeight;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Stop drawing a board if screen size is too small
            if (totalWidth <= this.offsetX + 20 ||
                totalHeight <= this.offsetY + 20) return;

            // Coord translator is not loaded yet
            int size = this.dimension * this.dimension;
            if (this.coordTranslator.Count < size) return;

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            bool lightSquare = false;
            for (int j = 0; j < this.dimension; j++)
            {
                lightSquare = !lightSquare;
                for (int i = 0; i < this.dimension; i++)
                {
                    Point p = this.coordTranslator[new Point(i, j)];
                    var destination = new Rectangle(p.X, p.Y, this.step, this.step);

                    Texture2D texture = lightSquare == true ?
                        this.squareTextureLight : this.squareTextureDark;
                    _spriteBatch.Draw(texture, destination, Color.White);

                    Square square = this.game.Board[i, j];
                    if (!square.IsEmpty)
                    {
                        Texture2D disk = square.Disk.Equals('W') ?
                            this.circleWhite : this.circleBlack;
                        _spriteBatch.Draw(disk, destination, Color.White);
                    }
                    lightSquare = !lightSquare;
                }
            }

            int menuY = 5;
            foreach (var c in menuStack.Peek().getMenuComponents())
            {

                if (c == menuStack.Peek().getCurrentComponent())
                {
                    _spriteBatch.DrawString(font, " # " + c.getMessage(), new Vector2(5, menuY), Color.Black);
                }
                else
                    _spriteBatch.DrawString(font, c.getMessage(), new Vector2(5, menuY), Color.Black);
                menuY += 15;
            }
            if(menuStack.Peek().getWindow() != null)
            {
                var window = menuStack.Peek().getWindow();
                var windowToDraw = new Rectangle(window.X, window.Y, window.Width, window.Height);
                _spriteBatch.Draw(squareTextureDark, windowToDraw, Color.White);
                _spriteBatch.DrawString(font, "test", new Vector2(window.X, window.Y), Color.Black);
            }
            
            

            //_spriteBatch.DrawString(font, _graphics.PreferredBackBufferHeight.ToString(), new Vector2(300, 300), Color.Black);
            //_spriteBatch.DrawString(font, _graphics.PreferredBackBufferWidth.ToString(), new Vector2(300, 400), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
