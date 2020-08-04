using Microsoft.Xna.Framework;

namespace MapDescriptorTest
{
    public static class GameOptions
    {
        /// <summary>
        /// Pixel size of each world grid tile
        /// </summary>
        public const int TileSize = 128;
        
        /// <summary>
        /// Inertia speed of the camera
        /// </summary>
        public static float InertiaFactor = 0.15f;
        
        /// <summary>
        /// Inertia speed of the player
        /// </summary>
        public static float MovementInertiaFactor = 0.12f;

        /// random slashes stab me
        public static int MapSize = 20;

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
