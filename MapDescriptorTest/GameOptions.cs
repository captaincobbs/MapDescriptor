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
        /// Inertia speed of the camera
        /// </summary>
        public static float InertiaFactor = 0.15f;
        /// <summary>
        /// Inertia speed of the player
        /// </summary>
        public static float MovementInertiaFactor = 0.12f;
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
