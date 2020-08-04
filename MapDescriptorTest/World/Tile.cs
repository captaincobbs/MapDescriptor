using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapDescriptorTest.World
{
    /// <summary>
    /// A square that contains all entities and terrain within it
    /// </summary>
    public class Tile
    {
        [JsonProperty("a")]
        public List<ITileObject> tileObjects { get; set; }

        /// <summary>
        /// X Position of the Tile relative to the world
        /// </summary>
        [JsonIgnore]
        public int XPosition { get; set; }

        /// <summary>
        /// Y Position of the Tile relative to the world
        /// </summary>
        [JsonIgnore]
        public int YPosition { get; set; }

        public Tile(int x, int y)
        {
            XPosition = x;
            YPosition = y;
            tileObjects = new List<ITileObject>();
        }
    }
}
