using MapDescriptorTest.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// Class of the controllable player, moves around the map from user input
    /// </summary>
    public class Player : IHasEntity
    {
        /// <summary>
        /// Name of the player
        /// </summary>
        public string Name { get; set; } = "Player";
        /// <summary>
        /// Entity properties of the Player entity
        /// </summary>
        public EntityProperties Properties { get; set; }
        /// <summary>
        /// Image used to represent the player
        /// </summary>
        public static Rectangle Image { get; set; }
        public static Rectangle DiagonalImage { get; set; }
        public static Rectangle PerpendicularImage { get; set; }
        /// <summary>
        /// Coordinate of the player, update to move the player
        /// </summary>
        ///
        public Vector2 DestCoordinate { get; set; }
        private Vector2 AnimCoordinate { get; set; }
        public float Direction { get; set; }
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

        /// <summary>
        /// Constructor for a player instance
        /// </summary>
        /// <param name="Name">The players chosen name</param>
        public Player(string Name)
        {
            AnimCoordinate = new Vector2(GameOptions.MapSize / 2, GameOptions.MapSize / 2);
            DestCoordinate = AnimCoordinate;
            this.Name = Name;
            Properties = new EntityProperties(PerpendicularImage, DestCoordinate, 1, Direction);
            PerpendicularImage = SpriteAtlas.Player;
            DiagonalImage = SpriteAtlas.PlayerDiagonal;
            Image = PerpendicularImage;
        }

        /// <summary>
        /// Method to update player actions
        /// </summary>
        public void Update()
        {
            // Movement
            if (InputManager.IsActive)
            {
                HandleMovement();
            }

            // Update the animation coordinate to make character slide
            AnimCoordinate = new Vector2(
            (DestCoordinate.X - AnimCoordinate.X) * GameOptions.MovementInertiaFactor + AnimCoordinate.X,
            (DestCoordinate.Y - AnimCoordinate.Y) * GameOptions.MovementInertiaFactor + AnimCoordinate.Y
            );
            Properties = new EntityProperties(Properties.Image, new Vector2(AnimCoordinate.X, AnimCoordinate.Y), Properties.Depth, Direction);

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

        private void HandleMovement()
        {
            // Keyboard input
            if (InputManager.KeyboardState.IsKeyDown(Keys.Right) && !InputManager.KeyboardState.IsKeyDown(Keys.Left))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Right))
                {
                    Move();
                    frameCounter = 0;
                }

                HorizontalIndicatedDirection = HorizontalPlayerDirection.Right;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.Left) && !InputManager.KeyboardState.IsKeyDown(Keys.Right))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Left))
                {
                    Move();
                    frameCounter = 0;
                }

                HorizontalIndicatedDirection = HorizontalPlayerDirection.Left;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.Up) && !InputManager.KeyboardState.IsKeyDown(Keys.Down))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Up))
                {
                    Move();
                    frameCounter = 0;
                }

                VerticalIndicatedDirection = VerticalPlayerDirection.Up;
            }

            if (InputManager.KeyboardState.IsKeyDown(Keys.Down) && !InputManager.KeyboardState.IsKeyDown(Keys.Up))
            {
                if (InputManager.LastKeyboardState.IsKeyUp(Keys.Down))
                {
                    Move();
                    frameCounter = 0;
                }

                VerticalIndicatedDirection = VerticalPlayerDirection.Down;
            }

            if (frameCounter == frameLimit)
            {
                Move();
            }
        }

        private void Move()
        {
            // Convert indicated direction state to movement
            int xMove = HorizontalIndicatedDirection == HorizontalPlayerDirection.Left
                ? -1 : HorizontalIndicatedDirection == HorizontalPlayerDirection.Right
                ? 1 : 0;

            int yMove = VerticalIndicatedDirection == VerticalPlayerDirection.Down
                ? 1 : VerticalIndicatedDirection == VerticalPlayerDirection.Up
                ? -1 : 0;

            // Rotate player based off of movement
            if (xMove == 1 && yMove == 0)
            {
                Image = PerpendicularImage;
                Direction = 0f;
            }
            else if (xMove == 1 && yMove == 1)
            {
                Image = DiagonalImage;
                Direction = (float)Math.PI / 2;
            }
            else if (xMove == 0 && yMove == 1)
            {
                Image = PerpendicularImage;
                Direction = (float)Math.PI / 2;
            }
            if (xMove == -1 && yMove == 1)
            {
                Image = DiagonalImage;
                Direction = -(float)Math.PI;
            }
            else if (xMove == -1 && yMove == 0)
            {
                Image = PerpendicularImage;
                Direction = -(float)Math.PI;
            }
            else if (xMove == -1 && yMove == -1)
            {
                Image = DiagonalImage;
                Direction = -(float)Math.PI / 2;
            }
            else if (xMove == 0 && yMove == -1)
            {
                Image = PerpendicularImage;
                Direction = -(float)Math.PI / 2;
            }
            else if (xMove == 1 && yMove == -1)
            {
                Image = DiagonalImage;
                Direction = 0f;
            }

            // Player Sliding
            DestCoordinate = new Vector2(DestCoordinate.X + xMove, DestCoordinate.Y + yMove);

            // Reset movement direction
            VerticalIndicatedDirection = VerticalPlayerDirection.None;
            HorizontalIndicatedDirection = HorizontalPlayerDirection.None;
        }

        /// <summary>
        /// Gets the properties of the player 
        /// </summary>
        /// <returns>Player properties</returns>
        public EntityProperties GetEntityProperties()
        {
            Properties.Image = Image;
            return Properties;
        }
    }
}