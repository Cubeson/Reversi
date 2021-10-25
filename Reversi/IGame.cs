using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal interface IGame
    {
        void Init(Player[] players, int size, bool ts);
        bool IsLegal(int x, int y, char color);
        bool CanMakeMove(int x, int y, char color);
        void MakeMove(int x, int y, char color, bool init);
        void NewGame();
    }
}
