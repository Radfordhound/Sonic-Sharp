using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.Reflection; // Let's take some time to reflect™...

namespace SonicSharp
{
    public static class Program
    {
        // Variables/Constants
        public static GameWindow Window;
        public static ContentManager Content => Window.Content;
        public static readonly string StartupPath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location);

        // Methods
        [STAThread]
        public static void Main()
        {
            using (Window = new GameWindow())
                Window.Run();
        }
    }
}