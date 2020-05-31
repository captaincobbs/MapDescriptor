namespace MapDescriptorTest.Sprite
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A size for the texture expressed as either a fixed width and height or a multiplier for
    /// the texture's width and height.
    /// </summary>
    public struct SpriteSize
    {
        #region Variables

        /// <summary>
        /// Represents an uninitialized sprite size, with a value of <see cref="Vector2.Zero"/>
        /// of type <see cref="SpriteSizeKind.Scaling"/>.
        /// </summary>
        public static SpriteSize Empty = new SpriteSize(Vector2.Zero, SpriteSizeKind.Scaling);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new sprite size object.
        /// </summary>
        /// <param name="values">
        /// The dimensions of the new size, or multipliers of the existing size.
        /// </param>
        /// <param name="type">
        /// Whether the values represent dimensions or multipliers.
        /// </param>
        public SpriteSize(Vector2 values, SpriteSizeKind type)
        {
            Values = values;
            Kind = type;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of size used. Can specify the absolute dimensions of the image or a scaling.
        /// </summary>
        public SpriteSizeKind Kind { get; private set; }

        /// <summary>
        /// Gets the values used for the size. The meaning depends on the type of values used. Values
        /// with <see cref="SpriteSizeKind.WidthAndHeight"/> define the final width and height of
        /// the texture displayed on-screen. It may shrink or stretch to fit.
        /// <see cref="SpriteSizeKind.Scaling"/> is a simple multiplier for width and height.
        /// </summary>
        public Vector2 Values { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether this is the same as <see cref="SpriteSize.Empty"/>.
        /// </summary>
        /// <returns>
        /// True if this is equivalent to the <see cref="Empty"/> <see cref="SpriteSize"/> instance.
        /// </returns>
        public bool IsEmpty
        {
            get
            {
                return Equals(Empty);
            }
        }

        #endregion
    }
}
