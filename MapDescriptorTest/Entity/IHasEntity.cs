using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// A class that uses this interface will have an entity
    /// </summary>
    public interface IHasEntity
    {
        /// <summary>
        /// Image drawn to represent the entity on the world grid
        /// </summary>
        Rectangle Image { get; set; }
        /// <summary>
        /// X,Y Position on the world grid of the Entity
        /// </summary>
        Vector2 Coordinate { get; set; }
        /// <summary>
        /// Z-Index of an entity
        /// </summary>
        float Depth { get; set; }
        /// <summary>
        /// Direction an entity is facing in radians
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Makes the terrain draw itself when needed
        /// </summary>
        /// <param name="spriteBatch">Sprite Batched used to allow the Terrain to draw itself</param>
        void Draw(SpriteBatch spriteBatch);

        void Update();
    }
}
