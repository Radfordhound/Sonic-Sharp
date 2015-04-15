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
                if (plr.animstate == Player.animationstate.pushing)
                {
                    plr.animstate = Player.animationstate.idle;
                }

                //We reset these every time we update the collision to prevent the player from "floating" in mid-air.
                plr.falling = true;
                plr.sensora = false;
                plr.sensorb = false;

                foreach (Tile tle in Level.tiles)
                {
                    //Horizontal Collision
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

                            plr.animstate = Player.animationstate.pushing;
                            plr.xsp = 0;
                        }
                    }

                    //Vertical Collision
                    for (int i = (int)plr.y; i < plr.y + 20; i++)
                    {
                        if (i >= tle.pos.Y && i <= tle.pos.Y + 16)
                        {
                            if (plr.x - 9 >= tle.pos.X && plr.x - 9 <= tle.pos.X + 16)
                            {
                                //Sensor A
                                plr.sensora = true;
                                plr.sensoratilepos = tle.pos;
                            }

                            if (plr.x + 9 >= tle.pos.X && plr.x + 9 <= tle.pos.X + 16)
                            {
                                //Sensor B
                                plr.sensorb = true;
                                plr.sensorbtilepos = tle.pos;
                            }
                        }
                    }
                }

                if ((plr.sensora || plr.sensorb) && !(plr.sensora && plr.sensorb))
                {
                    //Only one sensor is active.
                    if (plr.sensora && plr.x > plr.sensoratilepos.X + 16 && plr.xsp == 0)
                    {
                        plr.animstate = Player.animationstate.balancing;
                    }
                    else if (plr.sensorb && plr.x < plr.sensorbtilepos.X && plr.xsp == 0)
                    {
                        plr.animstate = Player.animationstate.balancing;
                    }

                    if (plr.sensora) { plr.y = plr.sensoratilepos.Y - 20; } else { plr.y = plr.sensorbtilepos.Y - 20; }

                    plr.ysp = 0;
                    plr.falling = false;
                }
                else if (plr.sensora && plr.sensorb)
                {
                    plr.ysp = 0;
                    plr.falling = false;

                    if (plr.sensoratilepos.Y < plr.sensorbtilepos.Y)
                    {
                        plr.y = plr.sensoratilepos.Y - 20;
                    }
                    else
                    {
                        plr.y = plr.sensorbtilepos.Y - 20;
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
