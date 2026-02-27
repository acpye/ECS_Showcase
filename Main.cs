using System;
using OpenGL.ENGINE.Managers;

namespace OpenGL_Game
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class MainEntry
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (SceneManager game = new SceneManager())
                game.Run();
        }
    }
#endif
}
