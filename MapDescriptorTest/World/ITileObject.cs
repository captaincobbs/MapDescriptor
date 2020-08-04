using Microsoft.Xna.Framework.Graphics;

namespace MapDescriptorTest.World
{
    public interface ITileObject
    {
        TileObjectType TileType { get; }

        /// <summary>
        /// Allows Tile Objects to be filtered based off of whether they are static (Static objects cannot move around the map)
        /// </summary>
        bool IsEntity { get; }

        /// <summary>
        /// Makes the tile draw itself when needed
        /// </summary>
        /// <param name="spriteBatch">Sprite Batch used to allow the tile to draw itself</param>
        ///  <param name="tile">Tile of the TileObject</param>
        void Draw(SpriteBatch spriteBatch, Tile tile);
    }
}
