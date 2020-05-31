namespace MapDescriptorTest.Sprite
{
    /// <summary>
    /// Whether a <see cref="SpriteSize"/> represents the exact desired width and height, or a
    /// multiplier for the texture dimensions.
    /// </summary>
    public enum SpriteSizeKind
    {
        /// <summary>
        /// The texture will be drawn at its width and height multiplied by the (x, y) of the
        /// <see cref="SpriteSize"/>.
        /// </summary>
        Scaling,

        /// <summary>
        /// The texture will be drawn with this width and height, regardless of the original
        /// texture dimensions. This is useful for making textures conform to the same size to
        /// keep on-scren placement of textures simple.
        /// </summary>
        WidthAndHeight,
    }
}
