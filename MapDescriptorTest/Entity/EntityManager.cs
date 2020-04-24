using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Entity
{
    public class EntityManager
    {
        public List<IHasEntity> Entities { get; set; } = new List<IHasEntity>();

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IHasEntity entity in Entities)
            {
                EntityProperties entityProperty = entity.GetEntityProperties();
                entityProperty.Draw(spriteBatch);
            }
        }
    }
}
