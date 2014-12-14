using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
    public class Sprite
    {
        private Texture2D texture;
        private Rectangle boundingBox;

        public Sprite(Texture2D texture, Rectangle boundingBox)
        {
            if (texture == null) throw new ArgumentNullException("texture", "texture must be valid");
            this.texture = texture;
            this.boundingBox = boundingBox;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color tintColor)
        {
            spriteBatch.Draw(texture,
                             new Rectangle((int)location.X, (int)location.Y, boundingBox.Width, boundingBox.Height),
                             boundingBox,
                             Color.White);
        }
    }
}
