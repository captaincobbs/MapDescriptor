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
        public Vector2 Coordinate { get; set; }
        public float Depth { get; set; }
        public float Rotation { get; set; }

        public EntityProperties(Texture2D image, Vector2 position, float depth, float rotation)
        {
            Image = image;
            Coordinate = position;
            Depth = depth;
            Rotation = rotation;
        }

        /// <summary>
        /// Makes the terrain draw itself when needed
        /// </summary>
        /// <param name="spriteBatch">Sprite Batched used to allow the Terrain to draw itself</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle(
               (int)(Coordinate.X * GameOptions.TileSize) - (GameOptions.TileSize / 2),
               (int)(Coordinate.Y * GameOptions.TileSize) - (GameOptions.TileSize / 2),
               GameOptions.TileSize,
               GameOptions.TileSize);

            spriteBatch.Draw(Image, destRect, null, Color.White, Rotation, new Vector2(GameOptions.TileSize / 2, GameOptions.TileSize / 2), SpriteEffects.None, Depth);
        }
    }
}
