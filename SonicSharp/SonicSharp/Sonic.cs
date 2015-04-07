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
            walking.AddRange(new Texture2D[] { Content.Load<Texture2D>("Sprites\\walking1"), Content.Load<Texture2D>("Sprites\\walking2"), Content.Load<Texture2D>("Sprites\\walking3"), Content.Load<Texture2D>("Sprites\\walking4"), Content.Load<Texture2D>("Sprites\\walking5"), Content.Load<Texture2D>("Sprites\\walking6"), Content.Load<Texture2D>("Sprites\\walking7"), Content.Load<Texture2D>("Sprites\\walking8") });
            running.AddRange(new Texture2D[] { Content.Load<Texture2D>("Sprites\\running1"), Content.Load<Texture2D>("Sprites\\running2"), Content.Load<Texture2D>("Sprites\\running3"), Content.Load<Texture2D>("Sprites\\running4")});
            idle.AddRange(new Texture2D[] { Content.Load<Texture2D>("Sprites\\idle1"), Content.Load<Texture2D>("Sprites\\idle2"), Content.Load<Texture2D>("Sprites\\idle3"), Content.Load<Texture2D>("Sprites\\idle4"), Content.Load<Texture2D>("Sprites\\idle5"), Content.Load<Texture2D>("Sprites\\idle6"), Content.Load<Texture2D>("Sprites\\idle7"), Content.Load<Texture2D>("Sprites\\idle8"), Content.Load<Texture2D>("Sprites\\idle9") });
            tex = idle[0];
        }
    }
}
