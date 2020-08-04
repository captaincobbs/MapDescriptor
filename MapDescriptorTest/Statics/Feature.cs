/* TODO TODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODOTODO
 - ITileObject has a new boolean to identify if something is an entity, the worldviewer draw method needs to cast to IEntity in order to update generically for tile objects after checking that it is an entity according to the interface
 - All positions for entities need to be computed synchronisly such that all positions are calculated with old values before changing to new
 */

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
    public class Feature : ITileObject
    {
        [JsonProperty("tiletype")]
        public TileObjectType TileType { get; } = TileObjectType.Feature;

        /// <inheritdoc/>
        public bool IsEntity { get; } = false;

        public Feature()
        {

        }

        public void Draw(SpriteBatch spriteBatch, Tile tile)
        {

        }
    }
}
