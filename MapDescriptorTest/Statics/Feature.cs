using MapDescriptorTest.World;
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
        public Feature()
        {

        }
    }
}
