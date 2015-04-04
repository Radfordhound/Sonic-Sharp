using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SonicSharp
{
    class Sonic:Player
    {
        public Sonic(int pid)
        {
            this.id = pid;
        }

        public void LoadContent(ContentManager Content)
        {
            running = Content.Load<Texture2D>("Sprites\\running1");
        }
    }
}
