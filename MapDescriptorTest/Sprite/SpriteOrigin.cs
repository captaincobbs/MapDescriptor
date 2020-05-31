namespace MapDescriptorTest.Sprite
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// An origin expressed as either absolute coordinates relative to the texture space
    /// or percentages of image width and height.
    /// </summary>
    public struct SpriteOrigin
    {
        #region Static Variables

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (0, 0).
        /// </summary>
        public static SpriteOrigin TopLeft = new SpriteOrigin(Vector2.Zero, SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (50, 0).
        /// </summary>
        public static SpriteOrigin TopCenter = new SpriteOrigin(new Vector2(0, 50), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (100, 0).
        /// </summary>
        public static SpriteOrigin TopRight = new SpriteOrigin(new Vector2(0, 100), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (0, 50).
        /// </summary>
        public static SpriteOrigin MidLeft = new SpriteOrigin(new Vector2(50, 0), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (50, 50).
        /// </summary>
        public static SpriteOrigin MidCenter = new SpriteOrigin(new Vector2(50, 50), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (100, 50).
        /// </summary>
        public static SpriteOrigin MidRight = new SpriteOrigin(new Vector2(50, 100), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (0, 100).
        /// </summary>
        public static SpriteOrigin BottomLeft = new SpriteOrigin(new Vector2(100, 0), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (50, 100).
        /// </summary>
        public static SpriteOrigin BottomCenter = new SpriteOrigin(new Vector2(100, 50), SpriteOriginKind.Percentile);

        /// <summary>
        /// Corresponds to a relative, percentage-based origin of (100, 100).
        /// </summary>
        public static SpriteOrigin BottomRight = new SpriteOrigin(new Vector2(100, 100), SpriteOriginKind.Percentile);
        #endregion

        /// <summary>
        /// Depicts an origin.
        /// </summary>
        /// <param name="values">
        /// The (x, y) or (width%, height%) values.
        /// </param>
        /// <param name="type">
        /// The type of values in use. Use <see cref="SpriteOriginKind.Absolute"/> for coordinates
        /// that should not vary based on the size of the texture; otherwise use
        /// <see cref="SpriteOriginKind.Percentile"/>.
        /// </param>
        public SpriteOrigin(Vector2 values, SpriteOriginKind type)
        {
            Values = values;
            Kind = type;
        }

        /// <summary>
        /// Gets the type of origin (absolute coordinates or relative to image width and height).
        /// </summary>
        public SpriteOriginKind Kind { get; private set; }

        /// <summary>
        /// Gets the values used for the origin. The meaning depends on the type of values used.
        /// Values with <see cref="SpriteOriginKind.Absolute"/> are (x, y) values. Example:
        /// (50, 10) is (sprite X + 50, sprite Y + 10). This is useful when the origin should be a
        /// fixed coordinate pair. Values with <see cref="SpriteOriginKind.Percentile"/> are
        /// (width%, height%) values. Example: (50, 10) is (sprite X + 0.5*scaled_width,
        /// sprite Y + 0.1*scaled_height), This is useful most of the time, since the origin
        /// adjusts based on the texture dimensions, allowing animations with different-sized
        /// frames and set origins to evaluate properly.
        /// </summary>
        public Vector2 Values { get; private set; }
    }
}
