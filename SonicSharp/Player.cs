using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonicSharp
{
    #region Player Types
    public class Sonic : Player
    {
        public Sonic(float x, float y) : base (x,y)
        {
            idlesprite = new Sprite(Program.game.Content.Load<Texture2D>("Sprites\\Players\\Sonic\\idle"),9,40,40,9,1);
            sprite = idlesprite;
        }
    }
    #endregion

    #region The actual Player class
    public class Player : gameObject
    {
        //Variables
        public Sprite idlesprite, walkingsprite, runningsprite;
        private float xsp = 0, ysp = 0;

        //Constants (Change these to variables if you need to edit their properties after the game has begun.)
        private const float acc = 0.046875f, dec = 0.5f, frc = 0.046875f, top = 6;

        public Player(float x, float y) { pos = new Vector2(x, y); }
    }
    #endregion
}
