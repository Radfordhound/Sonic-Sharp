using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SonicSharp
{
    class Tile
    {
        public Texture2D tex;
        public Vector2 pos;
        public HeightMap hm;

        public Tile(Vector2 pos,Texture2D tex,HeightMap hm)
        {
            this.pos = pos;
            this.tex = tex;
            this.hm = hm;
        }

        public void Draw()
        {
            Main.tilesBatch.Draw(tex,pos);
        }
    }

    class Tiles
    {
        public static void Draw()
        {
            if (Main.gs == GameState.Level)
            {
                foreach (Tile tle in Level.tiles)
                {
                    tle.Draw();
                }
            }
        }

        public static void CollideWith(Player plr)
        {
            if (Main.gs == GameState.Level)
            {
                foreach (Tile tle in Level.tiles)
                {
                    for (int i = (int)plr.x - 10; i < plr.x + 11; i++)
                    {
                        if (i >= tle.pos.X && i <= tle.pos.X+16 && plr.y+4 >= tle.pos.Y && plr.y+4 <= tle.pos.Y + 16)
                        {
                            if (tle.pos.X >= i)
                            {
                                plr.x = tle.pos.X - 11;
                            }
                            else
                            {
                                plr.x = (tle.pos.X+16) + 11;
                            }
                            plr.xsp = 0;
                        }
                    }
                }
            }
        }
    }

    class HeightMap
    {
        public List<int> hm = new List<int>();

        public HeightMap(List<int> hm)
        {
            this.hm = hm;
        }
    }
}
