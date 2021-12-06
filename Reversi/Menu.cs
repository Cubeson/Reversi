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
        private SpriteBatch spriteBatch;
        private Desktop desktop;
        private GameState gameState;
        private Options options;
        private FixedUpdate fixedUpdate;
        public Menu(GameState gameState,Options options,FixedUpdate fixedUpdate,GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.fixedUpdate = fixedUpdate;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.gameState = gameState;
            this.options = options;
            this.desktop = new Desktop();
            desktop.Root = NewMainMenu();
        }

        public void Draw()
        {
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
                Left = (int)(graphics.PreferredBackBufferWidth / 2) - buttonHeightDefault / 2,
                Top =  buttonHeightDefault,
            };

            grid.ColumnsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            TextButton newGameButton = new TextButton();
            newGameButton.Text = "New Game";
            newGameButton.Width = buttonWidthDefault;
            newGameButton.Height = buttonHeightDefault;

            newGameButton.GridColumn = 0;
            newGameButton.TouchDown += (s, e) =>
            {
                gameState.NewGame();
                desktop = new Desktop();
                desktop.Root = NewGamePanel();
                
            };
            grid.AddChild(newGameButton);

            TextButton optionsButton = new TextButton();
            optionsButton.Text = "Options";
            optionsButton.Width = buttonWidthDefault;
            optionsButton.Height = buttonHeightDefault;
            optionsButton.GridRow = 1;
            optionsButton.TouchDown += (s, e) =>
            {
                desktop = new Desktop();
                desktop.Root = NewOptionsMenu();
            };
            grid.AddChild(optionsButton);

            panel.AddChild(grid);

            TextButton exitButton = new TextButton();
            exitButton.Text = "Exit";
            exitButton.Left = 5;
            exitButton.Top = 5;
            exitButton.Width = 50;
            exitButton.Height = 40;
            exitButton.TouchDown += (s, e) =>
            {
                Environment.Exit(0);
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

            //FixedUpdate update = new FixedUpdate();
            VoidOp operation = null;
            operation = () => {

                if(label == null || !gameState.isPlaying)
                {
                    fixedUpdate.Remove(operation);
                }
                else
                {
                    var player = gameState.game.getCurrentPlayer();
                    var colorString = player.Color == 'W' ? "White" : "Black";
                    var color = player.Color == 'W' ? Color.White : Color.Black;
                    label.Text = String.Format("{0}'s Turn! [{1}]", player.Name, colorString);
                    label.TextColor = color;
                }
            };
            fixedUpdate.Add(operation);

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
                Left = (int)(graphics.PreferredBackBufferWidth / 2),
                Top = (int)(graphics.PreferredBackBufferHeight / 2)
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

            var boardSizeLabel = new Label();
            boardSizeLabel.Text = "Board Size";
            boardSizeLabel.HorizontalAlignment = HorizontalAlignment.Center;
            boardSizeLabel.GridRow = gridrow++;
            grid.Widgets.Add(boardSizeLabel);

            var playerALabel = new Label();
            playerALabel.Text = "Player 1 Name";
            playerALabel.HorizontalAlignment = HorizontalAlignment.Center;
            playerALabel.GridRow = gridrow++;
            grid.Widgets.Add(playerALabel);

            var playerBLabel = new Label();
            playerBLabel.Text = "Player 2 Name";
            playerBLabel.HorizontalAlignment = HorizontalAlignment.Center;
            playerBLabel.GridRow = gridrow++;
            grid.Widgets.Add(playerBLabel);

            var traditionalSetupLabel = new Label();
            traditionalSetupLabel.Text = "Traditional Setup";
            traditionalSetupLabel.HorizontalAlignment = HorizontalAlignment.Center;
            traditionalSetupLabel.GridRow = gridrow++;
            grid.Widgets.Add(traditionalSetupLabel);

            var saveButton = new TextButton();
            saveButton.Text = "Save";
            saveButton.GridRow = gridrow++;
            grid.Widgets.Add(saveButton);

            /* 2nd column */
            gridrow = 0;
            var boardSizeTextBox = new TextBox();
            boardSizeTextBox.Text = options.boardSize.ToString();
            boardSizeTextBox.GridColumn = 1;
            boardSizeTextBox.GridRow = gridrow++;
            boardSizeTextBox.MinWidth = 100;
            boardSizeTextBox.MaxWidth = 100;
            grid.Widgets.Add(boardSizeTextBox);

            var playerATextBox = new TextBox();
            playerATextBox.Text = options.playerA.ToString();
            playerATextBox.GridColumn = 1;
            playerATextBox.GridRow = gridrow++;
            playerATextBox.MinWidth = 100;
            playerATextBox.MaxWidth = 100;
            grid.Widgets.Add(playerATextBox);

            var playerBTextBox = new TextBox();
            playerBTextBox.Text = options.playerB.ToString();
            playerBTextBox.GridColumn = 1;
            playerBTextBox.GridRow = gridrow++;
            playerBTextBox.MinWidth = 100;
            playerBTextBox.MaxWidth = 100;
            grid.Widgets.Add(playerBTextBox);

            var traditionalSetupCheckBox = new CheckBox();
            traditionalSetupCheckBox.GridColumn = 1;
            traditionalSetupCheckBox.GridRow = gridrow++;
            traditionalSetupCheckBox.MinWidth = 100;
            traditionalSetupCheckBox.MaxWidth = 100;
            traditionalSetupCheckBox.IsChecked = options.isGameTraditional;
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
                options.boardSize = boardSize;
                options.playerA = playerAName;
                options.playerB = playerBName;
                options.isGameTraditional = traditional;


                boardSizeTextBox.Text = options.boardSize.ToString();

            };

            grid.Widgets.Add(saveButton);

            panel.AddChild(grid);
            AddBackButton(panel);
            return panel;
        }

        private void AddBackButton(MultipleItemsContainerBase container)
        {
            TextButton button = new TextButton();
            button.Text = "Back";
            button.Left = 5;
            button.Top = 5;
            button.Width = 50;
            button.Height = 40;
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
            Window window = new Window();
            window.Left = Left;
            window.Top = Top;
            Label label = new Label();
            label.Text = message;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            window.Content = label;

            container.AddChild(window);

        }
    }
}
