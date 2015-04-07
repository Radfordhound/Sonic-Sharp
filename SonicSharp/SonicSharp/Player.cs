using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SonicSharp
{
    public class Player
    {
        //Variables
        public int id; //The id of the player. For example, 1 for player 1, 2 for player 2, etc.
        public double x = 0;
        public double y = 0;
        public int currentframe = 0;
        public Vector2 origin = new Vector2(0, 0);
        public float angle = 0;
        public animationstate animstate = animationstate.idle;
        public bool left = false;
        public double xsp; //Player's X Speed
        public double ysp; //Player's Y Speed

        //Constants (We declare these as variables to allow for more flexibility.)
        public double acc = 0.046875;
        public double dec = 0.5;
        public double frc = 0.046875;
        public int top = 6;

        //Textures
        public List<Texture2D> idle = new List<Texture2D>();
        public List<Texture2D> walking = new List<Texture2D>();
        public List<Texture2D> running = new List<Texture2D>();
        public Texture2D tex;

        public void Update(GameTime gameTime)
        {
            //angle = MathHelper.ToRadians(-30); 

            KeyboardState kbst = Keyboard.GetState();

            if (kbst.IsKeyDown(Keys.Left) || kbst.IsKeyDown(Keys.A))
            {
                if (xsp > 0)
                {
                    xsp -= dec;
                }
                else if (xsp > -top)
                {
                    xsp = xsp - acc;
                }
                else
                {
                    xsp = -top;
                }
                left = true;
            }
            else if (kbst.IsKeyDown(Keys.Right) || kbst.IsKeyDown(Keys.D))
            {
                if (xsp < 0)
                {
                    xsp += dec;
                }
                else if (xsp < top)
                {
                    xsp = xsp + acc;
                }
                else
                {
                    xsp = top;
                }
                left = false;
            }
            else
            {
                xsp = xsp - Math.Min(Math.Abs(xsp), frc) * Math.Sign(xsp);
            }

            if (this.GetType() == typeof(Sonic))
            {
                //Custom Sonic-only logic goes here.
            }

            x += xsp;
        }

        public void Draw()
        {
            if (!left)
            {
                Main.spriteBatch.Draw(tex, new Vector2((float)x, 20), null, null, origin, angle, null, null, SpriteEffects.None);
            }
            else
            {
                Main.spriteBatch.Draw(tex, new Vector2((float)x, 20), null, null, origin, angle, null, null, SpriteEffects.FlipHorizontally);
            }

            Animate();

            if (this.GetType() == typeof(Sonic))
            {
                //Custom Sonic-only logic goes here.
            }
        }

        public void GetAnimState()
        {
            if (xsp >= 6 || xsp <= -6)
            {
                if (animstate != animationstate.running)
                {
                    animstate = animationstate.running;
                    currentframe = 0;
                }
            }
            else if (xsp != 0)
            {
                if (animstate != animationstate.walking)
                {
                    animstate = animationstate.walking;
                    currentframe = 0;
                }
            }
            else
            {
                if (animstate != animationstate.idle)
                {
                    animstate = animationstate.idle;
                    currentframe = 0;
                }
            }
        }

        public void GetFrames()
        {
            //
        }

        public void Animate()
        {
            GetAnimState();

            if (animstate == animationstate.idle)
            {
                if (currentframe < 300)
                {
                    tex = idle[0];
                }
                else if (currentframe >= 300 && currentframe < 306)
                {
                    tex = idle[1];
                }
                else if (currentframe >= 306 && currentframe < 300)
                {
                    tex = idle[2];
                }
                else if (currentframe >= 317 && currentframe < 330)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 330 && currentframe < 342)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 342 && currentframe < 354)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 354 && currentframe < 366)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 366 && currentframe < 378)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 378 && currentframe < 390)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 390 && currentframe < 402)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 402 && currentframe < 414)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 414 && currentframe < 426)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 426 && currentframe < 438)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 438 && currentframe < 450)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 450 && currentframe < 462)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 462 && currentframe < 474)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 474 && currentframe < 486)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 486 && currentframe < 498)
                {
                    tex = idle[3];
                }
                else if (currentframe >= 498 && currentframe < 510)
                {
                    tex = idle[4];
                }
                else if (currentframe >= 510 && currentframe < 546)
                {
                    tex = idle[5];
                }
                else if (currentframe >= 546 && currentframe < 582)
                {
                    tex = idle[6];
                }
                else if (currentframe >= 582 && currentframe < 588)
                {
                    tex = idle[7];
                }
                else if (currentframe >= 588 && currentframe < 624)
                {
                    tex = idle[8];
                }
                else if (currentframe >= 624 && currentframe < 635)
                {
                    tex = idle[7];
                }
                else
                {
                    tex = idle[3];
                    currentframe = 317;
                }
            }
            if (animstate == animationstate.walking)
            {
                if (currentframe < Math.Max(8 - Math.Abs(xsp), 1))
                {
                    tex = walking[0];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp),1) && currentframe <= Math.Max(8-Math.Abs(xsp),1)*2)
                {
                    tex = walking[1];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1)*2 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 3)
                {
                    tex = walking[2];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1)*3 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 4)
                {
                    tex = walking[3];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1)*4 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 5)
                {
                    tex = walking[4];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1)*5 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 6)
                {
                    tex = walking[5];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1)*6 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 7)
                {
                    tex = walking[6];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1)*7 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 8)
                {
                    tex = walking[7];
                }
                else
                {
                    tex = walking[0];
                    currentframe = 0;
                }
            }
            else if (animstate == animationstate.running)
            {
                if (currentframe < Math.Max(8 - Math.Abs(xsp), 1))
                {
                    tex = running[0];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1) && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 2)
                {
                    tex = running[1];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1) * 2 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 3)
                {
                    tex = running[2];
                }
                else if (currentframe >= Math.Max(8 - Math.Abs(xsp), 1) * 3 && currentframe <= Math.Max(8 - Math.Abs(xsp), 1) * 4)
                {
                    tex = running[3];
                }
                else
                {
                    tex = running[0];
                    currentframe = 0;
                }
            }
            currentframe++;
        }

        public enum animationstate
        {
            idle,
            walking,
            running
        }
    }
}
