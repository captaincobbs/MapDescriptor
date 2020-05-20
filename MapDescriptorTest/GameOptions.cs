using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest
{
    public static class GameOptions
    {
        /// <summary>
        /// Pixel size of each world grid tile
        /// </summary>
        public const int TileSize = 128;
        /// <summary>
        /// Size^2 of the world map
        /// </summary>
        public const int MapSize = 20;
        /// <summary>
        /// Delay for accepting input from the keyboard for player keyboard input
        /// </summary>
        public static int InputDelay = 6;
        /// <summary>
        /// Frames counted since last reached InputDelay
        /// </summary>
        public static int FrameCounter = 0;
        /// <summary>
        /// Inertia speed of the camera
        /// </summary>
        public static float InertiaFactor = 0.15f;
        /// <summary>
        /// Color of the window background
        /// </summary>
        public static Color BackgroundColor = Color.Black;
		/// <summary>
		/// Scrolls required to do scroll actions
		/// </summary>
		public static float ScrollSensitivity = 1f;
    }
}
