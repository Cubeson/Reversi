using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal class Player
    {
        /// <summary>
        /// The player's disk color.
        /// </summary>
        public char Color { get; set; }

        /// <summary>
        /// Indicates if player has a turn to make a move.
        /// </summary>
        public bool HasTurn { get; set; }

        /// <summary>
        /// The player's name.
        /// </summary>
        public string Name { get; set; }

        public Player(string name)
        {
            this.Name = name;
        }
    }
}
