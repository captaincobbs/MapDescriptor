using MapDescriptorTest.World;
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
        public Structure()
        {

        }
    }
}
