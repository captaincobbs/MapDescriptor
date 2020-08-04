using MapDescriptorTest.World;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Statics
{
    public class Structure : ITileObject
    {
        [JsonProperty("tiletype")]
        public TileObjectType TileType { get; } = TileObjectType.Structure;

        /// <inheritdoc/>
        public bool IsEntity { get; } = false;

        public Structure()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Tile tile)
        {

        }
    }
}
