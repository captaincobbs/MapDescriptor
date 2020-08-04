using Newtonsoft.Json;
using System;
using System.IO;

namespace MapDescriptorTest.World
{
    /// <summary>
    /// Is the gameworld you play on
    /// </summary>
    public class World
    {
        /// <summary>
        /// Array of all chunks in the world.
        /// </summary>
        [JsonIgnore]
        public Chunk[,] Chunks;

        /// <summary>
        /// The size of one dimension of the world in chunks. Assumes the world is square.
        /// </summary>
        [JsonProperty("mapsize")]
        public int MapSize { get; private set; }

        /// <summary>
        /// User defined name of the world
        /// </summary>
        [JsonProperty("worldname")]
        public string WorldName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mapSize">Size of the map in chunks, amount of chunks in any one dimension</param>
        public World(int mapSize)
        {
            MapSize = mapSize;
            Chunks = new Chunk[mapSize, mapSize];

            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    // All the chunks are initialized as empty to avoid lag
                    Chunks[x, y] = new Chunk(x, y, this);
                }
            }
        }

        ///
        [JsonConstructor]
        private World()
        {

        }

        /// <summary>
        /// Returns requested tile and loads the containing chunk if necessary.
        /// Does not check for valid x or y positions.
        /// Returns null if <paramref name="forceLoadChunk"/> is false and chunk is not loaded
        /// </summary>
        /// <param name="xPos">X position of requested tile</param>
        /// <param name="yPos">Y position of requested tile</param>
        /// <param name="forceLoadChunk">Loads the chunk if not available synchronously</param>
        public Tile GetTile(int xPos, int yPos, bool forceLoadChunk)
        {
            int worldSizeInTiles = this.MapSize * Chunk.TILES_PER_DIMENSION;

            if (xPos < worldSizeInTiles && xPos > 0 && yPos < worldSizeInTiles && yPos > 0)
            {
                int chunkX = (int)Math.Ceiling((double)xPos / Chunk.TILES_PER_DIMENSION);
                int chunkY = (int)Math.Ceiling((double)yPos / Chunk.TILES_PER_DIMENSION);
                Chunk chunk = Chunks[chunkX, chunkY];

                if (!chunk.IsLoaded)
                {
                    if (forceLoadChunk)
                    {
                        Chunks[chunkX, chunkY] = Chunk.LoadChunk(Chunk.GetChunkPath(xPos, yPos, Chunk.GetChunkDirectory(this)), this);
                    }
                    else
                    {
                        return null;
                    }
                }

                return Chunks[chunkX, chunkY].Tiles[xPos % chunkX, yPos % chunkY];
            }

            return null;
        }

        /// <summary>
        /// Gets Chunk from the coordinate of a Tile
        /// </summary>
        /// <param name="xPos">X Coordinate of a Tile relative to the World</param>
        /// <param name="yPos">Y Coordinate of a Tile relative to the World</param>
        /// <returns></returns>
        public Chunk GetChunk(int xPos, int yPos)
        {
            int worldSizeInTiles = this.MapSize * Chunk.TILES_PER_DIMENSION;

            if (xPos < worldSizeInTiles && xPos > 0 && yPos < worldSizeInTiles && yPos > 0)
            {
                int chunkX = (int)Math.Ceiling((double)xPos / Chunk.TILES_PER_DIMENSION);
                int chunkY = (int)Math.Ceiling((double)yPos / Chunk.TILES_PER_DIMENSION);

                return Chunks[chunkX, chunkY];
            }

            return null;
        }

        /// <summary>
        /// Saves the world and its chunks.
        /// </summary>
        public void Save()
        {
            // Map is always square
            int mapSize = Chunks.GetLength(0);

            // Save all chunks.
            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    Chunks[x,y].SaveChunk(this);
                }
            }

            string path = Path.Combine(Chunk.GetChunkDirectory(this), "world.json");
            FileUtilities.SaveFileJson(this, path);
        }

        /// <summary>
        /// Loads the world.
        /// Will return null if file loading is cancelled or fails
        /// </summary>
        public static World Load()
        {
            World world = FileUtilities.PromptLoadJson<World>("Select a world file");

            if (world == null)
            {
                return null;
            }

            world.Chunks = new Chunk[world.MapSize, world.MapSize];

            for (int y = 0; y < world.MapSize; y++)
            {
                for (int x = 0; x < world.MapSize; x++)
                {
                    world.Chunks[x, y] = new Chunk(x, y, world);
                }
            }

            return world;
        }

    }
}
