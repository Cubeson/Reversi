using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

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
        public Menu(GameState gameState,Options options,GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.gameState = gameState;
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

            // Set partitioning configuration
            grid.ColumnsProportions.Add(new Proportion());
            grid.ColumnsProportions.Add(new Proportion());
            grid.ColumnsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());
            grid.RowsProportions.Add(new Proportion());

            // Add widgets

            var fovLabel = new Label();
            fovLabel.Text = "Fov";
            fovLabel.HorizontalAlignment = HorizontalAlignment.Center;
            grid.Widgets.Add(fovLabel);

            var speedLabel = new Label();
            speedLabel.Text = "Speed";
            speedLabel.HorizontalAlignment = HorizontalAlignment.Center;
            speedLabel.GridRow = 1;
            grid.Widgets.Add(speedLabel);

            var countLabel = new Label();
            countLabel.Text = "Count";
            countLabel.HorizontalAlignment = HorizontalAlignment.Center;
            countLabel.GridRow = 2;
            grid.Widgets.Add(countLabel);

            /* ############# */

            var fovTextBox = new TextBox();
            fovTextBox.Text = "90";
            fovTextBox.GridColumn = 1;
            fovTextBox.MinWidth = 100;
            fovTextBox.MaxWidth = 100;
            grid.Widgets.Add(fovTextBox);

            var speedTextBox = new TextBox();
            speedTextBox.Text = "100";
            speedTextBox.GridColumn = 1;
            speedTextBox.GridRow = 1;
            speedTextBox.MinWidth = 100;
            speedTextBox.MaxWidth = 100;
            grid.Widgets.Add(speedTextBox);

            var countTextBox = new TextBox();
            countTextBox.Text = "50";
            countTextBox.GridColumn = 1;
            countTextBox.GridRow = 2;
            countTextBox.MinWidth = 100;
            countTextBox.MaxWidth = 100;
            grid.Widgets.Add(countTextBox);

            var saveButton = new TextButton();
            saveButton.Text = "Save";
            saveButton.GridRow = 3;

            saveButton.TouchDown += (s, e) =>
            {
                string fovStr = fovTextBox.Text.Trim();
                fovStr = fovStr.Replace('.', ',');
                string speedStr = speedTextBox.Text.Trim();
                speedStr = speedStr.Replace('.', ',');
                string countStr = countTextBox.Text.Trim();
                countStr = countStr.Replace('.', ',');

                float fov;
                float speed;
                int count;

                try
                {
                    fov = float.Parse(fovStr);
                    speed = float.Parse(speedStr);
                    count = int.Parse(countStr);
                }
                catch (FormatException)
                {
                    AddPopUpWindow(panel,
                        graphics.PreferredBackBufferWidth / 2,
                        graphics.PreferredBackBufferHeight / 2,
                        string.Format("At least one of the arguments is invalid!"));
                    return;
                }

                fovTextBox.Text = fov.ToString();
                speedTextBox.Text = speed.ToString();
                countTextBox.Text = count.ToString();
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
