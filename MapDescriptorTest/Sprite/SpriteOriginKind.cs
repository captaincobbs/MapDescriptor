namespace MapDescriptorTest.Sprite
{
    /// <summary>
    /// Whether the origin expresses absolute coordinates, or coordinates
    /// derived as a percent of the width and height of the image.
    /// </summary>
    public enum SpriteOriginKind
    {
        /// <summary>
        /// The origin doesn't change based on the texture dimensions. The sprite will rotate
        /// and scale about the point (sprite X - x, sprite Y - y).
        /// </summary>
        Absolute,

        /// <summary>
        /// The origin changes based on the texture dimensions. (x, y) are percentages of the
        /// width and height. The sprite will rotate and scale about the point
        /// (sprite X - x * texture_width * scale / 100, sprite Y - y * texture_height * scale / 100).
        /// </summary>
        Percentile,
    }
}
