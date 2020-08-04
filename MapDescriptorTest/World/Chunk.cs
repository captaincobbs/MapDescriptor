using MapDescriptorTest.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MapDescriptorTest.World
{
    /// <summary>
    /// Contains an array of tiles to be rendered and used actively
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// Directory where the Chunks are saved and loaded to/from
        /// </summary>
        private static string ChunkDirectory { get; set; }

        [JsonProperty("tiles")]
        public Tile[,] Tiles;

        /// <summary>
        /// This is the size of each dimension
        /// </summary>
        public static readonly int TILES_PER_DIMENSION = 20;

        [JsonIgnore]
        public bool IsDirty { get; set; }

        /// <summary>
        /// Status on if the chunk and all its contents are currently active
        /// </summary>
        [JsonIgnore]
        public bool IsLoaded { get; set; }

        /// <summary>
        /// Chunk's X Position on the Map
        /// </summary>
        [JsonIgnore]
        private int xPosition;

        /// <summary>
        /// Chunk's Y Position on the Map
        /// </summary>
        [JsonIgnore]
        private int yPosition;

        [JsonIgnore]
        private World world;

        static Chunk()
        {
            ChunkDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "MapDescriptor\\";
        }

        /// <summary>
        /// A chunk contains a 20x20 array of tiles and manages their status, a chunk saves with all of its child tiles and entities
        /// </summary>
        /// <param name="xPos">X-Position of the Chunk in the World Chunk Array</param>
        /// <param name="yPos">Y-Position of the Chunk in the World Chunk Array</param>
        public Chunk(int xPos, int yPos, World world)
        {
            IsDirty = false;
            IsLoaded = false;
            Tiles = null;
            xPosition = xPos;
            yPosition = yPos;
            this.world = world;
        }

        /// <summary>
        /// Recreates all tiles in the chunk from scratch
        /// </summary>
        private void CreateTiles()
        {
            IsDirty = true;
            IsLoaded = true;

            for (int y = 0; y < TILES_PER_DIMENSION; y++)
            {
                for (int x = 0; x < TILES_PER_DIMENSION; x++)
                {
                    Tiles[x, y] = new Tile(x * xPosition, y * yPosition);
                }
            }
        }

        /// <summary>
        /// Returns the directory to get/save chunks of the given world
        /// </summary>
        /// <param name="world">World in which chunks are going to be loaded and saved from</param>
        /// <returns>The file path where chunks are going to be loaded/saved from</returns>
        public static string GetChunkDirectory(World world)
        {
            return ChunkDirectory + world.WorldName + "\\";
        }

        /// <summary>
        /// Returns a string in the correct format to save a chunk name.
        /// </summary>
        /// <param name="chunkX">The x-coordinate of the chunk.</param>
        /// <param name="chunkY">The y-coordinate of the chunk.</param>
        public static string GetChunkPath(int chunkX, int chunkY, string baseDirectory)
        {
            return Path.Combine(baseDirectory, $"chunk{chunkX},{chunkY}.json");
        }

        /// <summary>
        /// This does not have any error checking
        /// </summary>
        /// <param name="path">Path of chunk to find the coordinate of</param>
        public static Tuple<int,int> GetCoordinatesFromPath(string path)
        {
            string[] segment = Path.GetFileNameWithoutExtension(path).Replace("chunk", "").Split(',');
            Int32.TryParse(segment[0], out int one);
            Int32.TryParse(segment[1], out int two);
            return Tuple.Create(one, two);
        }

        /// <summary>
        /// Loads a chunk from JSON as needed
        /// </summary>
        /// <param name="filePath">Path where chunk information will be saved</param>
        public static Chunk LoadChunk(string filePath, World world)
        {
            try
            {
                Chunk chunk = FileUtilities.LoadJson<Chunk>(filePath, true);
                chunk.IsLoaded = true;
                chunk.world = world;

                Tuple<int, int> tuple = GetCoordinatesFromPath(filePath);
                chunk.xPosition = tuple.Item1;
                chunk.yPosition = tuple.Item2;

                for (int x = 0; x < TILES_PER_DIMENSION; x++)
                {
                    for (int y = 0; y < TILES_PER_DIMENSION; y++)
                    {
                        chunk.Tiles[x, y].XPosition = x;
                        chunk.Tiles[x, y].YPosition = y;
                    }
                }

                return chunk;
            }
            catch (FileNotFoundException)
            {
                Tuple<int,int> tuple = GetCoordinatesFromPath(filePath);

                Chunk chunk = new Chunk(tuple.Item1, tuple.Item2, world);
                chunk.CreateTiles();

                return chunk;
            }
            catch (Exception ex)
            {
                string failure = $"The chunk at \"{filePath}\" failed to load. The program must exit.";
                System.Windows.Forms.MessageBox.Show(failure);
                LogUtilities.Log(ex, failure);

                Program.Exit();
            }

            return null;
        }

        /// <summary>
        /// Saves dirty chunks in JSON to the save directory of the passed through world
        /// </summary>
        /// <param name="world">Uses the save directory of passed through world</param>
        public void SaveChunk(World world)
        {
            // Chunks must be loaded and marked dirty to save.
            if (!IsDirty || Tiles == null)
            {
                return;
            }

            string path = GetChunkPath(xPosition, yPosition, GetChunkDirectory(world));

            try
            {
                FileUtilities.SaveFileJson(this, path, true);
                IsDirty = false;
            }
            catch (Exception ex)
            {
                string failure = $"The chunk at \"{path}\" failed to save.";
                LogUtilities.Log(ex, failure);
            }
        }

        /// <summary>
        /// Draws the chunk at the specified position. If the cached texture for the chunk isn't
        /// invalid, draws using that texture. Otherwise, draws each block in the chunk separately.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to use.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Tiles == null)
            {
                return;
            }

            for (int y = 0; y < Chunk.TILES_PER_DIMENSION; y++)
            {
                for (int x = 0; x < Chunk.TILES_PER_DIMENSION; x++)
                {
                    foreach (ITileObject tileObject in Tiles[x, y].tileObjects)
                    {
                        tileObject.Draw(spriteBatch, Tiles[x, y]);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (Tiles == null)
            {
                return; 
            }

            for (int y = 0; y < Chunk.TILES_PER_DIMENSION; y++)
            {
                for (int x = 0; x < Chunk.TILES_PER_DIMENSION; x++)
                {
                    foreach (ITileObject tileObject in Tiles[x, y].tileObjects)
                    {
                        if (tileObject.IsEntity)
                        {
                            ((IHasEntity)tileObject).Update(world);
                        }
                        
                    }
                }
            }
        }
    }
}
