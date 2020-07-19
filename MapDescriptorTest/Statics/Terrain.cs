using MapDescriptorTest.Sprite;
using MapDescriptorTest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace MapDescriptorTest.Statics
{
    /// <summary>
    /// Class for each tile of terrain
    /// </summary>
    public class Terrain : ITileObject
    {
        /// <summary>
        /// Count of possible types of terrain based off of the TerrainTypes enum
        /// </summary>
        public static int TerrainTypeLength { get; private set; } = Enum.GetNames(typeof(TerrainType)).Length;
        /// <summary>
        /// Image array to be used for representing tiles
        /// </summary>
        public static Rectangle[] Image { get; set; } = new Rectangle[5];

        /// <summary>
        /// Type of terrain that the tile will get its properties from
        /// </summary>
        [JsonProperty("terraintype")]
        public TerrainType TerrainType { get; set; }

        [JsonProperty("tiletype")]
        public TileObjectType TileType { get; } = TileObjectType.Terrain;

        /// <summary>
        /// Sets passed in variables to the stored properties within the individual terrain
        /// </summary>
        /// <param name="x">X-Coordinate of the Terrain Tile</param>
        /// <param name="y">Y-Coordinate of the Terrain Tile</param>
        /// <param name="terrainType">Terrain type of the Terrain Tile</param>
        public Terrain(TerrainType terrainType)
        {
            this.TerrainType = terrainType;
            Image[0] = SpriteAtlas.Desert;
            Image[1] = SpriteAtlas.Forest;
            Image[2] = SpriteAtlas.Grasslands;
            Image[3] = SpriteAtlas.Mountain;
            Image[4] = SpriteAtlas.Ocean;
        }

        /// <summary>
        /// Makes the terrain draw itself when needed
        /// </summary>
        /// <param name="spriteBatch">Sprite Batched used to allow the Terrain to draw itself</param>
        public void Draw(SpriteBatch spriteBatch, Tile tile)
        {
            Rectangle destRect = new Rectangle(tile.XPosition, tile.YPosition, GameOptions.TileSize, GameOptions.TileSize);
            spriteBatch.Draw(TextureIndex.SpriteAtlas, destRect, Image[(int)TerrainType], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
    }
}