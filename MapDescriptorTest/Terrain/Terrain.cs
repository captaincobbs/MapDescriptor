using MapDescriptorTest.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MapDescriptorTest.Terrain
{
    /// <summary>
    /// Class for each tile of terrain
    /// </summary>
    public class Terrain
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
        /// X,Y location of the tile on the world grid
        /// </summary>
        public Vector2 Coordinate { get; set; }
        /// <summary>
        /// Type of terrain that the tile will get its properties from
        /// </summary>
        public TerrainType TerrainType { get; set; }

        /// <summary>
        /// Sets passed in variables to the stored properties within the individual terrain
        /// </summary>
        /// <param name="x">X-Coordinate of the Terrain Tile</param>
        /// <param name="y">Y-Coordinate of the Terrain Tile</param>
        /// <param name="terrainType">Terrain type of the Terrain Tile</param>
        public Terrain(float x, float y, TerrainType terrainType)
        {
            Coordinate = new Vector2(x * GameOptions.TileSize, y * GameOptions.TileSize);
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
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)Coordinate.X, (int)Coordinate.Y, GameOptions.TileSize, GameOptions.TileSize);
            spriteBatch.Draw(TextureIndex.SpriteAtlas, destRect, Image[(int)TerrainType], Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        }
    }
}