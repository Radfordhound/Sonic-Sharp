using Microsoft.Xna.Framework.Content;
using System;

namespace SonicSharp
{
    public static class Program
    {
        // Variables/Constants
        public static GameWindow Window;
        public static ContentManager Content => Window.Content;

        // Methods
        [STAThread]
        public static void Main()
        {
            using (Window = new GameWindow())
                Window.Run();
        }
    }
}