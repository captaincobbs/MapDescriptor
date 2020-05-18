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
        public static Texture2D Image { get; set; }
        /// <summary>
        /// Starting coordinate of the player
        /// </summary>
        public Vector2 Coordinate { get; set; }

        /// <summary>
        /// Constructor for a player instance
        /// </summary>
        /// <param name="Name">The players chosen name</param>
        public Player(string Name)
        {
            Coordinate = new Vector2(GameOptions.MapSize / 2, GameOptions.MapSize / 2);
            this.Name = Name;
            Properties = new EntityProperties(Image, Coordinate, 1, 0f);
        }

        /// <summary>
        /// Loads all the map tile images into the Terrain Image array
        /// </summary>
        /// <param name="contentManager">ContentManager passed into the Terrain class to allow it to load textures</param>
        public static void LoadContent(ContentManager contentManager)
        {
            Image = contentManager.Load<Texture2D>("Player");
        }

        /// <summary>
        /// Method to update player actions
        /// </summary>
        public void Update()
        {
            if (InputManager.IsActive)
            {
                HandleMovement();
            }
            
        }

        private void HandleMovement()
        {
            if (GameOptions.FrameCounter == 0)
            {
                if (InputManager.KeyboardState.IsKeyDown(Keys.Right))
                {
                    Properties = new EntityProperties(Properties.Image, new Vector2(Properties.Coordinate.X + 1, Properties.Coordinate.Y), Properties.Depth, 0f);
                }

                if (InputManager.KeyboardState.IsKeyDown(Keys.Left))
                {
                    Properties = new EntityProperties(Properties.Image, new Vector2(Properties.Coordinate.X - 1, Properties.Coordinate.Y), Properties.Depth, -(float)Math.PI);
                }

                if (InputManager.KeyboardState.IsKeyDown(Keys.Up))
                {
                    Properties = new EntityProperties(Properties.Image, new Vector2(Properties.Coordinate.X, Properties.Coordinate.Y - 1), Properties.Depth, -(float)Math.PI / 2);
                }

                if (InputManager.KeyboardState.IsKeyDown(Keys.Down))
                {
                    Properties = new EntityProperties(Properties.Image, new Vector2(Properties.Coordinate.X, Properties.Coordinate.Y + 1), Properties.Depth, (float)Math.PI / 2);
                }
            }
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
