using MapDescriptorTest.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// Class used to manage entities and make them update themselves as needed
    /// </summary>
    public class EntityProperties
    {
        /// <summary>
        /// Image drawn to represent the entity on the world grid
        /// </summary>
        public Rectangle Image { get; set; }
        /// <summary>
        /// X,Y Position on the world grid of the Entity
        /// </summary>
        public Vector2 Coordinate { get; set; }
        /// <summary>
        /// Z-Index of an entity
        /// </summary>
        public float Depth { get; set; }
        /// <summary>
        /// Direction an entity is facing in radians
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Stores and manages the properties of an entity
        /// </summary>
        /// <param name="image">Image drawn on the world grid to represent the entity</param>
        /// <param name="coordinate">Position of the entity on the world grid</param>
        /// <param name="depth">Z-Index of an entity</param>
        /// <param name="rotation">Direction a entity shall be facing in radians</param>
        public EntityProperties(Rectangle image, Vector2 coordinate, float depth, float rotation)
        {
            Image = image;
            Coordinate = coordinate;
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

            spriteBatch.Draw(TextureIndex.SpriteAtlas, destRect, Image, Color.White, Rotation, new Vector2(GameOptions.TileSize / 2, GameOptions.TileSize / 2), SpriteEffects.None, Depth);
        }
    }
}
