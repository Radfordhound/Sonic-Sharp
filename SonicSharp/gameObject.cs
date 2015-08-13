using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonicSharp
{
    public class gameObject
    {
        public Vector2 pos = Vector2.Zero;
        public Sprite sprite;

        public virtual void Update()
        {
            if (sprite != null) { sprite.Animate(); }
        }

        public virtual void Draw()
        {
            Main.spriteBatch.Draw(texture: sprite.texture, position: pos, sourceRectangle: sprite.frames[sprite.currentframe]);
        }
    }
}
