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

        public Sonic(int pid, Point pos)
        {
            this.id = pid;
            this.x = pos.X;
            this.y = pos.Y;
        }

        public Sonic(int pid,int x, int y)
        {
            this.id = pid;
            this.x = x;
            this.y = y;
        }

        public void LoadContent(ContentManager Content)
        {
            walking.AddRange(new Texture2D[] { Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking1"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking2"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking3"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking4"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking5"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking6"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking7"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\walking8") });
            running.AddRange(new Texture2D[] { Content.Load<Texture2D>("Sprites\\Player\\Sonic\\running1"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\running2"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\running3"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\running4")});
            idle.AddRange(new Texture2D[] { Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle1"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle2"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle3"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle4"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle5"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle6"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle7"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle8"), Content.Load<Texture2D>("Sprites\\Player\\Sonic\\idle9") });
            tex = idle[0];
        }
    }
}
