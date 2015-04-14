using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SonicSharp
{
    class Camera
    {
        public static Vector2 campos = new Vector2(0,100);
        public static Vector2 hborders = new Vector2(((Main.virtualscreenwidth/320)*144)/2,((Main.virtualscreenwidth/320)*160)/2);

        public static void Update()
        {
            if (Main.players.Count == 1)
            {
                //LB: 216
                //RB: 240

                hborders = new Vector2(((Main.virtualscreenwidth/320)*144)/2,((Main.virtualscreenwidth/320)*160)/2);

                int x = (int)Main.players[0].x;
                int y = (int)Main.players[0].y;

                if(x >= (campos.X/2)+hborders.Y)
                {
                    if (((x - campos.X/2) - hborders.Y) <= 0)
                    {
                        campos.X++;
                    }
                    else if (((x - campos.X/2) - hborders.Y) < 16)
                    {
                        campos.X += (x - campos.X/2) - hborders.Y;
                    }
                    else
                    {
                        campos.X += 16;
                    }
                }
                else if (x <= (campos.X/2)+hborders.X)
                {
                    if (((x - campos.X / 2) - hborders.X) >= 0)
                    {
                        campos.X--;
                    }
                    else if (((x - campos.X / 2) - hborders.X) > -16)
                    {
                        campos.X += (x - campos.X / 2) - hborders.X;
                    }
                    else
                    {
                        campos.X -= 16;
                    }
                }
            }
        }
    }
}
