using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Myra.Graphics2D.UI;
using Myra.Attributes;
using Myra.MML;
using Myra.Utility;
using Myra;
using FunctionTasker;
namespace Reversi
{
    
    internal class Controller : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private Desktop _desktop;
        
        private Texture2D squareTextureLight, squareTextureDark;
        private Texture2D circleWhite, circleBlack;
        private GameState _gameState;
        //private Game game;
        private SpriteFont font;
        internal static bool isPlaying = false;
        private bool shouldUpdate = true;
        private readonly int offsetX = 60;
        private readonly int offsetY = 30;
        private int startX, startY, step;
        private Dictionary<Point, Point> coordTranslator;
        Options _options;
        ITasker _tasker;
        Menu menu;



        public Controller()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = true;
            _tasker = new Tasker();
        }

        public void OnResize(object sender, EventArgs e)
        {
            this.shouldUpdate = true;
        }

        //internal void InitGame()
        //{
        //    Player playerA = new Player("A");
        //    Player playerB = new Player("B");
        //    bool traditional = true;
        //    this.boardSize = 8;

        //    this.game = new Game();
        //    this.game.Init(new Player[] { playerA, playerB }, this.boardSize, traditional);
        //    this.game.NewGame();
        //}

        protected override void Initialize()
        {
            this.coordTranslator = new Dictionary<Point, Point>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            MyraEnvironment.Game = this;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.squareTextureLight = Content.Load<Texture2D>("textures/square_green_light");
            this.squareTextureDark = Content.Load<Texture2D>("textures/square_green_dark");
            this.circleWhite = Content.Load<Texture2D>("textures/circle_white");
            this.circleBlack = Content.Load<Texture2D>("textures/circle_black");
            this.font = Content.Load<SpriteFont>("File");


            _options = new Options();
            _gameState = new GameState(_options);
            menu = new Menu(_gameState,_options,_tasker,_graphics,_spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState.isPlaying)
            {

                int totalWidth = _graphics.PreferredBackBufferWidth;
                int totalHeight = _graphics.PreferredBackBufferHeight;

                //if (this.shouldUpdate == true)
                // TODO fix "shouldUpdate"
                // temporary solution until "shouldUpdate" is fixed
                if(true)
                {
                    // Update board positions if user has changed the screen size..
                    int width = totalWidth - this.offsetX * 2;
                    int height = totalHeight - this.offsetY * 2;

                    int count = width < height ? width : height;
                    this.startX = (totalWidth - count) / 2;
                    this.startY = (totalHeight - count) / 2;
                    this.step = count / _options.boardSize;

                    // Then translate the coordinates to access squares faster.
                    this.coordTranslator.Clear();
                    for (int x = 0; x < _options.boardSize; x++)
                    {
                        for (int y = 0; y < _options.boardSize; y++)
                        {
                            int tx = this.startX + this.step * x;
                            int ty = this.startY + this.step * y;
                            this.coordTranslator.Add(new Point(x, y), new Point(tx, ty));
                        }
                    }
                    this.shouldUpdate = false;
                }

                // Make-move logic

                var mouse = Mouse.GetState();
                if ((mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
                    && mouse.X > this.startX && mouse.Y > this.startY && mouse.X < this.startX +
                    _options.boardSize * this.step && mouse.Y < this.startY + _options.boardSize * this.step)
                {
                    var pos = this.coordTranslator.First(p =>
                        mouse.X >= p.Value.X && mouse.Y >= p.Value.Y
                        && mouse.X <= p.Value.X + this.step &&
                        mouse.Y <= p.Value.Y + this.step).Key;
                    char color = (mouse.LeftButton == ButtonState.Pressed) ? 'W' : 'B';

                    // Validate and make move..
                    if (this._gameState.game.CanMakeMove(pos.X, pos.Y, color))
                        this._gameState.game.MakeMove(pos.X, pos.Y, color);
                }
            }





            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
    || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            _tasker.Update();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            int size = _options.boardSize * _options.boardSize;
            _spriteBatch.Begin();
            //if (!(this.coordTranslator.Count < size))
                DrawBoard();
            _spriteBatch.End();
            menu.Draw();

            base.Draw(gameTime);
        }

        private void DrawBoard()
        {
            int totalWidth = _graphics.PreferredBackBufferWidth;
            int totalHeight = _graphics.PreferredBackBufferHeight;
            // Stop drawing a board if screen size is too small
            if (totalWidth <= this.offsetX + 20 ||
                totalHeight <= this.offsetY + 20) return;

            if (_gameState.isPlaying)
            {
                bool lightSquare = false;
                for (int j = 0; j < _options.boardSize; j++)
                {
                    lightSquare = !lightSquare;
                    for (int i = 0; i < _options.boardSize; i++)
                    {
                        Point p = this.coordTranslator[new Point(i, j)];
                        var destination = new Rectangle(p.X, p.Y, this.step, this.step);

                        Texture2D texture = lightSquare == true ?
                            this.squareTextureLight : this.squareTextureDark;
                        _spriteBatch.Draw(texture, destination, Color.White);

                        Square square = this._gameState.game.Board[i, j];
                        if (!square.IsEmpty)
                        {
                            Texture2D disk = square.Disk.Equals('W') ?
                                this.circleWhite : this.circleBlack;
                            _spriteBatch.Draw(disk, destination, Color.White);
                        }
                        lightSquare = !lightSquare;
                    }
                }

            }
        }
    }
}
