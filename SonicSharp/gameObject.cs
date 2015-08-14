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
        private Sprite Sprite;
        public bool active = true;

        public Sprite sprite
        {
            get { return Sprite; }
            set { if (Sprite != value) { value.curfrm = 0; value.currentframe = 0; } Sprite = value; }
        }

        public virtual void Update()
        {
            if (sprite != null && active) { sprite.Animate(); }
        }

        public virtual void Draw()
        {
            if (active) { Main.spriteBatch.Draw(texture: sprite.texture, position: pos, sourceRectangle: sprite.frames[sprite.currentframe]); }
        }
    }
}
