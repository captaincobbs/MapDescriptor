using MapDescriptorTest;
using MapDescriptorTest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace MapDescriptorTest.World
{
    /// <summary>
    /// The map and all associated map data for the civilization game.
    /// </summary>
    public class WorldViewer
    {
        #region Members
        /// <summary>
        /// The World the World Viewer views
        /// </summary>
        World world { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new map with the given chunk information.
        /// </summary>
        /// </param>
        /// <param name="chunksPerDimension">
        /// The number of chunks for both rows and columns. Total chunks is this number squared.
        /// </param>
        public WorldViewer(World world)
        {
            this.world = world;
        }
        #endregion

        #region Chunk and Tile methods
        /// <summary>
        /// Chunks inside the rectangle formed from the given coordinates will be loaded if null.
        /// </summary>
        /// <param name="xStart">The x-position of the first Tile in the range.</param>
        /// <param name="yStart">The y-position of the first Tile in the range.</param>
        /// <param name="xEnd">The x-position of the last Tile in the range.</param>
        /// <param name="yEnd">The y-position of the last Tile in the range.</param>
        public void UpdateChunkRange(int xStart, int yStart, int xEnd, int yEnd)
        {
            int x1 = xStart / Chunk.TILES_PER_DIMENSION;
            int x2 = (int)Math.Ceiling((double)xEnd / Chunk.TILES_PER_DIMENSION);
            int y1 = yStart / Chunk.TILES_PER_DIMENSION;
            int y2 = (int)Math.Ceiling((double)yEnd / Chunk.TILES_PER_DIMENSION);
            string path = Chunk.GetChunkDirectory(world);

            for (int y = 0; y < world.MapSize; y++)
            {
                for (int x = 0; x < world.MapSize; x++)
                {
                    // Ensure Tiles are loaded in the visible range.
                    if (y >= y1 && y < y2 && x >= x1 && x < x2)
                    {
                        if (world.Chunks[x, y].Tiles == null)
                        {

                            Chunk loadedChunk = Chunk.LoadChunk(Chunk.GetChunkPath(x, y, path), world);

                            if (loadedChunk != null)
                            {
                                world.Chunks[x, y] = loadedChunk;
                            }
                        }

                        world.Chunks[x, y].Update();
                    }

                    // Save and unload Tiles otherwise.
                    else
                    {
                        world.Chunks[x,y].SaveChunk(world);
                        world.Chunks[x, y].Tiles = null;
                    }
                }
            }
        }

        /// <summary>
        /// Draws chunks within the given area.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch instance.</param>
        /// <param name="xStart">The x-position of the first Tile in the range.</param>
        /// <param name="yStart">The y-position of the first Tile in the range.</param>
        /// <param name="xEnd">The x-position of the last Tile in the range.</param>
        /// <param name="yEnd">The y-position of the last Tile in the range.</param>
        public void DrawChunkRange(SpriteBatch spriteBatch, int xStart, int yStart, int xEnd, int yEnd)
        {
            int x1 = xStart / Chunk.TILES_PER_DIMENSION;
            int x2 = (int)Math.Ceiling((double)xEnd / Chunk.TILES_PER_DIMENSION);
            int y1 = yStart / Chunk.TILES_PER_DIMENSION;
            int y2 = (int)Math.Ceiling((double)yEnd / Chunk.TILES_PER_DIMENSION);

            for (int y = 0; y < world.MapSize; y++)
            {
                for (int x = 0; x < world.MapSize; x++)
                {
                    // Ensure Tiles are loaded in the visible range.
                    if (y >= y1 && y < y2 && x >= x1 && x < x2)
                    {
                        world.Chunks[x, y].Draw(spriteBatch);
                    }
                }
            }
        }
        #endregion
    }
}
