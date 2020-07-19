using Microsoft.Xna.Framework;
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
        [JsonProperty("tiles")]
        public Tile[,] Tiles;

        /// <summary>
        /// This is the size of each dimension
        /// </summary>
        public static readonly int TILES_PER_DIMENSION = 20;

        [JsonIgnore]
        private bool isDirty;

        [JsonIgnore]
        public bool IsLoaded { get; set; }

        [JsonIgnore]
        private int xPosition;

        [JsonIgnore]
        private int yPosition;

        public Chunk(int xPos, int yPos)
        {
            isDirty = false;
            IsLoaded = false;
            Tiles = null;
            xPosition = xPos;
            yPosition = yPos;
        }

        private void CreateTiles()
        {
            isDirty = true;
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
        /// Returns a string in the correct format to save a chunk name.
        /// </summary>
        /// <param name="chunkX">The x-coordinate of the chunk.</param>
        /// <param name="chunkY">The y-coordinate of the chunk.</param>
        public static string GetChunkPath(int chunkX, int chunkY, string baseDirectory)
        {
            return System.IO.Path.Combine(baseDirectory, $"chunk{chunkX},{chunkY}.json");
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

        public static Chunk LoadChunk(string filePath)
        {
            try
            {
                Chunk chunk = FileUtilities.LoadJson<Chunk>(filePath, true);
                chunk.IsLoaded = true;
                return chunk;
            }
            catch (FileNotFoundException)
            {
                Tuple<int,int> tuple = GetCoordinatesFromPath(filePath);

                Chunk chunk = new Chunk(tuple.Item1, tuple.Item2);
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

        public void SaveChunk(World world)
        {
            // Chunks must be loaded and marked dirty to save.
            if (!isDirty || Tiles == null)
            {
                return;
            }

            string path = GetChunkPath(xPosition, yPosition, world.SaveDirectory);

            try
            {
                FileUtilities.SaveFileJson(this, path, true);
                isDirty = false;
            }
            catch (Exception ex)
            {
                string failure = $"The chunk at \"{path}\" failed to save.";
                LogUtilities.Log(ex, failure);
            }
        }
    }
}
