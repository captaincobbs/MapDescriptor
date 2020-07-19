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
        [JsonIgnore]
        public Chunk[,] Chunks;

        [JsonIgnore]
        public string SaveDirectory { get; private set; } = null;

        [JsonProperty("mapsize")]
        public int MapSize { get; private set; }

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
                    Chunks[x, y] = new Chunk(x, y);
                }
            }
        }

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
            int chunkX = (int)Math.Ceiling((double)xPos / Chunk.TILES_PER_DIMENSION);
            int chunkY = (int)Math.Ceiling((double)yPos / Chunk.TILES_PER_DIMENSION);
            Chunk chunk = Chunks[chunkX, chunkY];

            if (!chunk.IsLoaded)
            {
                if(forceLoadChunk)
                {
                    Chunks[chunkX, chunkY] = Chunk.LoadChunk(Chunk.GetChunkPath(xPos, yPos, SaveDirectory));
                }
                else
                {
                    return null;
                }
            }

            return Chunks[chunkX,chunkY].Tiles[xPos % chunkX, yPos % chunkY];
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

            string path = Path.Combine(SaveDirectory, "world.json");
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
                    world.Chunks[x, y] = new Chunk(x, y);
                }
            }

            return world;
        }
    }
}
