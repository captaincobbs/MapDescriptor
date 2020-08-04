using MapDescriptorTest.Entity;
using MapDescriptorTest.World;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MapDescriptorTest.Statics
{
    /// <summary>
    /// Generates the terrain
    /// </summary>
    public static class WorldGenerator
    {
        private static readonly Random rng = new Random();

        /// <summary>
        /// Gets the total amount of Terrain Types in the TerrainTypes enum
        /// Goes through the X/Y grid and adds a random terrain type at the coordinates specified by the double for loop
        /// </summary>
        /// <param name="chunkSize">Size of a dimension of the square world 2D array in chunks</param>
        public static World.World Generate(string worldName, Player player)
        {
            World.World world = new World.World(GameOptions.MapSize);
            world.WorldName = worldName;
            world.Chunks = new Chunk[GameOptions.MapSize, GameOptions.MapSize];

            for (int chunkY = 0; chunkY < GameOptions.MapSize; chunkY++)
            {
                for (int chunkX = 0; chunkX < GameOptions.MapSize; chunkX++)
                {
                    world.Chunks[chunkX, chunkY] = new Chunk(chunkX, chunkY, world);
                    world.Chunks[chunkX, chunkY].IsLoaded = true;
                    world.Chunks[chunkX, chunkY].Tiles = new Tile[Chunk.TILES_PER_DIMENSION, Chunk.TILES_PER_DIMENSION];

                    for (int tileX = 0; tileX < Chunk.TILES_PER_DIMENSION; tileX++)
                    {
                        for (int tileY = 0; tileY < Chunk.TILES_PER_DIMENSION; tileY++)
                        {
                            Tile tile = new Tile(tileX * chunkX, tileY * chunkY);
                            world.Chunks[chunkX, chunkY].Tiles[tileX, tileY] = tile;
                            tile.tileObjects.Add(new Terrain((TerrainType)rng.Next(0, Terrain.TerrainTypeLength)));
                            
                            // Place player in ~about~ the center of the world
                            if (chunkX == world.MapSize / 2 &&
                                chunkY == world.MapSize / 2 &&
                                tileX == Chunk.TILES_PER_DIMENSION / 2 &&
                                tileY == Chunk.TILES_PER_DIMENSION / 2)
                            {
                                tile.tileObjects.Add(player);
                                player.ContainingTile = tile;
                            }
                        }
                    }

                    world.Chunks[chunkX, chunkY].IsDirty = true;
                    world.Chunks[chunkX, chunkY].SaveChunk(world);
                    world.Chunks[chunkX, chunkY].Tiles = null;
                }
            }

            return world;
        }
    }
}
