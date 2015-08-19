using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonicSharp
{
    #region gameObject class
    public class gameObject
    {
        public Vector2 pos = Vector2.Zero;
        public float width = 16, height = 16;
        private Sprite Sprite;
        public bool active = true;
        public int id = -1;

        public Sprite sprite
        {
            get { return Sprite; }
            set { if (Sprite != value) { value.curfrm = 0; value.currentframe = 0; } Sprite = value; }
        }

        public gameObject()
        {
            id = Level.objects.Count;
        }

        /// <summary>
        /// Called whenever a player collides with a gameObject on the object the player collided with. #MakesSense
        /// </summary>
        public virtual void PlayerCollision(Player plr) {}

        public virtual void Update()
        {
            if (sprite != null && active) { sprite.Animate(sprite.framerate); }
        }

        public virtual void Draw()
        {
            if (active) { Main.spriteBatch.Draw(texture: sprite.texture, position: pos, sourceRectangle: sprite.frames[sprite.currentframe]); }
        }

        public virtual void Delete()
        {
            for (int i = id+1; i < Level.objects.Count; i++) { Level.objects[i].id--; }
            Level.objects.RemoveAt(id);
        }
    }
    #endregion

    #region Object types
    public class CollideableObject : gameObject
    {
        //TODO: Add collideable objects!
    }

    public class CameraHBorder : gameObject
    {
        public bool stopsplayer = false;

        public CameraHBorder(Rectangle border, bool stopsplayer) : base()
        {
            pos = new Vector2(border.X,border.Y);
            width = border.Width; height = border.Height;
            this.stopsplayer = stopsplayer; active = false;
        }

        public override void PlayerCollision(Player plr)
        {
            if (stopsplayer)
            {
                plr.pos.X = pos.X+((plr.pos.X > pos.X)? width + 12:-11);
                plr.xsp = (Math.Abs(plr.xsp) > 0)?((Math.Abs(plr.xsp) >= Player.top)?(Player.top/2)*Math.Sign(plr.xsp):plr.xsp):0;
            }
        }

        public override void Update()
        {
            if (Camera.pos.Y / Main.scalemodifier < pos.Y + height && Camera.pos.Y / Main.scalemodifier > pos.Y)
            {
                if (Camera.pos.X / Main.scalemodifier < pos.X + width && Camera.pos.X / Main.scalemodifier > pos.X)
                {
                    Camera.pos.X = (pos.X + ((pos.X < Camera.pos.X / Main.scalemodifier) ? width : 0)) * Main.scalemodifier;
                }
                else if ((Camera.pos.X + Program.game.Window.ClientBounds.Width) / Main.scalemodifier < pos.X + width && (Camera.pos.X + Program.game.Window.ClientBounds.Width) / Main.scalemodifier > pos.X)
                {
                    Camera.pos.X = ((pos.X + ((pos.X < Camera.pos.X / Main.scalemodifier) ? width : 0)) * Main.scalemodifier) - Program.game.Window.ClientBounds.Width;
                }
            }
        }
    }

    public class CameraVBorder : gameObject
    {
        public bool stopsplayer = false;

        public CameraVBorder(Rectangle border, bool stopsplayer) : base()
        {
            pos = new Vector2(border.X, border.Y);
            width = border.Width; height = border.Height;
            this.stopsplayer = stopsplayer; active = false;
        }

        public override void PlayerCollision(Player plr)
        {
            if (stopsplayer)
            {
                plr.pos.Y = pos.Y + ((plr.pos.Y > pos.Y) ? height + 12 : -11);
                plr.ysp = 0;
            }
        }

        public override void Update()
        {
            if (Camera.pos.X / Main.scalemodifier < pos.X + width && Camera.pos.X / Main.scalemodifier > pos.X)
            {
                if (Camera.pos.Y / Main.scalemodifier < pos.Y + height && Camera.pos.Y / Main.scalemodifier > pos.Y)
                {
                    Camera.pos.Y = (pos.Y + ((pos.Y < Camera.pos.Y / Main.scalemodifier) ? height : 0)) * Main.scalemodifier;
                }
                else if ((Camera.pos.Y + Program.game.Window.ClientBounds.Height) / Main.scalemodifier < pos.Y + height && (Camera.pos.Y + Program.game.Window.ClientBounds.Height) / Main.scalemodifier > pos.Y)
                {
                    Camera.pos.Y = ((pos.Y + ((pos.Y < Camera.pos.Y / Main.scalemodifier) ? height : 0)) * Main.scalemodifier) - Program.game.Window.ClientBounds.Height;
                }
            }
        }
    }

    public class DeathTrigger : gameObject
    {
        public DeathTrigger(Rectangle border) : base()
        {
            pos = new Vector2(border.X, border.Y);
            width = border.Width; height = border.Height;
            active = false;
        }

        public override void PlayerCollision(Player plr)
        {
            Player.deathsound.Play();
            plr.sprite = plr.deathsprite;
            plr.xsp = 0; plr.ysp = -7;
            Main.FadeOut(4,140,124);
        }

        public override void Draw()
        {
            if (Main.fadingout && Main.afterfadedelay == Main.afdcounter)
            {
                foreach (Player plr in Main.players)
                {
                    if (plr.GetType() == typeof(Sonic)) { plr.pos = Level.playerstarts[0]; }
                    else if (plr.GetType() == typeof(Tails)) { plr.pos = Level.playerstarts[1]; }
                    else if (plr.GetType() == typeof(Knuckles)) { plr.pos = Level.playerstarts[2]; }

                    if (plr.GetType() == typeof(Sonic)) { Camera.pos = Level.camerastarts[0] * Main.scalemodifier; }
                    else if (plr.GetType() == typeof(Tails)) { Camera.pos = Level.camerastarts[1] * Main.scalemodifier; }
                    else if (plr.GetType() == typeof(Knuckles)) { Camera.pos = Level.camerastarts[2] * Main.scalemodifier; }

                    plr.sprite = plr.idlesprite;
                    plr.xsp = 0; plr.ysp = 0;
                    plr.active = true; plr.falling = true; plr.left = false;
                }
                Main.FadeIn(4);  //TODO: Re-spawn deleted objects
            }
        }
    }

    public class Ring : gameObject
    {
        public static Sprite ringsprite;
        public static SoundEffect ringsound;

        public Ring(float x, float y) : base()
        {
            pos = new Vector2(x,y);
        }

        public override void PlayerCollision(Player plr)
        {
            //TODO: Add one to ring counter
            ringsound.Play();
            Level.objects.Add(new Particle(pos.X,pos.Y-16,new Sprite(Particle.ringsparkle,4,16,16,4,1,6),24));
            Delete();
        }

        public override void Draw()
        {
            if (active) { Main.spriteBatch.Draw(texture: ringsprite.texture, position: pos, sourceRectangle: ringsprite.frames[ringsprite.currentframe], origin: new Vector2(0,16)); }
        }
    }

    public class Particle : gameObject
    {
        public static Texture2D ringsparkle;

        public float framesuntildeletion;
        private int frame = 0;

        public Particle(float x, float y, Sprite sprite, float framesuntildeletion) : base()
        {
            pos = new Vector2(x, y);
            this.sprite = sprite;
            this.framesuntildeletion = framesuntildeletion;
        }

        public override void Update()
        {
            base.Update();
            if (frame >= framesuntildeletion) { Delete(); }
            frame++;
        }
    }
    #endregion
}
