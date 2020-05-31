namespace MapDescriptorTest.Sprite
{
    using System;
    using System.Runtime.CompilerServices;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents a drawable graphic, which can be loaded upfront or deferred until requested.
    /// Sprite batch sprites can be used with default MonoGame tools, but are frequently slower on
    /// account of using sprite batching.
    /// </summary>
    public struct SpriteBatchSprite
    {
        #region Variables

        /// <summary>
        /// Optional mirroring options to draw the texture horizontally and/or vertically flipped.
        /// Default <see cref="SpriteEffects.None"/>.
        /// </summary>
        public SpriteEffects TextureMirroring;

        /// <summary>
        /// The (counter-clockwise) rotation angle in radians.
        /// Default 0.
        /// </summary>
        public float Angle;

        /// <summary>
        /// The drawing order for overlapping sprites as a value from 0.0 to 1.0, inclusive.
        /// Sprites with a smaller depth are drawn close to last and appear on top of others;
        /// note that this value is only sent to draw calls when <see cref="UseDepthInDraw"/>
        /// is true; else it's expected to be ignored or used with custom sorting logic.
        /// Default 0.
        /// </summary>
        public float Depth;

        /// <summary>
        /// If true, setting depth can change which <see cref="Draw"/> overload is used, assuming
        /// <see cref="SpriteSortMode.FrontToBack"/> or <see cref="SpriteSortMode.BackToFront"/>
        /// is used when rendering the sprite. Depth is still provided when false, but expected not
        /// to be honored. Ordinary sprite sorting will sort every frame; performance can be saved
        /// by implementing custom sort logic and leaving this value as false to make use of
        /// <see cref="Depth"/> yourself.
        /// Default false.
        /// </summary>
        public bool UseDepthInDraw;

        /// <summary>
        /// A color and alpha used to blend with the sprite.
        /// </summary>
        private Color cachedColorAlphaBlend;

        /// <summary>
        /// The origin, which is updated when the texture URI or origin change to avoid redundant
        /// computation on each draw call.
        /// </summary>
        private Vector2 cachedOrigin;

        /// <summary>
        /// The scale, which is updated when the size or texture changes to avoid redundant
        /// computation on each draw call.
        /// </summary>
        private Vector2 cachedScale;

        /// <summary>
        /// The dimensions to draw, which are updated when the texture URI or size changes to avoid
        /// redundant computation on each draw call.
        /// </summary>
        private Tuple<int, int> cachedSize;

        /// <summary>
        /// The texture to draw, which is updated when the origin or dimensions are changed, or if
        /// <see cref="Draw"/> is called, and set to null if texture uri is changed, to avoid
        /// redundant computation on each draw call.
        /// </summary>
        private Texture2D cachedTexture;

        /// <summary>
        /// The source rectangle defining the bounds within the texture to draw with, which is
        /// updated when the texture or source rectangle change to avoid redundant computation on
        /// each draw call.
        /// </summary>
        private Rectangle cachedSrcRect;

        /// <summary>
        /// The destination rectangle, which is updated when the position, size, or scale changes.
        /// </summary>
        private Rectangle cachedDestRect;

        /// <summary>
        /// Backing field for <see cref="Origin"/>.
        /// </summary>
        private SpriteOrigin origin;

        /// <summary>
        /// Backing field for <see cref="PerformBlendingPremultiplied"/>.
        /// </summary>
        private bool performBlendingPremultiplied;

        /// <summary>
        /// Backing field for <see cref="Position"/>.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// Backing field for <see cref="Size"/>.
        /// </summary>
        private SpriteSize size;

        /// <summary>
        /// Backing field for <see cref="TextureAlphaBlend"/>.
        /// </summary>
        private float textureAlphaBlend;

        /// <summary>
        /// Backing field for <see cref="TextureColorBlend"/>.
        /// </summary>
        private Color textureColorBlend;

        /// <summary>
        /// Backing field for <see cref="TextureSourceRect"/>.
        /// </summary>
        private SmoothRect textureSourceRect;

        /// <summary>
        /// Backing field for <see cref="TextureUri"/>.
        /// </summary>
        private string textureUri;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a sprite with default settings.
        /// </summary>
        /// <param name="textureUri">The relative project path for a texture.</param>
        public SpriteBatchSprite(string textureUri)
        {
            cachedColorAlphaBlend = Color.White;
            cachedOrigin = Vector2.Zero;
            cachedScale = Vector2.One;
            cachedSize = new Tuple<int, int>(0, 0);
            cachedTexture = null;
            cachedSrcRect = Rectangle.Empty;
            cachedDestRect = Rectangle.Empty;
            origin = SpriteOrigin.TopLeft;
            performBlendingPremultiplied = true;
            position = Vector2.Zero;
            size = SpriteSize.Empty;
            textureAlphaBlend = 1f;
            textureColorBlend = Color.White;
            textureSourceRect = SmoothRect.Empty;
            this.textureUri = textureUri;
            Angle = 0f;
            Depth = 0f;
            TextureMirroring = SpriteEffects.None;
            UseDepthInDraw = false;

            // Computes all cached values.
            cachedTexture = Texture2DManager.Get(textureUri);
            bool isCachedTextureLoaded = cachedTexture != null && !cachedTexture.IsDisposed;

            ComputeColorAlphaBlend();

            if (Origin.Kind != SpriteOriginKind.Percentile || isCachedTextureLoaded)
            {
                ComputeOrigin();
            }

            if (Size.IsEmpty || Size.Kind == SpriteSizeKind.Scaling || isCachedTextureLoaded)
            {
                ComputeScale();
            }

            if (Size.Kind != SpriteSizeKind.Scaling || isCachedTextureLoaded)
            {
                ComputeSizeAndDestRectangle();
            }

            if (!TextureSourceRect.IsEmpty || isCachedTextureLoaded)
            {
                ComputeSourceRectangle();
            }
        }

        /// <summary>
        /// Creates a deep copy of a sprite from another.
        /// </summary>
        /// <param name="sprite">The sprite to copy.</param>
        public SpriteBatchSprite(SpriteBatchSprite sprite)
        {
            Angle = sprite.Angle;
            cachedColorAlphaBlend = sprite.cachedColorAlphaBlend;
            cachedDestRect = sprite.cachedDestRect;
            cachedOrigin = sprite.cachedOrigin;
            cachedScale = sprite.cachedScale;
            cachedSize = sprite.cachedSize;
            cachedSrcRect = sprite.cachedSrcRect;
            cachedTexture = sprite.cachedTexture;
            Depth = sprite.Depth;
            UseDepthInDraw = sprite.UseDepthInDraw;
            origin = sprite.Origin;
            performBlendingPremultiplied = sprite.PerformBlendingPremultiplied;
            position = sprite.Position;
            size = sprite.Size;
            textureAlphaBlend = sprite.TextureAlphaBlend;
            textureColorBlend = sprite.TextureColorBlend;
            TextureMirroring = sprite.TextureMirroring;
            textureSourceRect = sprite.TextureSourceRect;
            textureUri = sprite.textureUri;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rectangle defining which portion of the source texture to use. If set
        /// to <see cref="SmoothRect.Empty"/>, the full dimensions of the texture will be used.
        /// Default <see cref="SmoothRect.Empty"/>.
        /// </summary>
        public SmoothRect TextureSourceRect
        {
            get
            {
                return textureSourceRect;
            }

            set
            {
                textureSourceRect = value;

                // Performance: these values are not used with a null texture or when , and are
                // updated when the texture value changes. If this changes, this should be updated.
                if (cachedTexture != null || !TextureSourceRect.IsEmpty)
                {
                    ComputeSourceRectangle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the relative project path for a texture.
        /// Default empty string.
        /// </summary>
        public string TextureUri
        {
            get
            {
                return textureUri;
            }

            set
            {
                textureUri = value;
                cachedTexture = Texture2DManager.Get(textureUri);

                if (cachedTexture != null && !cachedTexture.IsDisposed)
                {
                    RecomputeCache();
                }
            }
        }

        /// <summary>
        /// Gets or sets the point in local texture coordinates to rotate, scale, and draw from.
        /// Default <see cref="SpriteOrigin.TopLeft"/>.
        /// </summary>
        public SpriteOrigin Origin
        {
            get
            {
                return origin;
            }

            set
            {
                origin = value;

                // Performance: these values are not used with a null texture, and are updated when
                // the texture value changes. If this changes, this should be updated.
                if (cachedTexture != null)
                {
                    ComputeOrigin();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to treat colors as premultiplied when blending
        /// color. Leave as premultiplied when rendering sprites using
        /// <see cref="BlendState.AlphaBlend"/>.
        /// Default true.
        /// </summary>
        public bool PerformBlendingPremultiplied
        {
            get
            {
                return performBlendingPremultiplied;
            }

            set
            {
                performBlendingPremultiplied = value;
                ComputeColorAlphaBlend();
            }
        }

        /// <summary>
        /// Gets or sets the screen coordinates to draw the texture at.
        /// Default <see cref="Vector2.Zero"/>.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                ComputeDestRectangle();
            }
        }

        /// <summary>
        /// Gets or sets size. If not null, specifies the width and height of the drawn image as a final value or as
        /// a multiplier.
        /// Default <see cref="SpriteSize.Empty"/>.
        /// </summary>
        public SpriteSize Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;

                // Performance: these values are not used with a null texture, and are updated when
                // the texture value changes. If this changes, this should be updated.
                if (cachedTexture != null)
                {
                    ComputeScale();
                    ComputeSizeAndDestRectangle();
                }
            }
        }

        /// <summary>
        /// Gets or sets the alpha to blend the sprite with as a value between 0 and 1. Default 1 (no change).
        /// </summary>
        public float TextureAlphaBlend
        {
            get
            {
                return textureAlphaBlend;
            }

            set
            {
                textureAlphaBlend = value;
                ComputeColorAlphaBlend();
            }
        }

        /// <summary>
        /// Gets or sets a color to blend the sprite with. Default <see cref="Color.White"/> (no change).
        /// </summary>
        public Color TextureColorBlend
        {
            get
            {
                return textureColorBlend;
            }

            set
            {
                textureColorBlend = value;
                ComputeColorAlphaBlend();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the sprite using cached values computed when properties change.
        /// </summary>
        /// <param name="spriteBatch">
        /// A <see cref="SpriteBatch"/> instance to draw with. Expects Begin() to have been called.
        /// </param>
        /// <param name="mngr">
        /// The content manager to load textures from disk when not available, if provided. If a
        /// texture is not cached and no content manager is provided, it will fail to draw.
        /// </param>
        /// <returns>
        /// False if the sprite is null or disposed and cannot be drawn, else true.
        /// </returns>
        public bool Draw(SpriteBatch spriteBatch, ContentManager mngr = null)
        {
            // Fetches the texture if necessary.
            if (cachedTexture == null)
            {
                cachedTexture = Texture2DManager.Get(textureUri, mngr);

                if (cachedTexture == null || cachedTexture.IsDisposed)
                {
                    return false;
                }

                RecomputeCache();
            }
            else if (cachedTexture.IsDisposed)
            {
                return false;
            }

            // Draws the texture using cached values.
            if (RequiresFullDrawOverload())
            {
                // Texture, position, source, color, rotation, origin, scale, effects, depth
                spriteBatch.Draw(
                    cachedTexture,
                    Position,
                    cachedSrcRect,
                    cachedColorAlphaBlend,
                    Angle,
                    cachedOrigin,
                    cachedScale,
                    TextureMirroring,
                    Depth);
            }
            else if (!TextureSourceRect.IsEmpty)
            {
                if (!Size.IsEmpty)
                {
                    // Texture, position, size, source, color
                    spriteBatch.Draw(
                        cachedTexture,
                        cachedDestRect,
                        cachedSrcRect,
                        cachedColorAlphaBlend);
                }
                else
                {
                    // Texture, position, source, color
                    spriteBatch.Draw(
                        cachedTexture,
                        Position,
                        cachedSrcRect,
                        cachedColorAlphaBlend);
                }
            }
            else
            {
                if (!Size.IsEmpty)
                {
                    // Texture, position, size, color
                    spriteBatch.Draw(
                        cachedTexture,
                        cachedDestRect,
                        cachedColorAlphaBlend);
                }
                else
                {
                    // Texture, position, color
                    spriteBatch.Draw(
                        cachedTexture,
                        Position,
                        cachedColorAlphaBlend);
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the cached texture, loading it from disk if a content manager is provided and
        /// it's not available. Returns null if an error occurs or the content manager is null
        /// when the texture is not cached.
        /// </summary>
        /// <param name="mngr">
        /// The content manager to load textures from disk when not available, if provided. If a
        /// texture is not cached and no content manager is provided, null will be returned.
        /// </param>
        /// <returns>
        /// The <see cref="Texture2D"/> corresponding to the sprite.
        /// </returns>
        public Texture2D GetTexture(ContentManager mngr = null)
        {
            return Texture2DManager.Get(textureUri, mngr);
        }

        /// <summary>
        /// Forcibly runs cached computations that depend on texture. This should only be called if
        /// the texture referenced in this sprite is changed in <see cref="Texture2DManager"/>
        /// without changing the texture URI, or the sprite dimensions are directly mutated on the
        /// <see cref="Texture2D"/> instance. If the texture is null, nothing happens.
        /// </summary>
        public void RecomputeCache()
        {
            // Performance: these values are not used with a null texture, and are updated when
            // the texture value changes. If this changes, this should be updated.
            if (cachedTexture != null && !cachedTexture.IsDisposed)
            {
                // Performance: Scale doesn't depend on texture when Size is specified
                // in scaling. It's not used at all except in the full draw.
                if (Size.Kind != SpriteSizeKind.Scaling && RequiresFullDrawOverload())
                {
                    ComputeScale();
                }

                // Performance: Source rectangle only depends on texture if it's empty, since
                // it defaults to the full texture dimensions.
                if (TextureSourceRect.IsEmpty)
                {
                    ComputeSourceRectangle();
                }

                // Performance: Percentage-based origin computes actual dimensions based on
                // the texture's dimensions.
                if (Origin.Kind == SpriteOriginKind.Percentile)
                {
                    ComputeOrigin();
                }

                // Performance: Scaling-based size computes actual dimensions based on the
                // texture's dimensions.
                if (Size.Kind == SpriteSizeKind.Scaling)
                {
                    ComputeSizeAndDestRectangle();
                }
            }
        }

        /// <summary>
        /// Recomputes the cached color/alpha blend.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ComputeColorAlphaBlend()
        {
            cachedColorAlphaBlend = PerformBlendingPremultiplied
                ? TextureColorBlend * TextureAlphaBlend
                : new Color(TextureColorBlend.R, TextureColorBlend.G, TextureColorBlend.B, TextureAlphaBlend);
        }

        /// <summary>
        /// Recomputes the cached destination rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ComputeDestRectangle()
        {
            cachedDestRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                cachedSize.Item1,
                cachedSize.Item2);
        }

        /// <summary>
        /// Recomputes the cached origin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ComputeOrigin()
        {
            cachedOrigin = (Origin.Kind == SpriteOriginKind.Percentile)
                ? new Vector2(
                    Origin.Values.X * cachedTexture.Width,
                    Origin.Values.Y * cachedTexture.Height)
                : Origin.Values;
        }

        /// <summary>
        /// Recomputes the cached scale of the image.
        /// </summary>
        private void ComputeScale()
        {
            if (!Size.IsEmpty)
            {
                cachedScale = (Size.Kind == SpriteSizeKind.Scaling)
                    ? Size.Values
                    : new Vector2(
                        Size.Values.X / cachedTexture.Width,
                        Size.Values.Y / cachedTexture.Height);
            }
            else
            {
                cachedScale = Vector2.One;
            }
        }

        /// <summary>
        /// Recomputes the cached size of the image, then updates the destination rectangle.
        /// </summary>
        private void ComputeSizeAndDestRectangle()
        {
            cachedSize = (Size.Kind == SpriteSizeKind.Scaling)
                ? new Tuple<int, int>(
                    (int)(Size.Values.X * cachedTexture.Width),
                    (int)(Size.Values.Y * cachedTexture.Height))
                : new Tuple<int, int>(
                    (int)Size.Values.X,
                    (int)Size.Values.Y);

            ComputeDestRectangle();
        }

        /// <summary>
        /// Recomputes the cached source rectangle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ComputeSourceRectangle()
        {
            cachedSrcRect = TextureSourceRect.IsEmpty
                ? new Rectangle(0, 0, cachedTexture.Width, cachedTexture.Height)
                : TextureSourceRect.ToRect();
        }

        /// <summary>
        /// Returns whether or not a feature is in use that requires using the largest overload
        /// for <see cref="Draw"/>. Written for use in multiple places for better performance.
        /// </summary>
        /// <remarks>
        /// This function is very ad-hoc, and is written for cleaner usage in multiple places. The
        /// check is made for increased performance.
        /// </remarks>
        /// <returns>True if the overload is required.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool RequiresFullDrawOverload()
        {
            return Angle != 0
                || !Origin.Values.Equals(Vector2.Zero)
                || TextureMirroring != SpriteEffects.None
                || (UseDepthInDraw && Depth != 0);
        }

        #endregion
    }
}
