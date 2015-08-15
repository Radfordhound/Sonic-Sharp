using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        public Sonic(float x, float y, Controllers controller) : base (x,y,controller)
        {
            idlesprite = new Sprite(Program.game.Content.Load<Texture2D>("Sprites\\Players\\Sonic\\idle"), new Rectangle[]
            {
                new Rectangle(0,0,40,40), new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),
                new Rectangle(0,0,40,40), new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),
                new Rectangle(0,0,40,40), new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),
                new Rectangle(0,0,40,40), new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),
                new Rectangle(0,0,40,40), new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),new Rectangle(0,0,40,40),
                new Rectangle(40,0,40,40), new Rectangle(80,0,40,40),new Rectangle(80,0,40,40),new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),new Rectangle(160,0,40,40),new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),
                new Rectangle(160,0,40,40), new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),new Rectangle(160,0,40,40),new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),new Rectangle(160,0,40,40),new Rectangle(120,0,40,40),
                new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),new Rectangle(160,0,40,40),new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),new Rectangle(160,0,40,40),new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(160,0,40,40),
                new Rectangle(160,0,40,40),new Rectangle(120,0,40,40),new Rectangle(120,0,40,40),new Rectangle(200,0,40,40),new Rectangle(200,0,40,40),new Rectangle(200,0,40,40),new Rectangle(200,0,40,40),new Rectangle(240,0,40,40),new Rectangle(240,0,40,40),new Rectangle(240,0,40,40),
                new Rectangle(240,0,40,40),new Rectangle(240,0,40,40),new Rectangle(280,0,40,40),new Rectangle(320,0,40,40),new Rectangle(320,0,40,40),new Rectangle(320,0,40,40),new Rectangle(320,0,40,40),new Rectangle(280,0,40,40),new Rectangle(280,0,40,40)
            },6,53);

            walkingsprite = new Sprite(Program.game.Content.Load<Texture2D>("Sprites\\Players\\Sonic\\walking"),8,40,40,8,1);
            runningsprite = new Sprite(Program.game.Content.Load<Texture2D>("Sprites\\Players\\Sonic\\running"),4,40,40,4,1);
            sprite = idlesprite;
        }
    }

    public class Tails : Player
    {
        public Tails(float x, float y, Controllers controller) : base (x,y,controller)
        {
            //TODO: Dis. :P
        }
    }

    public class Knuckles : Player
    {
        public Knuckles(float x, float y, Controllers controller) : base(x,y,controller)
        {
            //TODO: Dis. :P
        }
    }
    #endregion

    #region The actual Player class (HEAVILY based off of the awesome Sonic Physics Guide from the even awesome-er Sonic Retro Wiki! http://info.sonicretro.org/Sonic_Physics_Guide)
    public class Player : gameObject
    {
        //Variables
        public Sprite idlesprite, walkingsprite, runningsprite;
        private Controllers controller = Controllers.keyboard;
        private float xsp = 0, ysp = 0;
        private bool left = false;

        public enum Controllers { keyboard, gamepad1, gamepad2, gamepad3, gamepad4 }
        private enum ControllerStates { justpressed, isdown, justreleased, notbeingused }
        private enum Controls { left, right }

        //Constants (Change these to variables if you need to edit their properties after the game has begun.)
        private const float acc = 0.046875f, dec = 0.5f, frc = 0.046875f, top = 6;

        //Player Constructor
        public Player(float x, float y, Controllers controller) { pos = new Vector2(x, y); this.controller = controller; active = false; }

        #region Functions
        /// <summary>
        /// Gets the current state of the given control. GREATLY simplifies checking for user input!
        /// </summary>
        /// <param name="control">The control to check for.</param>
        /// <returns>The state of the given control.</returns>
        private ControllerStates GetControlState(Controls control)
        {
            if (controller == Controllers.keyboard)
            {
                if (control == Controls.left)
                {
                    return (Main.kbst.IsKeyDown(Keys.Left) || Main.kbst.IsKeyDown(Keys.A)) ? ((!Main.prevkbst.IsKeyDown(Keys.Left) || !Main.prevkbst.IsKeyDown(Keys.A)) ? ControllerStates.justpressed:ControllerStates.isdown) : (Main.prevkbst.IsKeyDown(Keys.Left) || Main.prevkbst.IsKeyDown(Keys.A)) ?ControllerStates.justreleased:ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.kbst.IsKeyDown(Keys.Right) || Main.kbst.IsKeyDown(Keys.D)) ? ((!Main.prevkbst.IsKeyDown(Keys.Right) || !Main.prevkbst.IsKeyDown(Keys.D)) ? ControllerStates.justpressed:ControllerStates.isdown) : (Main.prevkbst.IsKeyDown(Keys.Right) || Main.prevkbst.IsKeyDown(Keys.D)) ?ControllerStates.justreleased:ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad1)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[0].ThumbSticks.Left.X < 0 || Main.gpsts[0].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[0].ThumbSticks.Left.X > 0 || Main.gpsts[0].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed:ControllerStates.isdown) : (Main.prevgpsts[0].ThumbSticks.Left.X < 0 || Main.prevgpsts[0].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed:ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[0].ThumbSticks.Left.X > 0 || Main.gpsts[0].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[0].ThumbSticks.Left.X < 0 || Main.gpsts[0].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[0].ThumbSticks.Left.X > 0 || Main.prevgpsts[0].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad2)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[1].ThumbSticks.Left.X < 0 || Main.gpsts[1].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[1].ThumbSticks.Left.X > 0 || Main.gpsts[1].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[1].ThumbSticks.Left.X < 0 || Main.prevgpsts[1].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[1].ThumbSticks.Left.X > 0 || Main.gpsts[1].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[1].ThumbSticks.Left.X < 0 || Main.gpsts[1].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[1].ThumbSticks.Left.X > 0 || Main.prevgpsts[1].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad3)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[2].ThumbSticks.Left.X < 0 || Main.gpsts[2].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[2].ThumbSticks.Left.X > 0 || Main.gpsts[2].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[2].ThumbSticks.Left.X < 0 || Main.prevgpsts[2].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[2].ThumbSticks.Left.X > 0 || Main.gpsts[2].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[2].ThumbSticks.Left.X < 0 || Main.gpsts[2].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[2].ThumbSticks.Left.X > 0 || Main.prevgpsts[2].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad4)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[3].ThumbSticks.Left.X < 0 || Main.gpsts[3].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[3].ThumbSticks.Left.X > 0 || Main.gpsts[3].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[3].ThumbSticks.Left.X < 0 || Main.prevgpsts[3].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[3].ThumbSticks.Left.X > 0 || Main.gpsts[3].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[3].ThumbSticks.Left.X < 0 || Main.gpsts[3].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[3].ThumbSticks.Left.X > 0 || Main.prevgpsts[3].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
            }

            return ControllerStates.notbeingused;
        }

        public override void Update()
        {
            if (active)
            {
                if ((int)GetControlState(Controls.left) < 2)
                {
                    if (xsp > 0)
                    {
                        xsp -= dec;
                    }
                    else if (xsp > -top)
                    {
                        xsp = xsp - acc;
                    }
                    left = true;
                }
                else if ((int)GetControlState(Controls.right) < 2)
                {
                    if (xsp < 0)
                    {
                        xsp += dec;
                    }
                    else if (xsp < top)
                    {
                        xsp = xsp + acc;
                    }
                    left = false;
                }
                else xsp = xsp - Math.Min(Math.Abs(xsp), frc) * Math.Sign(xsp);

                pos.X += xsp;
                pos.Y += ysp;

                //Animation
                if (Math.Abs(xsp) >= top) { sprite = runningsprite; }
                else if (Math.Abs(xsp) > 0) { sprite = walkingsprite; }
                else { sprite = idlesprite; }

                if (sprite != null) { sprite.Animate((sprite == runningsprite || sprite == walkingsprite)? Math.Max(8 - Math.Abs(xsp), 1):sprite.framerate); }
            }
        }

        public override void Draw()
        {
            if (active) { Main.spriteBatch.Draw(texture: sprite.texture, position: pos, sourceRectangle: sprite.frames[sprite.currentframe], effects: (left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, origin: new Vector2(20, 20)); }
        }
        #endregion
    }
    #endregion
}
