using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal interface IGame
    {
        void Init(Player[] players, int dimension, bool ts);
        bool IsLegal(int x, int y, char color);
        bool CanMakeMove(Player player);
        void MakeMove(int x, int y, char color, Player player);
        void NewGame();
    }
}
