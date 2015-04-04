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
        public double xsp; //Player's X Speed
        public double ysp; //Player's Y Speed

        //Constants (We declare these as variables to allow for more flexibility.)
        public double acc = 0.046875;
        public double dec = 0.5;
        public double frc = 0.046875;
        public int top = 6;

        //Textures
        public Texture2D running;

        public void Update(GameTime gameTime)
        {
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
            Main.spriteBatch.Draw(running, new Vector2((float)x, 70));

            if (this.GetType() == typeof(Sonic))
            {
                //Custom Sonic-only logic goes here.
            }
        }
    }
}
