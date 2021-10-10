using System;

namespace Reversi
{
    internal class Game : IGame
    {
        private Square[,] board;
        private Player[] players;

        internal Square[,] Board => this.board;

        private int BoardSize { get; set; }
        private bool TraditionalSetup { get; set; }

        public void Init(Player[] players, int dimension = 8, bool traditional = true)
        {
            if (dimension % 2 != 0 || dimension < 4)
                throw new Exception("The board dimension is invalid.");

            this.BoardSize = dimension;
            this.TraditionalSetup = traditional;
            this.board = new Square[dimension, dimension];

            if (players != null)
            {
                this.players = players;
                return;
            }

            this.players = new Player[2];
            this.players[0].Id = 0;
            this.players[1].Id = 1;
            this.players[0].Name = "A";
            this.players[1].Name = "B";
        }

        public bool CanMakeMove(Player player)
        {
            return true; // TODO
        }

        public bool IsLegal(int x, int y, char color)
        {
            return true; // TODO
        }

        public void MakeMove(int x, int y, char color, Player player = null)
        {
            this.board[x, y].Disk = color;
            this.board[x, y].IsEmpty = color.Equals('\0');

            // TODO: Update player's turn state
        }

        public void NewGame()
        {
            for (int x = 0; x < this.BoardSize; x++)
                for (int y = 0; y < this.BoardSize; y++)
                    this.board[x, y] = new Square(x, y);

            int mid = this.BoardSize / 2 - 1;
            bool ts = this.TraditionalSetup;

            this.MakeMove(mid, mid, ts ? 'W' : 'B');
            this.MakeMove(mid + 1, mid, ts ? 'B' : 'W');
            this.MakeMove(mid, mid + 1, ts ? 'B' : 'B');
            this.MakeMove(mid + 1, mid + 1, ts ? 'W' : 'W');

            this.players[0].HasTurn = true;
            this.players[1].HasTurn = false;
        }
    }
}
