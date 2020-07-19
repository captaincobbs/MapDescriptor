using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MapDescriptorTest.Statics
{
    /// <summary>
    /// Generates the terrain
    /// </summary>
    public class TerrainGenerator
    {
        private static Random rng = new Random();
        private List<Terrain> map = new List<Terrain>();

        /// <summary>
        /// Gets the total amount of Terrain Types in the TerrainTypes enum
        /// Goes through the X/Y grid and adds a random terrain type at the coordinates specified by the double for loop
        /// </summary>
        public void Generate()
        {
            // Map Generation
            for (int y = 0; y < GameOptions.MapSize; y++)
            {
                for (int x = 0; x < GameOptions.MapSize; x++)
                {
                    map.Add(new Terrain((TerrainType)rng.Next(0,Terrain.TerrainTypeLength)));
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
