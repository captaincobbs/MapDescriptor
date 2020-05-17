using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Entity
{
    public class Player : IHasEntity
    {
        public string Name { get; set; } = "Player";
        public EntityProperties Properties { get; set; }
        public static Texture2D Image { get; set; }
        public Vector2 Coordinate { get; set; }

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

        public void Update()
        {
            MovePlayer();
        }

        private void MovePlayer()
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

        public EntityProperties GetEntityProperties()
        {
            Properties.Image = Image;
            return Properties;
        }
    }
}
