using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonicSharp
{
    public static class Camera
    {
        public static Vector2 pos = Vector2.Zero;
        public static float[] hborders = new float[2], vborders = new float[2];

        public static void Update()
        {
            //TODO: Split-screen camera

            hborders[0] = ((Main.virtualscreenwidth / 320) * 144) / Main.scalemodifier;
            hborders[1] = ((Main.virtualscreenwidth / 320) * 160) / Main.scalemodifier;

            vborders[0] = ((Main.virtualscreenheight / 224) * 64) / Main.scalemodifier;
            vborders[1] = ((Main.virtualscreenheight / 224) * 128) / Main.scalemodifier;

            if (Main.players.Count > 0 && Main.players[0].sprite != Main.players[0].deathsprite)
            {
                //Horizontal Camera
                if (Main.players[0].pos.X >= (pos.X/Main.scalemodifier) + hborders[1])
                {
                    if (((Main.players[0].pos.X - pos.X/Main.scalemodifier) - hborders[1]) <= 0)
                    {
                        pos.X++;
                    }
                    else if (((Main.players[0].pos.X - pos.X/Main.scalemodifier) - hborders[1]) < 16)
                    {
                        pos.X += (Main.players[0].pos.X - pos.X/Main.scalemodifier) - hborders[1];
                    }
                    else pos.X += 16;
                }
                else if (Main.players[0].pos.X <= (pos.X/Main.scalemodifier) + hborders[0])
                {
                    if (((Main.players[0].pos.X - pos.X/Main.scalemodifier) - hborders[0]) >= 0)
                    {
                        pos.X--;
                    }
                    else if (((Main.players[0].pos.X - pos.X/Main.scalemodifier) - hborders[0]) > -16)
                    {
                        pos.X += (Main.players[0].pos.X - pos.X/Main.scalemodifier) - hborders[0];
                    }
                    else pos.X -= 16;
                }

                //Vertical Camera
                if (Main.players[0].falling)
                {
                    if (Main.players[0].pos.Y >= (pos.Y / Main.scalemodifier) + vborders[1])
                    {
                        if (((Main.players[0].pos.Y - pos.Y / Main.scalemodifier) - vborders[1]) <= 0)
                        {
                            pos.Y++;
                        }
                        else if (((Main.players[0].pos.Y - pos.Y / Main.scalemodifier) - vborders[1]) < 16)
                        {
                            pos.Y += (Main.players[0].pos.Y - pos.Y / Main.scalemodifier) - vborders[1];
                        }
                        else pos.Y += 16;
                    }
                    else if (Main.players[0].pos.Y <= (pos.Y / Main.scalemodifier) + vborders[0])
                    {
                        if (((Main.players[0].pos.Y - pos.Y / Main.scalemodifier) - vborders[0]) >= 0)
                        {
                            pos.Y--;
                        }
                        else if (((Main.players[0].pos.Y - pos.Y / Main.scalemodifier) - vborders[0]) > -16)
                        {
                            pos.Y += (Main.players[0].pos.Y - pos.Y / Main.scalemodifier) - vborders[0];
                        }
                        else pos.Y -= 16;
                    }
                }
                else if (Main.players[0].pos.Y * Main.scalemodifier - pos.Y != 96 * Main.scalemodifier)
                {
                    float dif = 96 * Main.scalemodifier - (Main.players[0].pos.Y * Main.scalemodifier - pos.Y);
                    pos.Y += (Math.Abs(dif) <= 6) ? -dif : 6 * -Math.Sign(dif);
                }
            }
        }
    }
}
