using FunctionTasker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Menu(GameState gameState,GameOptions gameOptions,Resources resources,GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
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

        public void Draw()
        {
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

                GridColumn = 0
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
                Height = 40
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
            Label label = new Label
            {
                Text = "",
                Left = 10,
                Top = 60,
            };

            panel.AddChild(label);

            tasker.AddTask( () =>
            {
                if (!gameState.isPlaying)
                {
                    return false;
                }

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
                        spriteBatch.Draw(texture, destination, Color.White);

                        Square square = this.gameState.game.Board[i, j];
                        if (!square.IsEmpty)
                        {
                            Texture2D disk = square.Disk.Equals('W') ?
                                resources.circleWhite : resources.circleBlack;
                            spriteBatch.Draw(disk, destination, Color.White);
                        }
                        lightSquare = !lightSquare;
                    }
                }
                spriteBatch.End();
                return true;
            });

            tasker.AddTask(() => { 
                if(label == null || !gameState.isPlaying)
                {
                    return false;
                }
                var player = gameState.game.getCurrentPlayer();
                var colorString = player.Color == 'W' ? "White" : "Black";
                var color = player.Color == 'W' ? Color.White : Color.Black;
                label.Text = String.Format("{0}'s Turn! [{1}]", player.Name, colorString);
                label.TextColor = color;

                return true; 
            });

            return panel;
        }

        private Panel NewOptionsMenu()
        {
            Panel panel = FullWindowPanel();

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
                GridRow = gridrow++
            };
            grid.Widgets.Add(boardSizeLabel);

            var playerALabel = new Label
            {
                Text = "Player 1 Name",
                HorizontalAlignment = HorizontalAlignment.Center,
                GridRow = gridrow++
            };
            grid.Widgets.Add(playerALabel);

            var playerBLabel = new Label
            {
                Text = "Player 2 Name",
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
                GridRow = gridrow++
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
                MaxWidth = 100
            };
            grid.Widgets.Add(boardSizeTextBox);

            var playerATextBox = new TextBox
            {
                Text = gameOptions.playerA.ToString(),
                GridColumn = 1,
                GridRow = gridrow++,
                MinWidth = 100,
                MaxWidth = 100
            };
            grid.Widgets.Add(playerATextBox);

            var playerBTextBox = new TextBox
            {
                Text = gameOptions.playerB.ToString(),
                GridColumn = 1,
                GridRow = gridrow++,
                MinWidth = 100,
                MaxWidth = 100
            };
            grid.Widgets.Add(playerBTextBox);

            var traditionalSetupCheckBox = new CheckBox
            {
                GridColumn = 1,
                GridRow = gridrow++,
                MinWidth = 100,
                MaxWidth = 100,
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
                        err += "Changes were not saved.";
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {
                    AddPopUpWindow(panel,
                        graphics.PreferredBackBufferWidth / 2,
                        graphics.PreferredBackBufferHeight / 2,
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
                Height = 40
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

        private void AddPopUpWindow(MultipleItemsContainerBase container, int Left = 50, int Top = 50, string message = "Exit")
        {
            Window window = new Window
            {
                Left = Left,
                Top = Top
            };
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
