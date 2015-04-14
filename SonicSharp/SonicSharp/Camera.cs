using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SonicSharp
{
    class Camera
    {
        public static Vector2 campos = new Vector2(0,0);
        public static Vector2 hborders = new Vector2(((Main.virtualscreenwidth/320)*144)/2,((Main.virtualscreenwidth/320)*160)/2);
        public static Vector2 vborders = new Vector2(((Main.virtualscreenheight / 224) * 144) / 2, ((Main.virtualscreenheight / 320) * 160) / 2);
        public static double vbordergnd = ((Main.virtualscreenheight / 224) * 116) / 2;//116
        public static int yspeed = 6;

        public static void Update()
        {
            if (Main.players.Count == 1)
            {
                //Get the borders every frame in-case the screen's size changed.
                hborders = new Vector2(((Main.virtualscreenwidth/320)*144)/2,((Main.virtualscreenwidth/320)*160)/2);
                vbordergnd = ((Main.virtualscreenheight / 224) * 116) / 2;

                int x = (int)Main.players[0].x;
                int y = (int)Main.players[0].y;

                //Horizontal camera
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

                //Vertical Camera
                if ((y - campos.Y / 2) != vbordergnd && Main.players[0].animstate != Player.animationstate.lookingup && Main.players[0].animstate != Player.animationstate.ducking)
                {
                    if (Math.Abs(Main.players[0].ysp) <= 6 && yspeed != 2){yspeed = 6;}else if (yspeed != 2){yspeed = 16;}
                    campos.Y += Math.Min((y - campos.Y / 2) - (float)vbordergnd, yspeed);

                    if (Math.Abs((y - campos.Y / 2) - (float)vbordergnd) < 2)
                    {
                        yspeed = 6;
                        Main.players[0].canlook = true;
                    }
                }
            }
        }
    }
}
