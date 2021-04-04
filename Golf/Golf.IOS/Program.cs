using System;
using Foundation;
using Golf.Core;
using UIKit;

namespace Golf.IOS
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static Core.MiniGolf _game;

        internal static void RunGame()
        {
            _game = new Core.MiniGolf();
            _game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }
    }
}
