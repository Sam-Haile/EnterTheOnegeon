using System;

namespace EnterTheOnegeon
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Game1 game = new Game1())
                game.Run();
        }
    }
}
