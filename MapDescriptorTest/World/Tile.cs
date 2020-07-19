using Newtonsoft.Json;
using System.Collections.Generic;

namespace MapDescriptorTest.World
{
    /// <summary>
    /// A square that contains all entities and terrain within it
    /// </summary>
    public class Tile
    {
        [JsonProperty("tileObjects")]
        public List<ITileObject> tileObjects { get; set; }

        [JsonIgnore]
        public int XPosition { get; }

        [JsonIgnore]
        public int YPosition { get; }

        public Tile(int x, int y)
        {
            XPosition = x;
            YPosition = y;
            tileObjects = new List<ITileObject>();
        }
    }
}
