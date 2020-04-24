using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Terrain
{
    public class TerrainGenerator
    {
        private static Random rng = new Random();
        private List<Terrain> map = new List<Terrain>();
        private static int mapSize = 20;

        /// <summary>
        /// Gets the total amount of Terrain Types in the TerrainTypes enum
        /// Goes through the X/Y grid and adds a random terrain type at the coordinates specified by the double for loop
        /// </summary>
        public void Generate()
        {
            int terrainTypeLength = Enum.GetNames(typeof(TerrainType)).Length;
            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    map.Add(new Terrain(x,y,(TerrainType)rng.Next(0,terrainTypeLength)));
                }
            }
        }

        /// <summary>
        /// Tells every tile on the map to draw itself
        /// </summary>
        /// <param name="spriteBatch">Used to pass in the SpriteBatch into the individual terrain so it can draw itself</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Terrain terrain in map)
            {
                terrain.Draw(spriteBatch);
            }
        }
    }
}
