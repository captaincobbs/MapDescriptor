namespace MapDescriptorTest.Sprite
{
    using System;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A floating-point rectangle which allows for smooth animation.
    /// </summary>
    public struct SmoothRect : IEquatable<SmoothRect>
    {
        #region Variables

        /// <summary>
        /// An immutable, empty instance of the rectangle.
        /// </summary>
        public static SmoothRect Empty = new SmoothRect(0, 0, 0, 0);

        private float x;
        private float y;
        private float width;
        private float height;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a rectangle from a position and dimensions.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="isImmutable">
        /// Whether the dimensions of the rectangle are unchangeable. True by default.
        /// </param>
        public SmoothRect(float x, float y, float width, float height, bool isImmutable = true)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            IsImmutable = isImmutable;
        }

        /// <summary>
        /// Creates a rectangle from a position and dimensions.
        /// </summary>
        /// <param name="pos">The X and Y coordinates.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="isImmutable">
        /// Whether the dimensions of the rectangle are unchangeable. True by default.
        /// </param>
        public SmoothRect(Vector2 pos, float width, float height, bool isImmutable = true)
        {
            x = pos.X;
            y = pos.Y;
            this.width = width;
            this.height = height;
            IsImmutable = isImmutable;
        }

        /// <summary>
        /// Creates one rectangle from another.
        /// </summary>
        /// <param name="rect">
        /// The rectangle to copy. Copies immutable status unless keepImmutable is false.
        /// </param>
        /// <param name="keepImmutable">
        /// Whether to copy the immutability of the rectangle to copy from. If false, the new
        /// rectangle is mutable.
        /// </param>
        public SmoothRect(SmoothRect rect, bool keepImmutable = true)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
            IsImmutable = keepImmutable && rect.IsImmutable;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the x-coordinate, referring to position.
        /// </summary>
        public float X
        {
            get
            {
                return x;
            }

            set
            {
                if (!IsImmutable)
                {
                    x = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate, referring to position.
        /// </summary>
        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                if (!IsImmutable)
                {
                    y = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle, referring to dimension.
        /// </summary>
        public float Width
        {
            get
            {
                return width;
            }

            set
            {
                if (!IsImmutable)
                {
                    width = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle, referring to dimension.
        /// </summary>
        public float Height
        {
            get
            {
                return height;
            }

            set
            {
                if (!IsImmutable)
                {
                    height = value;
                }
            }
        }

        /// <summary>
        /// Gets the first Y coordinate.
        /// </summary>
        public float Top
        {
            get
            {
                return Y;
            }
        }

        /// <summary>
        /// Gets the second Y coordinate.
        /// </summary>
        public float Bottom
        {
            get
            {
                return Y + Height;
            }
        }

        /// <summary>
        /// Gets the first X coordinate.
        /// </summary>
        public float Left
        {
            get
            {
                return X;
            }
        }

        /// <summary>
        /// Gets the second X coordinate.
        /// </summary>
        public float Right
        {
            get
            {
                return X + Width;
            }
        }

        /// <summary>
        /// Gets the X and Y coordinates.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return new Vector2(X, Y);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is identical to the empty <see cref="SmoothRect"/>.
        /// </summary>
        /// <returns>
        /// True if this is the <see cref="Empty"/> <see cref="SmoothRect"/> instance.
        /// </returns>
        public bool IsEmpty
        {
            get
            {
                return Equals(Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the values of the SmoothRect can be mutated or not.
        /// </summary>
        public bool IsImmutable
        {
            get;
            private set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a new rectangle just large enough to fit two others.
        /// </summary>
        /// <param name="rect1">
        /// The first rectangle to include in the union.
        /// </param>
        /// <param name="rect2">
        /// The second rectangle to include in the union.
        /// </param>
        /// <returns>
        /// A rectangle that fits around two others.
        /// </returns>
        public static SmoothRect Union(SmoothRect rect1, SmoothRect rect2)
        {
            return new SmoothRect(
                Math.Min(rect1.x, rect2.x),
                Math.Min(rect1.y, rect2.y),
                Math.Max(rect1.x + rect1.width, rect2.x + rect2.width),
                Math.Max(rect1.y + rect1.height, rect2.y + rect2.height));
        }

        /// <summary>
        /// Returns whether or not two rectangles overlap.
        /// </summary>
        /// <param name="rect1">
        /// The first rectangle to include in the union.
        /// </param>
        /// <param name="rect2">
        /// The second rectangle to include in the union.
        /// </param>
        /// <returns>
        /// True if the rectangles overlap, false otherwise.
        /// </returns>
        public static bool IsIntersecting(SmoothRect rect1, SmoothRect rect2)
        {
            return rect1.Left <= rect2.Right &&
                rect1.Right >= rect2.Left &&
                rect1.Top <= rect2.Bottom &&
                rect1.Bottom >= rect2.Top;
        }

        /// <summary>
        /// Determines whether the specified rectangle is equivalent.
        /// </summary>
        /// <param name="other">
        /// The rectangle to compare to this rectangle.
        /// </param>
        /// <returns>
        /// True if the objects are equal, false otherwise.
        /// </returns>
        public bool Equals(SmoothRect other)
        {
            return
                x == other.x &&
                y == other.y &&
                width == other.width &&
                height == other.height;
        }

        /// <summary>
        /// Returns a <see cref="Rectangle"/> instance. Rounds to the nearest integer if midpoint
        /// rounding is used.
        /// </summary>
        /// <param name="useMidpointRounding">
        /// Whether to truncate the value when casting from float to int, or round instead.
        /// </param>
        /// <returns>
        /// A <see cref="Rectangle"/> with the same position and dimensions.
        /// </returns>
        public Rectangle ToRect(bool useMidpointRounding = false)
        {
            return useMidpointRounding
                ? new Rectangle(
                    (int)Math.Round(x),
                    (int)Math.Round(y),
                    (int)Math.Round(width),
                    (int)Math.Round(height))
                : new Rectangle(
                    (int)x,
                    (int)y,
                    (int)width,
                    (int)height);
        }

        #endregion
    }
}