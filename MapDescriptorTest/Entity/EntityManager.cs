using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// A manager that constantly updates and draws changes in entities
    /// </summary>
    public class EntityManager
    {
        /// <summary>
        /// Collection of all entities
        /// </summary>
        public List<IHasEntity> Entities { get; set; } = new List<IHasEntity>();

        /// <summary>
        /// Goes through every entity and redraws them as needed
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch to add updated sprites onto</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IHasEntity entity in Entities)
            {
                EntityProperties entityProperty = entity.GetEntityProperties();
                entityProperty.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Updates all entities every frame
        /// </summary>
        public void Update()
        {
            foreach (IHasEntity entity in Entities)
            {
                entity.Update();
            }
        }
    }
}
