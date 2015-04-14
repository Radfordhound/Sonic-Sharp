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
        public bool falling = true;
        public int currentframe = 0;
        public int frmindex = 0;
        public Vector2 origin = new Vector2(20, 20);
        public float angle = 0;
        public animationstate animstate = animationstate.idle;
        public bool left = false;
        public double xsp = 0; //Player's X Speed
        public double ysp = 0; //Player's Y Speed
        public bool islookingup = false;
        public bool islookingdown = false;
        public Vector2 prevcampos = new Vector2();
        private int lookdelay = 0;
        public bool canlook = true;

        //Constants (We declare these as variables to allow for more flexibility.)
        public double acc = 0.046875;
        public double dec = 0.5;
        public double frc = 0.046875;
        public int top = 6;

        //Textures
        public List<Texture2D> idle = new List<Texture2D>();
        public List<Texture2D> walking = new List<Texture2D>();
        public List<Texture2D> running = new List<Texture2D>();
        public List<Texture2D> pushing = new List<Texture2D>();
        public List<Texture2D> lookingup = new List<Texture2D>();
        public List<Texture2D> ducking = new List<Texture2D>();
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

            if ((kbst.IsKeyDown(Keys.Up) || kbst.IsKeyDown(Keys.W)) && xsp == 0 && (animstate == animationstate.idle || animstate == animationstate.lookingup))
            {
                if (canlook)
                {
                    if (animstate != animationstate.lookingup)
                    {
                        prevcampos = Camera.campos;
                        animstate = animationstate.lookingup;
                        currentframe = 0;
                    }
                    else if (lookdelay >= 120 && Camera.campos.Y > prevcampos.Y - 104)
                    {
                        Camera.campos.Y -= 2;
                        Console.WriteLine(animstate.ToString());
                    }
                    else
                    {
                        lookdelay++;
                    }
                    
                }
            }
            else
            {
                if (animstate == animationstate.lookingup)
                {
                    Camera.yspeed = 2;
                    lookdelay = 0;
                    canlook = false;
                    animstate = animationstate.idle;
                    currentframe = 0;
                }
            }
            
            if ((kbst.IsKeyDown(Keys.Down) || kbst.IsKeyDown(Keys.S)) && xsp == 0 && (animstate == animationstate.idle || animstate == animationstate.ducking))
            {
                if (canlook)
                {
                    if (animstate != animationstate.ducking)
                    {
                        prevcampos = Camera.campos;
                        animstate = animationstate.ducking;
                        currentframe = 0;
                    }
                    else if (lookdelay >= 120 && Camera.campos.Y < prevcampos.Y + 88)
                    {
                        Camera.campos.Y += 2;
                        Console.WriteLine(Camera.campos.Y + " , " + (prevcampos.Y+88).ToString() + " , " + lookdelay);
                    }
                    else
                    {
                        lookdelay++;
                    }
                }
            }
            else
            {
                if (animstate == animationstate.ducking)
                {
                    Camera.yspeed = 2;
                    lookdelay = 0;
                    canlook = false;
                    animstate = animationstate.idle;
                    currentframe = 0;
                }
                
            }

            if (falling)
            {
                ysp = 1;
            }
            else
            {
                ysp = 0;
            }

            if (this.GetType() == typeof(Sonic))
            {
                //Custom Sonic-only logic goes here.
            }

            x += xsp;
            y += ysp;

            Tiles.CollideWith(this);
            
        }

        public void Draw()
        {
            if (!left)
            {
                Main.mainBatch.Draw(tex, new Vector2((float)x, (float)y), null, null, origin, angle, null, null, SpriteEffects.None);
            }
            else
            {
                Main.mainBatch.Draw(tex, new Vector2((float)x, (float)y), null, null, origin, angle, null, null, SpriteEffects.FlipHorizontally);
            }

            Animate();

            if (this.GetType() == typeof(Sonic))
            {
                //Custom Sonic-only logic goes here.
            }
        }

        public void GetAnimState()
        {
            if (animstate != animationstate.pushing && animstate != animationstate.ducking && animstate != animationstate.lookingup)
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
            else
            {
                if (animstate == animationstate.pushing)
                {
                    if (!(Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right)))
                    {
                        animstate = animationstate.idle;
                        canlook = true;
                    }
                }
            }
        }

        public Texture2D GetTexture(double[] frames,List<Texture2D> textures)
        {
            if (currentframe < frames[0])
            {
                return textures[0];
            }
            else
            {

                for (int i = 0; i < frames.Length;i++)
                {
                    if (currentframe >= frames[i] && frames.Length >= i+1 && currentframe < frames[i+1])
                    {
                        return textures[i];
                    }
                }
            }
            return null;
        }

        /*public Texture2D GetTexture(int framerate, List<Texture2D> textures)
        {
            if (currentframe < frames[0])
            {
                return textures[0];
            }
            else
            {

                for (int i = 0; i < frames.Length; i++)
                {
                    if (currentframe >= frames[i] && frames.Length >= i + 1 && currentframe < frames[i + 1])
                    {
                        return textures[i];
                    }
                }
            }
            return null;
        }*/

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
            /*if (animstate == animationstate.idle)
            {
                tex = GetTexture(new double[] { 300,306,312,317,330,342,354,366,378,390,402,414,426,438,450,462,474,486,498,510,546,582,588,624,635}, idle); //test
            }*/
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
            else if (animstate == animationstate.pushing)
            {
                if (currentframe < 32)
                {
                    tex = pushing[0];
                }
                else if (currentframe >= 32 && currentframe <= 64)
                {
                    tex = pushing[1];
                }
                else if (currentframe >= 64 && currentframe <= 96)
                {
                    tex = pushing[2];
                }
                else if (currentframe >= 96 && currentframe <= 128)
                {
                    tex = pushing[3];
                }
                else
                {
                    tex = pushing[0];
                    currentframe = 0;
                }
            }
            else if (animstate == animationstate.lookingup)
            {
                if (currentframe < 7)
                {
                    tex = lookingup[0];
                }
                else if (currentframe >= 7 && currentframe <= 14)
                {
                    tex = lookingup[1];
                }
            }
            else if (animstate == animationstate.ducking)
            {
                if (currentframe < 7)
                {
                    tex = ducking[0];
                }
                else if (currentframe >= 7 && currentframe <= 14)
                {
                    tex = ducking[1];
                }
            }
            currentframe++;
        }

        public enum animationstate
        {
            idle,
            walking,
            running,
            pushing,
            lookingup,
            ducking
        }
    }
}
