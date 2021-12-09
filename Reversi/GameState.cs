using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal class GameState
    {
        public Game game { get; private set; }
        public bool isPlaying { get; private set; }
        public bool shouldUpdate = true;
        public GameOptions options { get; private set; }
        public GameState(GameOptions options)
        {
            game = new Game(); 
            this.options = options;
        }
        public void DisposeGame()
        {
            //game.Dispose();
            game = null;
            this.isPlaying = false;
        }
        public Game getGame()
        {
            return game;
        }

        public void NewGame()
        {
            Player playerA = new Player(options.playerA);
            Player playerB = new Player(options.playerB);

            this.game = new Game();
            this.game.Init(new Player[] { playerA, playerB }, options.boardSize, options.isGameTraditional);
            this.game.NewGame();
            this.isPlaying = true;
            this.shouldUpdate = true;
        }
    }
}
