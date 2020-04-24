using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Entity
{
    public class EntityProperties
    {
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }
        public float Depth { get; set; }

        public EntityProperties(Texture2D image, Vector2 position, float depth)
        {
            Image = image;
            Position = position;
            Depth = depth;
        }

        /// <summary>
        /// Makes the terrain draw itself when needed
        /// </summary>
        /// <param name="spriteBatch">Sprite Batched used to allow the Terrain to draw itself</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, GameOptions.TileSize, GameOptions.TileSize);
            spriteBatch.Draw(Image, destRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, Depth);
        }
    }
}
