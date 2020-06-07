using MapDescriptorTest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Entity
{
    public class Hostile : IHasEntity, ITileObject
    {
        public TileObjectType TileType { get; } = TileObjectType.Entity;

        public Rectangle Image { get; set; }

        public Vector2 Coordinate { get; set; }

        public float Depth { get; set; }

        public float Rotation { get; set; }

        public Hostile()
        {

        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
