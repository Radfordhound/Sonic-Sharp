using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

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
            jumpingsprite = new Sprite(Program.game.Content.Load<Texture2D>("Sprites\\Players\\Sonic\\jumping"),5,30,30,5,1);
            deathsprite = new Sprite(Program.game.Content.Load<Texture2D>("Sprites\\Players\\Sonic\\death"),1,40,40,1,1);

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
        public Sprite idlesprite, walkingsprite, runningsprite, jumpingsprite, deathsprite;
        public static SoundEffect jumpingsound, deathsound;
        public float xsp = 0, ysp = 1;
        public bool left = false, falling = true, paused = false;
        private Controllers controller = Controllers.keyboard;

        public enum Controllers { keyboard, gamepad1, gamepad2, gamepad3, gamepad4 }
        private enum ControllerStates { justpressed, isdown, justreleased, notbeingused }
        private enum Controls { left, right, jump }

        //Constants (Change these to variables if you need to edit their properties after the game has begun.)
        public const float acc = 0.046875f, dec = 0.5f, frc = 0.046875f, top = 6, grv = 0.21875f, jmp = -6.5f, air = 0.09375f;

        //Player Constructor
        public Player(float x, float y, Controllers controller) { pos = new Vector2(x, y); this.controller = controller; active = false; id = Main.players.Count; }

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
                else if (control == Controls.jump)
                {
                    return (Main.kbst.IsKeyDown(Keys.Space)) ? ((!Main.prevkbst.IsKeyDown(Keys.Space)) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevkbst.IsKeyDown(Keys.Space)) ? ControllerStates.justreleased : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad1)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[0].ThumbSticks.Left.X < 0 || Main.gpsts[0].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[0].ThumbSticks.Left.X > 0 || Main.prevgpsts[0].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed:ControllerStates.isdown) : (Main.prevgpsts[0].ThumbSticks.Left.X < 0 || Main.prevgpsts[0].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed:ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[0].ThumbSticks.Left.X > 0 || Main.gpsts[0].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[0].ThumbSticks.Left.X < 0 || Main.prevgpsts[0].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[0].ThumbSticks.Left.X > 0 || Main.prevgpsts[0].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.jump)
                {
                    return (Main.gpsts[0].Buttons.A == ButtonState.Pressed) ? ((Main.prevgpsts[0].Buttons.A != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[0].Buttons.A == ButtonState.Pressed) ? ControllerStates.justreleased : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad2)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[1].ThumbSticks.Left.X < 0 || Main.gpsts[1].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[1].ThumbSticks.Left.X > 0 || Main.prevgpsts[1].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[1].ThumbSticks.Left.X < 0 || Main.prevgpsts[1].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[1].ThumbSticks.Left.X > 0 || Main.gpsts[1].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[1].ThumbSticks.Left.X < 0 || Main.prevgpsts[1].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[1].ThumbSticks.Left.X > 0 || Main.prevgpsts[1].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.jump)
                {
                    return (Main.gpsts[1].Buttons.A == ButtonState.Pressed) ? ((Main.prevgpsts[1].Buttons.A != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[1].Buttons.A == ButtonState.Pressed) ? ControllerStates.justreleased : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad3)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[2].ThumbSticks.Left.X < 0 || Main.gpsts[2].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[2].ThumbSticks.Left.X > 0 || Main.prevgpsts[2].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[2].ThumbSticks.Left.X < 0 || Main.prevgpsts[2].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[2].ThumbSticks.Left.X > 0 || Main.gpsts[2].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[2].ThumbSticks.Left.X < 0 || Main.prevgpsts[2].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[2].ThumbSticks.Left.X > 0 || Main.prevgpsts[2].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.jump)
                {
                    return (Main.gpsts[2].Buttons.A == ButtonState.Pressed) ? ((Main.prevgpsts[2].Buttons.A != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[2].Buttons.A == ButtonState.Pressed) ? ControllerStates.justreleased : ControllerStates.notbeingused;
                }
            }
            else if (controller == Controllers.gamepad4)
            {
                if (control == Controls.left)
                {
                    return (Main.gpsts[3].ThumbSticks.Left.X < 0 || Main.gpsts[3].DPad.Left == ButtonState.Pressed) ? ((Main.prevgpsts[3].ThumbSticks.Left.X > 0 || Main.prevgpsts[3].DPad.Left != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[3].ThumbSticks.Left.X < 0 || Main.prevgpsts[3].DPad.Left == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.right)
                {
                    return (Main.gpsts[3].ThumbSticks.Left.X > 0 || Main.gpsts[3].DPad.Right == ButtonState.Pressed) ? ((Main.prevgpsts[3].ThumbSticks.Left.X < 0 || Main.prevgpsts[3].DPad.Right != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[3].ThumbSticks.Left.X > 0 || Main.prevgpsts[3].DPad.Right == ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.notbeingused;
                }
                else if (control == Controls.jump)
                {
                    return (Main.gpsts[3].Buttons.A == ButtonState.Pressed) ? ((Main.prevgpsts[3].Buttons.A != ButtonState.Pressed) ? ControllerStates.justpressed : ControllerStates.isdown) : (Main.prevgpsts[3].Buttons.A == ButtonState.Pressed) ? ControllerStates.justreleased : ControllerStates.notbeingused;
                }
            }

            return ControllerStates.notbeingused;
        }

        public override void Update()
        {
            if (active && !paused)
            {
                #region Input/Movement
                if (sprite != deathsprite)
                {
                    //Directonal Movement
                    if ((int)GetControlState(Controls.left) < 2)
                    {
                        if (!falling)
                        {
                            if (xsp > 0)
                            {
                                xsp -= dec;
                            }
                            else if (xsp > -top)
                            {
                                xsp = xsp - acc;
                            }
                        }
                        else if (xsp > -top) { xsp -= air; }

                        left = true;
                    }
                    else if ((int)GetControlState(Controls.right) < 2)
                    {
                        if (!falling)
                        {
                            if (xsp < 0)
                            {
                                xsp += dec;
                            }
                            else if (xsp < top)
                            {
                                xsp = xsp + acc;
                            }
                        }
                        else if (xsp < top) { xsp += air; }

                        left = false;
                    }
                    else if (!falling) { xsp = xsp - Math.Min(Math.Abs(xsp), frc) * Math.Sign(xsp); }

                    //Jumping
                    if (GetControlState(Controls.jump) == ControllerStates.justpressed && !falling)
                    {
                        bool canjump = true;

                        //Check for a ceiling collison at Y-25 before jumping.
                        foreach (Tile tile in Level.tiles)
                        {
                            if (Level.tilesets.Count > tile.textureid && Level.tilesets[tile.textureid].tileproperties.Count > tile.tileid && Level.tilesets[tile.textureid].tileproperties[tile.tileid] != null && Level.tilesets[tile.textureid].tileproperties[tile.tileid].ContainsKey("hm"))
                            {
                                for (int x = (int)pos.X - 11; x <= pos.X + 11; x++)
                                {
                                    if (tile.pos.X <= x && tile.pos.X + 16 >= x && tile.pos.Y <= pos.Y - 25 && tile.pos.Y + 16 >= pos.Y - 25)
                                    {
                                        if (Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',').Length > ((int)(x - tile.pos.X)) && (tile.pos.Y + 16) - Convert.ToInt32(Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',')[(int)(x - tile.pos.X)]) <= pos.Y - 25)
                                        {
                                            canjump = false; break;
                                        }
                                    }
                                }
                            }
                        }

                        if (canjump)
                        {
                            pos.Y += 5; //This is to make up for the 5 pixel height difference between the jumping sprite and the regular sprites.
                            ysp = jmp; sprite = jumpingsprite;
                            jumpingsound.Play();
                        }
                    }

                    if (GetControlState(Controls.jump) == ControllerStates.justreleased && ysp < -4) { ysp = -4; }

                    //Air Drag
                    if (falling && ysp < 0 && ysp > -4)
                    {
                        xsp = xsp - ((xsp / 0.125f) / 256);
                    }
                }

                //Now that we've computed where we need to move the character, it's time to actually move the character!
                pos.X += xsp;
                pos.Y += ysp;
                #endregion

                #region Collision
                float ay = -1, by = -1, cy = -1, dy = -1;
                falling = false; //Still feel odd about setting this to false by default for some reason...

                if (sprite != deathsprite)
                {
                    //Tile Collision
                    foreach (Tile tile in Level.tiles)
                    {
                        if (Level.tilesets.Count > tile.textureid && Level.tilesets[tile.textureid].tileproperties.Count > tile.tileid && Level.tilesets[tile.textureid].tileproperties[tile.tileid] != null && Level.tilesets[tile.textureid].tileproperties[tile.tileid].ContainsKey("hm"))
                        {
                            //Horizontal Collision
                            for (int x = (int)pos.X - 11; x <= pos.X + 11; x++)
                            {
                                if (tile.pos.X <= x && tile.pos.X + 16 >= x && tile.pos.Y <= pos.Y + ((sprite != jumpingsprite) ? 4 : 0) && tile.pos.Y + 16 >= pos.Y + ((sprite != jumpingsprite) ? 4 : 0))
                                {
                                    if (Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',').Length > ((int)(x - tile.pos.X)) && (tile.pos.Y + 16) - Convert.ToInt32(Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',')[(int)(x - tile.pos.X)]) <= pos.Y + ((sprite != jumpingsprite) ? 4 : 0))
                                    {
                                        if (tile.pos.X >= x) { pos.X = tile.pos.X - 11; }
                                        else { pos.X = tile.pos.X + 27; }

                                        if (sprite != jumpingsprite || (tile.pos.X >= x && Math.Sign(xsp) >= 0) || (tile.pos.X < x && Math.Sign(xsp) < 0)) { xsp = 0; }
                                    }
                                }
                            }

                            //Vertical Collision
                            if (ysp >= 0)
                            {
                                //Floor Collision
                                for (int y = (int)pos.Y; y < pos.Y + 20; y++)
                                {
                                    //Sensor A
                                    if (tile.pos.Y <= y && tile.pos.Y + 16 >= y && tile.pos.X <= pos.X - 9 && tile.pos.X + 16 >= pos.X - 9)
                                    {
                                        if (Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',').Length > ((int)((pos.X - 9) - tile.pos.X)))
                                        {
                                            ay = ((tile.pos.Y + 16) - (Convert.ToInt32(Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',')[(int)((pos.X - 9) - tile.pos.X)])) - ((sprite == jumpingsprite) ? 15 : 20));
                                        }
                                        ysp = 0;
                                    }

                                    //Sensor B
                                    if (tile.pos.Y <= y && tile.pos.Y + 16 >= y && tile.pos.X <= pos.X + 9 && tile.pos.X + 16 >= pos.X + 9)
                                    {
                                        if (Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',').Length > ((int)((pos.X + 9) - tile.pos.X)))
                                        {
                                            by = ((tile.pos.Y + 16) - (Convert.ToInt32(Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',')[(int)((pos.X + 9) - tile.pos.X)])) - ((sprite == jumpingsprite) ? 15 : 20));
                                        }
                                        ysp = 0;
                                    }
                                }
                            }

                            //Ceiling Collision
                            for (int y = (int)pos.Y; y < pos.Y - 20; y++)
                            {
                                //Sensor C
                                if (tile.pos.Y <= y && tile.pos.Y + 16 >= y && tile.pos.X <= pos.X - 9 && tile.pos.X + 16 >= pos.X - 9)
                                {
                                    if (Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',').Length > ((int)((pos.X - 9) - tile.pos.X)) && Convert.ToInt32(Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',')[(int)((pos.X - 9) - tile.pos.X)]) != 0)
                                    {
                                        cy = tile.pos.Y + ((sprite == jumpingsprite) ? 31 : 36);
                                    }
                                    ysp = 0;
                                }

                                //Sensor D
                                if (tile.pos.Y <= y && tile.pos.Y + 16 >= y && tile.pos.X <= pos.X + 9 && tile.pos.X + 16 >= pos.X + 9)
                                {
                                    if (Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',').Length > ((int)((pos.X + 9) - tile.pos.X)) && Convert.ToInt32(Level.tilesets[tile.textureid].tileproperties[tile.tileid]["hm"].Split(',')[(int)((pos.X + 9) - tile.pos.X)]) != 0)
                                    {
                                        dy = tile.pos.Y + ((sprite == jumpingsprite) ? 31 : 36);
                                    }
                                    ysp = 0;
                                }
                            }
                        }
                    }
                }

                if (ay != -1 && by != -1) { pos.Y = (ay <= by) ? ((sprite != jumpingsprite || pos.Y > ay)?ay:pos.Y) : ((sprite != jumpingsprite || pos.Y > by)?by:pos.Y); }
                else if (ay != -1) { pos.Y = (sprite != jumpingsprite || pos.Y > ay)?ay:pos.Y; }
                else if (by != -1) { pos.Y = (sprite != jumpingsprite || pos.Y > by)?by:pos.Y; }
                else { falling = true; ysp += grv; if (ysp > 16) { ysp = 16; } }

                if (sprite != deathsprite)
                {
                    if (!falling && sprite == jumpingsprite) { sprite = idlesprite; }

                    //Object Collision
                    for (int i = Level.objects.Count - 1; i >= 0; i--)
                    {
                        gameObject obj = Level.objects[i];
                        //TODO: Add collideable objects.

                        //Horizontal Collision
                        for (int x = (int)pos.X - 11; x <= pos.X + 11; x++)
                        {
                            if (obj.pos.X <= x && obj.pos.X + obj.width >= x && obj.pos.Y <= pos.Y + 4 && obj.pos.Y + obj.height >= pos.Y + 4)
                            {
                                obj.PlayerCollision(this);
                                break;
                            }
                        }

                        //Vertical Collision
                        for (int y = (int)pos.Y; y < pos.Y + 20; y++)
                        {
                            //Sensor A
                            if (obj.pos.Y <= y && obj.pos.Y + obj.height >= y && obj.pos.X <= pos.X - 9 && obj.pos.X + obj.width >= pos.X - 9)
                            {
                                obj.PlayerCollision(this);
                                break;
                            }

                            //Sensor B
                            if (obj.pos.Y <= y && obj.pos.Y + obj.height >= y && obj.pos.X <= pos.X + 9 && obj.pos.X + obj.width >= pos.X + 9)
                            {
                                obj.PlayerCollision(this);
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Animation
                if (sprite != jumpingsprite && sprite != deathsprite)
                {
                    if (Math.Abs(xsp) >= top) { sprite = runningsprite; }
                    else if (Math.Abs(xsp) > 0) { sprite = walkingsprite; }
                    else { sprite = idlesprite; }
                }

                if (sprite != null) { sprite.Animate((sprite == runningsprite || sprite == walkingsprite || sprite == jumpingsprite)? Math.Max(((sprite == jumpingsprite)?5:8) - Math.Abs(xsp), 1):sprite.framerate); }
                #endregion
            }
        }

        public override void Draw()
        {
            if (active) { Main.spriteBatch.Draw(texture: sprite.texture, position: pos, sourceRectangle: sprite.frames[sprite.currentframe], effects: (left) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, origin: new Vector2((sprite == jumpingsprite)?15:20, (sprite == jumpingsprite)?15:20)); }
        }

        public override void Delete()
        {
            for (int i = id + 1; i < Main.players.Count; i++) { Main.players[i].id--; }
            Main.players.RemoveAt(id);
        }
        #endregion
    }
    #endregion
}
