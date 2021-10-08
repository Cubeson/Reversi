using System;

namespace Reversi
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ReversiGame())
                game.Run();
        }
    }
}
