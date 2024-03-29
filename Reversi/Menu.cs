﻿using FunctionTasker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.Text;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Reversi
{
    internal class Menu
    {
        private readonly int buttonWidthDefault = 120;
        private readonly int buttonHeightDefault = 80;
        private readonly GraphicsDeviceManager graphics;
        private readonly SpriteBatch spriteBatch;
        private Desktop desktop;
        private readonly GameState gameState;
        private readonly GameOptions gameOptions;
        private readonly Resources resources;
        private readonly ITasker tasker;
        private GameTime gameTime;

        public Menu(GameState gameState,GameOptions gameOptions, Resources resources,
            GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.tasker = new Tasker();
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.gameState = gameState;
            this.gameOptions = gameOptions;
            this.resources = resources;
            this.desktop = new Desktop();
            desktop.Root = NewMainMenu();
        }

        public void Draw(GameTime gameTime)
        {
            this.gameTime = gameTime;
            tasker.Update();
            desktop.Render();
        }

        private Panel NewMainMenu()
        {
            Panel panel = FullWindowPanel();

            Grid grid = new Grid
            {
                ShowGridLines = false,
                ColumnSpacing = 8,
                RowSpacing = 8,
                Left = (int)(graphics.PreferredBackBufferWidth / 2) - buttonWidthDefault / 2,
                Top =  buttonHeightDefault,
            };

            grid.ColumnsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            TextButton newGameButton = new TextButton
            {
                Text = "New Game",
                Width = buttonWidthDefault,
                Height = buttonHeightDefault,
                Background      = new SolidBrush(resources.colorButton),
                OverBackground  = new SolidBrush(resources.colorButtonOver),
                GridColumn = 0,
            };
            newGameButton.TouchDown += (s, e) =>
            {
                gameState.NewGame();
                desktop = new Desktop
                {
                    Root = NewGamePanel()
                };

            };
            grid.AddChild(newGameButton);
            
            TextButton optionsButton = new TextButton
            {
                Text = "Options",
                Width = buttonWidthDefault,
                Height = buttonHeightDefault,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
                GridRow = 1
            };
            optionsButton.TouchDown += (s, e) =>
            {
                desktop = new Desktop
                {
                    Root = NewOptionsMenu()
                };
            };
            grid.AddChild(optionsButton);

            panel.AddChild(grid);

            TextButton exitButton = new TextButton
            {
                Text = "Exit",
                Left = 5,
                Top = 5,
                Width = 50,
                Height = 40,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
            };
            exitButton.TouchDown += (s, e) =>
            {
                resources.shouldExit = true;
                //Environment.Exit(0);
            };
            panel.AddChild(exitButton);
            return panel;
        }

        private Panel NewGamePanel()
        {
            Panel panel = FullWindowPanel();
            AddBackButton(panel);
            Label playerLabel = new Label
            {
                Text = "",
                HorizontalAlignment = HorizontalAlignment.Center,
                Top = 5,
            };

            panel.AddChild(playerLabel);

            double timeToWait = 500d;
            tasker.AddTask(() =>
            {
               if (!gameState.isPlaying) return false;
               var timeSpan = gameTime.ElapsedGameTime;
               var x = timeSpan.TotalMilliseconds;
               timeToWait -= x;
               if(timeToWait < 0)
               {
                   //AddPopUpWindow(panel,String.Format("{0}",x));
                   gameState.allowMove = true;
                   return false;
               }
               return true;
            });

            tasker.AddTask( () =>
            {
                if (!gameState.isPlaying)
                {
                    return false;
                }
                Game game = gameState.game;

                int totalWidth = graphics.PreferredBackBufferWidth;
                int totalHeight = graphics.PreferredBackBufferHeight;
                // Stop drawing a board if screen size is too small
                if (totalWidth <= resources.offsetX + 20 ||
                    totalHeight <= resources.offsetY + 20) return true;

                bool lightSquare = false;
                spriteBatch.Begin();
                for (int j = 0; j < gameOptions.boardSize; j++)
                {
                    lightSquare = !lightSquare;
                    for (int i = 0; i < gameOptions.boardSize; i++)
                    {
                        Point p = resources.coordTranslator[new Point(i, j)];
                        var destination = new Rectangle(p.X, p.Y, resources.step, resources.step);

                        Texture2D texture = lightSquare == true ?
                            resources.squareTextureLight : resources.squareTextureDark;  
;                        spriteBatch.Draw(texture, destination, Color.White);

                        Square square = this.gameState.game.Board[i, j];
                        if (!square.IsEmpty)
                        {
                            Texture2D disk = square.Disk.Equals('W') ?
                                resources.circleWhite : resources.circleBlack;
                            spriteBatch.Draw(disk, destination, Color.White);
                        }

                        if (game.IsLegal(i, j, game.GetCurrentPlayer().Color))
                        {
                            spriteBatch.Draw(resources.whiteRectangle,destination,
                                resources.colorHighlightLegal);
                        }

                        lightSquare = !lightSquare;
                    }
                }
                spriteBatch.End();
                return true;
            });

            tasker.AddTask(() => { 
                if(playerLabel == null || !gameState.isPlaying)
                {
                    return false;
                }
                var player = gameState.game.GetCurrentPlayer();
                var colorString = player.Color == 'W' ? "White" : "Black";
                var color = player.Color == 'W' ? Color.White : Color.Black;
                playerLabel.Text = String.Format("{0}'s Turn! [{1}]", player.Name, colorString);
                playerLabel.TextColor = color;

                return true; 
            });

            tasker.AddTask(() =>
            {
               if(!gameState.isPlaying)
                return false;
               if(gameState.game.PlayerVictory != null)
               {
                    var player = gameState.game.PlayerVictory;
                    string msg = "";
                    if(player == Game.PlayerNoOne)
                       msg = String.Format("It's a tie! No one won!");
                   else
                        msg = String.Format("Player: {0} won!",player.Name);
                    AddPopUpWindow(panel,msg, () => { gameState.DisposeGame(); desktop.Root = NewMainMenu(); });
                    return false;
               }

               return true;
            });

            return panel;
        }

        private Panel NewOptionsMenu()
        {
            Panel panel = FullWindowPanel();
            int left = (graphics.PreferredBackBufferWidth / 2) - buttonWidthDefault;
            int top = (graphics.PreferredBackBufferHeight / 2) - buttonHeightDefault;
            Panel sub = new Panel
            {
                Background = new SolidBrush(resources.colorOptionsBack),
                Left = left - 10,
                Top = top - 10,
                Width =  280,
                Height = 150,
            };
            panel.AddChild(sub);

            var grid = new Grid
            {
                ShowGridLines = false,
                ColumnSpacing = 8,
                RowSpacing = 8,
                Left = (graphics.PreferredBackBufferWidth / 2) - buttonWidthDefault,
                Top = (graphics.PreferredBackBufferHeight / 2) - buttonHeightDefault
            };
            int gridrow = 0;

            // Set partitioning configuration
            grid.ColumnsProportions.Add(new Proportion());
            grid.ColumnsProportions.Add(new Proportion());
            grid.ColumnsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            /* 1st column */

            var boardSizeLabel = new Label
            {
                Text = "Board Size",
                HorizontalAlignment = HorizontalAlignment.Center,
                GridRow = gridrow++,
                
            };
            grid.Widgets.Add(boardSizeLabel);

            var playerALabel = new Label
            {
                Text = "Player Black Name",
                HorizontalAlignment = HorizontalAlignment.Center,
                GridRow = gridrow++
            };
            grid.Widgets.Add(playerALabel);

            var playerBLabel = new Label
            {
                Text = "Player White Name",
                HorizontalAlignment = HorizontalAlignment.Center,
                GridRow = gridrow++
            };
            grid.Widgets.Add(playerBLabel);

            var traditionalSetupLabel = new Label
            {
                Text = "Traditional Setup",
                HorizontalAlignment = HorizontalAlignment.Center,
                GridRow = gridrow++
            };
            grid.Widgets.Add(traditionalSetupLabel);

            var saveButton = new TextButton
            {
                Text = "Save",
                GridRow = gridrow++,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
            };
            grid.Widgets.Add(saveButton);

            /* 2nd column */
            gridrow = 0;
            var boardSizeTextBox = new TextBox
            {
                Text = gameOptions.boardSize.ToString(),
                GridColumn = 1,
                GridRow = gridrow++,
                MinWidth = 100,
                MaxWidth = 100,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
            };
            grid.Widgets.Add(boardSizeTextBox);

            var playerATextBox = new TextBox
            {
                Text = gameOptions.playerA.ToString(),
                GridColumn = 1,
                GridRow = gridrow++,
                MinWidth = 100,
                MaxWidth = 100,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
            };
            grid.Widgets.Add(playerATextBox);

            var playerBTextBox = new TextBox
            {
                Text = gameOptions.playerB.ToString(),
                GridColumn = 1,
                GridRow = gridrow++,
                MinWidth = 100,
                MaxWidth = 100,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
            };
            grid.Widgets.Add(playerBTextBox);

            var traditionalSetupCheckBox = new CheckBox
            {
                GridColumn = 1,
                GridRow = gridrow++,
                //MinWidth = 100,
                //MaxWidth = 100,
                //Background = new SolidBrush(resources.colorButton),
                //OverBackground = new SolidBrush(resources.colorButtonOver),
                IsChecked = gameOptions.isGameTraditional
            };
            grid.Widgets.Add(traditionalSetupCheckBox);

            saveButton.TouchDown += (s, e) =>
            {
                string boardSizeStr = boardSizeTextBox.Text.Trim();
                string playerAName = playerATextBox.Text.Trim();
                string playerBName = playerBTextBox.Text.Trim();
                bool traditional = traditionalSetupCheckBox.IsChecked;
                string err = "At least one of the arguments is invalid!\n";

                int boardSize;
                try
                {
                    bool throwException = false;
                    boardSize = int.Parse(boardSizeStr);
                    if (boardSize < 4 || boardSize > 32 || boardSize % 2 != 0)
                    {
                        err += String.Format("Invalid board size: {0}\n",boardSize);
                        throwException = true;
                    }
                    if (throwException)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    err += "Changes were not saved.";
                    AddPopUpWindow(panel,
                        err);
                    return;
                }
                gameOptions.boardSize = boardSize;
                gameOptions.playerA = playerAName;
                gameOptions.playerB = playerBName;
                gameOptions.isGameTraditional = traditional;


                boardSizeTextBox.Text = gameOptions.boardSize.ToString();

            };

            grid.Widgets.Add(saveButton);

            panel.AddChild(grid);
            AddBackButton(panel);
            return panel;
        }

        private void AddBackButton(MultipleItemsContainerBase container)
        {
            TextButton button = new TextButton
            {
                Text = "Back",
                Left = 5,
                Top = 5,
                Width = 50,
                Height = 40,
                Background = new SolidBrush(resources.colorButton),
                OverBackground = new SolidBrush(resources.colorButtonOver),
            };
            button.TouchDown += (s, e) =>
            {
                if (gameState.isPlaying)
                {
                    gameState.DisposeGame();
                }
                desktop.Root = NewMainMenu();
            };
            container.AddChild(button);
        }

        private Panel FullWindowPanel()
        {
            Panel panel = new Panel();
            int windowWidth = graphics.PreferredBackBufferWidth;
            int windowHeight = graphics.PreferredBackBufferHeight;
            panel.Width = windowWidth;
            panel.Height = windowHeight;
            return panel;
        }

        private void AddPopUpWindow(MultipleItemsContainerBase container, string message = "Exit", Action onClosedAction = null)
        {
            Window window = new Window
            {
                Left = graphics.PreferredBackBufferWidth/2,
                Top = graphics.PreferredBackBufferHeight/2,
            };
            if (onClosedAction != null)
            {
                window.Closed += (s, e) =>
                {
                    onClosedAction();
                };
            }
            Label label = new Label
            {
                Text = message,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            window.Content = label;

            container.AddChild(window);
        }
    }
}
