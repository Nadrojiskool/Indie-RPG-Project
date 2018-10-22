using System;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetResolution();
            using (var game = new Game1())
                game.Run();
        }
        
        static void SetResolution()
        {
            Game1.displayWidth = 1920;//d.Width;
            Game1.displayHeight = 1080;//d.Height;
        }
    }
#endif
}
