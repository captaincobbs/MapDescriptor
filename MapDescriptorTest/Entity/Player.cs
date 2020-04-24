using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
            Coordinate = new Vector2(0, 0);
            this.Name = Name;
            Properties = new EntityProperties(Image, Coordinate, 1);
        }

        /// <summary>
        /// Loads all the map tile images into the Terrain Image array
        /// </summary>
        /// <param name="contentManager">ContentManager passed into the Terrain class to allow it to load textures</param>
        public static void LoadContent(ContentManager contentManager)
        {
            Image = contentManager.Load<Texture2D>("Player");
        }

        public EntityProperties GetEntityProperties()
        {
            Properties.Image = Image;
            return Properties;
        }
    }
}
