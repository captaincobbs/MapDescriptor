using MapDescriptorTest.Sprite;
using MapDescriptorTest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;

namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// Class of the controllable player, moves around the map from user input
    /// </summary>
    public class Player : IHasEntity, ITileObject
    {
        /// <summary>
        /// Name of the player, inputted by user
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = "Player";

        /// <summary>
        /// Image used to represent the player
        /// </summary>
        public Rectangle Image { get; set; }

        /// <summary>
        /// Images used by the player, arranged in a 3x3 array. 1, 1 is the center, so X + 1 would be the right-facing sprite, and Y + 1 would be the up-facing sprite
        /// </summary>
        public static Rectangle DiagonalImage { get; set; }

        [JsonProperty("depth")]
        public float Depth { get; set; }

        /// <summary>
        /// Coordinate of the player, update to move the player
        /// </summary>
        [JsonIgnore]
        public Tile ContainingTile { get; set; }

        public TileObjectType TileType { get; } = TileObjectType.Player;

        private Vector2 AnimCoordinate { get; set; }

        [JsonProperty("direction")]
        public float Rotation { get; set; }
        private int frameCounter = 0;
        private int frameLimit = 8;

        // Player Direction
        /// <summary>
        /// Vertical movement states for the player
        /// </summary>
        public enum VerticalPlayerDirection { None, Up, Down }
        /// <summary>
        /// Horizontal movement states for the player
        /// </summary>
        public enum HorizontalPlayerDirection { None, Right, Left }
        /// <summary>
        /// Vertical movement state the player is planning on moving to
        /// </summary>
        public static VerticalPlayerDirection VerticalIndicatedDirection = VerticalPlayerDirection.None;
        /// <summary>
        /// Horizontal movement state the player is planning on moving to
        /// </summary>
        public static HorizontalPlayerDirection HorizontalIndicatedDirection = HorizontalPlayerDirection.None;

        /// <inheritdoc/>
        public bool IsEntity { get; } = true;

        /// <summary>
        /// Constructor for a player instance
        /// </summary>
        /// <param name="Name">The players chosen name</param>
        public Player(string Name)
        {
            AnimCoordinate = new Vector2(GameOptions.MapSize / 2, GameOptions.MapSize / 2);
            this.Name = Name;
            Depth = 1f;
        }

        /// <summary>
        /// Method to update player actions
        /// </summary>
        public void Update(World.World world)
        {
            // Movement
            if (InputManager.IsActive)
            {
                HandleMovement(world);
            }

            // Update the animation coordinate to make character slide
            AnimCoordinate = new Vector2(
            (ContainingTile.XPosition - AnimCoordinate.X) * GameOptions.MovementInertiaFactor + AnimCoordinate.X,
            (ContainingTile.YPosition - AnimCoordinate.Y) * GameOptions.MovementInertiaFactor + AnimCoordinate.Y
            );

            // Frame Counting
            if (frameCounter == frameLimit)
            {
                frameCounter = 0;
            }
            else
            {
                frameCounter++;
            }
        }

        private void HandleMovement(World.World world)
        {
            // Keyboard input
            if (InputManager.KeyboardState.IsKeyDown(Keys.Right) && !InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Right))
                {
                    Move(world);
                    frameCounter = 0;
                }

                HorizontalIndicatedDirection = HorizontalPlayerDirection.Right;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.Left) && !InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Left))
                {
                    Move(world);
                    frameCounter = 0;
                }

                HorizontalIndicatedDirection = HorizontalPlayerDirection.Left;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.Up) && !InputManager.KeyboardState.IsKeyDown(Keys.Down))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Up))
                {
                    Move(world);
                    frameCounter = 0;
                }

                VerticalIndicatedDirection = VerticalPlayerDirection.Up;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.Down) && !InputManager.KeyboardState.IsKeyDown(Keys.Up))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Down))
                {
                    Move(world);
                    frameCounter = 0;
                }

                VerticalIndicatedDirection = VerticalPlayerDirection.Down;
            }

            if (frameCounter == frameLimit)
            {
                Move(world);
            }
        }

        private void Move(World.World world)
        {
            // Convert indicated direction state to movement, Right = Positive X, Down = Positive Y
            int xMove = HorizontalIndicatedDirection == HorizontalPlayerDirection.Left
                ? -1 : HorizontalIndicatedDirection == HorizontalPlayerDirection.Right
                ? 1 : 0;

            int yMove = VerticalIndicatedDirection == VerticalPlayerDirection.Down
                ? 1 : VerticalIndicatedDirection == VerticalPlayerDirection.Up
                ? -1 : 0;

            // Change rotational sprite based off of player movement
            // Right
            if (xMove == 1 && yMove == 0)
            {
                Image = SpriteAtlas.PlayerRight;
            }
            // Down Right
            else if (xMove == 1 && yMove == 1)
            {
                Image = SpriteAtlas.PlayerBottomRight;
            }
            // Down
            else if (xMove == 0 && yMove == 1)
            {
                Image = SpriteAtlas.PlayerDown;
            }
            // Down Left
            if (xMove == -1 && yMove == 1)
            {
                Image = SpriteAtlas.PlayerBottomLeft;
            }
            // Left
            else if (xMove == -1 && yMove == 0)
            {
                Image = SpriteAtlas.PlayerLeft;
            }
            // Up Left
            else if (xMove == -1 && yMove == -1)
            {
                Image = SpriteAtlas.PlayerTopLeft;
            }
            // Up
            else if (xMove == 0 && yMove == -1)
            {
                Image = SpriteAtlas.PlayerUp;
            }
            // Up Right
            else if (xMove == 1 && yMove == -1)
            {
                Image = SpriteAtlas.PlayerTopRight;
            }

            // Player Sliding
            Tile newTile = world.GetTile(ContainingTile.XPosition + xMove, ContainingTile.YPosition + yMove, false);
            if (newTile != null)
            {
                world.GetChunk(newTile.XPosition, newTile.YPosition).IsDirty = true;
                world.GetChunk(ContainingTile.XPosition, ContainingTile.YPosition).IsDirty = true;

                newTile.tileObjects.Add(this);
                ContainingTile.tileObjects.Remove(this);

                ContainingTile = newTile;
            }

            // Reset movement direction
            VerticalIndicatedDirection = VerticalPlayerDirection.None;
            HorizontalIndicatedDirection = HorizontalPlayerDirection.None;
        }

        public void Draw(SpriteBatch spriteBatch, Tile tile)
        {
            Rectangle destRect = new Rectangle(
               (int)(AnimCoordinate.X * GameOptions.TileSize) - (GameOptions.TileSize / 2),
               (int)(AnimCoordinate.Y * GameOptions.TileSize) - (GameOptions.TileSize / 2),
               GameOptions.TileSize,
               GameOptions.TileSize);

            spriteBatch.Draw(TextureIndex.SpriteAtlas, destRect, Image, Color.White, Rotation, new Vector2(GameOptions.TileSize / 2, GameOptions.TileSize / 2), SpriteEffects.None, Depth);
        }

    }
}