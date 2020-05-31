using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Sprite
{
    public class TextureIndex
    {
        public static Texture2D SpriteAtlas;

        public static void LoadContent(ContentManager content)
        {
            SpriteAtlas = content.Load<Texture2D>("Textures");
        }
    }
}
