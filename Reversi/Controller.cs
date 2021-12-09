﻿using Microsoft.Xna.Framework;
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
        private GameState _gameState;
        internal static bool isPlaying = false;
        GameOptions _options;
        Resources _resources;
        Menu menu;
        public Controller()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.ClientSizeChanged += OnResize;
            IsMouseVisible = true;
        }

        public void OnResize(object sender, EventArgs e)
        {
            if(_gameState != null)
            _gameState.shouldUpdate = true;
        }
        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _resources = new Resources();
            _options = new GameOptions();
            _resources.coordTranslator = new Dictionary<Point, Point>();
            _gameState = new GameState(_options);
            MyraEnvironment.Game = this;
            menu = new Menu(_gameState, _options, _resources, _graphics, _spriteBatch);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _resources.squareTextureLight = Content.Load<Texture2D>("textures/square_green_light");
            _resources.squareTextureDark = Content.Load<Texture2D>("textures/square_green_dark");
            _resources.circleWhite = Content.Load<Texture2D>("textures/circle_white");
            _resources.circleBlack = Content.Load<Texture2D>("textures/circle_black");
            _resources.font = Content.Load<SpriteFont>("File");
        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState.isPlaying)
            {
                if (_gameState.shouldUpdate)
                {
                    _gameState.game.UpdateBoardPositions(_graphics,_resources);
                    _gameState.shouldUpdate = false;
                }
                _gameState.game.UserUpdate(_resources, _options);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
                || _resources.shouldExit == true)
                    Exit();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            int size = _options.boardSize * _options.boardSize;
            menu.Draw();
            base.Draw(gameTime);
        }

    }
}
