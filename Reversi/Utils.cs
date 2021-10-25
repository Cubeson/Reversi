using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Reversi
{
    internal class Utils
    {
        internal List<Point> GenerateDirections()
        {
            var directions = new List<Point>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    directions.Add(new Point(i, j));
                }
            }
            return directions;
        }
    }
}
