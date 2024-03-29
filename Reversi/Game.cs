﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reversi
{
    internal class Game : IGame
    {
        public static readonly Player PlayerNoOne = new Player("No one")
        {
            Color = 'N',
            HasTurn = false
        };

        private Square[,] board;
        private Player[] players;
        private readonly Utils utils = new Utils();
        private List<Point> whiteLegal = new List<Point>();
        private List<Point> blackLegal = new List<Point>();
        private string gameFile = "";

        public Player PlayerVictory { get; private set; }
        internal Square[,] Board => this.board;
        private int BoardSize { get; set; }
        private bool TraditionalSetup { get; set; }

        public Player GetCurrentPlayer()
        {
            return (from p in players where p.HasTurn select p).First();
        }

        public void Init(Player[] players, int size = 8, bool traditional = true)
        {
            if (size % 2 != 0 || size < 4)
                throw new Exception("The board size is invalid.");

            this.BoardSize = size;
            this.TraditionalSetup = traditional;
            this.board = new Square[size, size];

            if (players != null)
            {
                this.players = players;
                this.players[0].Color = 'B';
                this.players[1].Color = 'W';
            }
            else
            {
                this.players = new Player[2];
                this.players[0].Color = 'B';
                this.players[1].Color = 'W';
                this.players[0].Name = "A";
                this.players[1].Name = "B";
            }

            // init a file to save moves
            this.gameFile = DateTime.Now.ToString("MM/dd/yyyy HH-mm-ss") + ".txt";
            using StreamWriter sw = File.CreateText(this.gameFile);
            sw.WriteLine("Traditional setup: " + this.TraditionalSetup.ToString());
            sw.WriteLine("Board size: " + this.BoardSize.ToString());
            sw.WriteLine("Player white: " + this.players[0].Name);
            sw.WriteLine("Player black: " + this.players[1].Name);
            sw.Write("Move history: ");
        }

        public bool CanMakeMove(int x, int y, char color)
        {
            Player A = this.players[0], B = this.players[1];
            Player player = A.Color == color ? A : B;
            Player opponent = A.Color != color ? A : B;

            if (!this.IsLegal(x, y, player.Color)
                || !player.HasTurn) return false;

            var moves = player.Color == 'W'
                ? this.whiteLegal : this.blackLegal;
            if (moves.Count == 0)
            {
                player.HasTurn = false;
                opponent.HasTurn = true;
            }
            return moves.Count > 0;
        }

        private List<Point> FindDisksToFlip(int x, int y, char color)
        {
            List<Point> disksToFlip = new List<Point>();
            char opponentColor = color == 'W' ? 'B' : 'W';
            foreach (Point dir in this.utils.GenerateDirections())
            {
                List<Point> tempResults = new List<Point>();
                Point next = new Point(x + dir.X, y + dir.Y);
                if (this.IsInside(next.X, next.Y) &&
                    this.board[next.X, next.Y].Disk == opponentColor)
                {
                    tempResults.Add(next); next += dir;
                    while (this.IsInside(next.X, next.Y))
                    {
                        if (this.board[next.X, next.Y].Disk == color)
                        {
                            disksToFlip.AddRange(tempResults);
                            break;
                        }
                        else if (this.board[next.X, next.Y].Disk == opponentColor)
                        {
                            tempResults.Add(next);
                        }
                        else if (this.board[next.X, next.Y].IsEmpty)
                        {
                            break;
                        }
                        next += dir;
                    }
                }
            }
            return disksToFlip;
        }
        
        private List<Point> GenerateLegalMoves(char color)
        {
            List<Point> moves = new List<Point>();
            char opponentColor = color == 'W' ? 'B' : 'W';
            for (int x = 0; x < this.BoardSize; x++)
            {
                for (int y = 0; y < this.BoardSize; y++)
                {
                    bool alreadyFound = false;
                    if (!this.board[x, y].IsEmpty) continue;
                    foreach (Point dir in this.utils.GenerateDirections())
                    {
                        if (alreadyFound) break;
                        Point next = new Point(x + dir.X, y + dir.Y);
                        if (this.IsInside(next.X, next.Y) &&
                            this.board[next.X, next.Y].Disk == opponentColor)
                        {
                            next += dir;
                            while (this.IsInside(next.X, next.Y))
                            {
                                if (this.board[next.X, next.Y].Disk == color)
                                {
                                    moves.Add(new Point(x, y));
                                    alreadyFound = true; break;
                                }
                                else if (this.board[next.X, next.Y].IsEmpty) break;
                                next += dir;
                            }
                        }
                    }
                }
            }
            return moves;
        }

        public bool IsLegal(int x, int y, char color)
        {
            return color == 'W'
                ? this.whiteLegal.Any(m => m.X == x && m.Y == y)
                : this.blackLegal.Any(m => m.X == x && m.Y == y);
        }

        private bool IsInside(int x, int y)
        {
            return !this.IsOutside(x, y);
        }

        private bool IsOutside(int x, int y)
        {
            return x < 0 || x >= this.BoardSize
                || y < 0 || y >= this.BoardSize;
        }

        public void MakeMove(int x, int y, char color, bool init = false)
        {
            if (init == true)
            {
                this.board[x, y].Place(color);
                this.whiteLegal = this.GenerateLegalMoves('W');
                this.blackLegal = this.GenerateLegalMoves('B');
                return;
            }

            // Check if player can make a desired move
            if (!this.CanMakeMove(x, y, color)) return;

            // Save a move to the file
            File.AppendAllText(this.gameFile, string.Format(
                "{0}{1} ", (char)(x + 97), y + 1));

            // Flip the disks to opposite color
            List<Point> disksToFlip = this.FindDisksToFlip(x, y, color);
            foreach (Point p in disksToFlip) this.board[p.X, p.Y].Place(color);
            if (disksToFlip.Count > 0) this.board[x, y].Place(color);

            Player A = this.players[0], B = this.players[1];
            Player moveMaker = A.Color == color ? A : B;
            Player opponent = A.Color != color ? A : B;

            moveMaker.HasTurn = false;
            opponent.HasTurn = true;

            this.whiteLegal = this.GenerateLegalMoves('W');
            this.blackLegal = this.GenerateLegalMoves('B');

            // In case the opponent was blocked (no legal moves)
            if (moveMaker.Color == 'W' && this.blackLegal.Count == 0 ||
                moveMaker.Color == 'B' && this.whiteLegal.Count == 0)
            {
                moveMaker.HasTurn = true;
                opponent.HasTurn = false;
            }
        }

        public void NewGame()
        {
            // Reset a board
            for (int x = 0; x < this.BoardSize; x++)
                for (int y = 0; y < this.BoardSize; y++)
                    this.board[x, y] = new Square(x, y);

            int mid = this.BoardSize / 2 - 1;
            bool ts = this.TraditionalSetup;

            // Initialise a starting setup
            this.MakeMove(mid, mid, ts ? 'W' : 'B', true);
            this.MakeMove(mid + 1, mid, ts ? 'B' : 'W', true);
            this.MakeMove(mid, mid + 1, ts ? 'B' : 'B', true);
            this.MakeMove(mid + 1, mid + 1, ts ? 'W' : 'W', true);

            this.players[0].Color = 'B';
            this.players[1].Color = 'W';
            this.players[0].HasTurn = true;
            this.players[1].HasTurn = false;
        }

        public void UserUpdate(Resources resources, GameOptions gameOptions)
        {
            // Victory handler
            if (this.whiteLegal.Count == 0 && this.blackLegal.Count == 0)
            {
                int[] counts = new int[2] { 0, 0 };
                for (int player = 0; player <= 1; player++)
                {
                    for (int x = 0; x < BoardSize; x++)
                    {
                        for (int y = 0; y < BoardSize; y++)
                        {
                            char disk = this.board[x, y].Disk;
                            if (player == 0 && disk == 'B' ||
                                player == 1 && disk == 'W')
                                counts[player]++;
                        }
                    }
                }
                if (counts[0] > counts[1]) PlayerVictory = players[0];
                if (counts[0] < counts[1]) PlayerVictory = players[1];
                if (counts[0] == counts[1]) PlayerVictory = PlayerNoOne;

                // jest jeszcze jeden przypadek.. remis.. co wtedy?
            }

            // Make-move logic

            var mouse = Mouse.GetState();
            if ((mouse.LeftButton == ButtonState.Pressed)
                && mouse.X > resources.startX && mouse.Y > resources.startY
                && mouse.X < resources.startX + gameOptions.boardSize * resources.step
                && mouse.Y < resources.startY + gameOptions.boardSize * resources.step)
            {
                var pos = resources.coordTranslator.First(p =>
                    mouse.X >= p.Value.X && mouse.Y >= p.Value.Y
                    && mouse.X <= p.Value.X + resources.step &&
                    mouse.Y <= p.Value.Y + resources.step).Key;
                //char color = (mouse.LeftButton == ButtonState.Pressed) ? 'W' : 'B';
                char color = this.GetCurrentPlayer().Color;

                // Validate and make move..
                if (this.CanMakeMove(pos.X, pos.Y, color))
                    this.MakeMove(pos.X, pos.Y, color);
            }
        }

        public void UpdateBoardPositions(GraphicsDeviceManager graphics, Resources resources)
        {
            int totalWidth = graphics.PreferredBackBufferWidth;
            int totalHeight = graphics.PreferredBackBufferHeight;

            // Update board positions if user has changed the screen size..
            int width = totalWidth - resources.offsetX * 2;
            int height = totalHeight - resources.offsetY * 2;

            int count = width < height ? width : height;
            resources.startX = (totalWidth - count) / 2;
            resources.startY = (totalHeight - count) / 2;
            resources.step = count / this.BoardSize;

            // Then translate the coordinates to access squares faster.
            resources.coordTranslator.Clear();
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    int tx = resources.startX + resources.step * x;
                    int ty = resources.startY + resources.step * y;
                    resources.coordTranslator.Add(new Point(x, y), new Point(tx, ty));
                }
            }
        }
    }
 }
