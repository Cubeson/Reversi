using System;

namespace Reversi
{
    public static class Program
    {
        //[STAThread]
        static void Main()
        {
            using var controller = new Controller();
            controller.Run();
        }
    }
}
