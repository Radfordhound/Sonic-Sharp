using System;
using System.Windows.Forms;

namespace SonicSharp
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static Main game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //TODO: Re-enable these after completing debugging.
            //try
            //{
                Application.EnableVisualStyles();
                game = new Main();
                game.Run();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("ERROR: " + ex.Message,"SoniC#",MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}
        }
    }
#endif
}
