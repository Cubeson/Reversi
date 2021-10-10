using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal class Player
    {
        /// <summary>
        /// Determines the ID for player.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Indicates if player has a turn to make a move.
        /// </summary>
        public bool HasTurn { get; set; }

        /// <summary>
        /// The player's name.
        /// </summary>
        public string Name { get; set; }

        public Player(int id, string name, bool hasTurn = false)
        {
            this.Id = id;
            this.Name = name;
            this.HasTurn = hasTurn;
        }
    }
}
