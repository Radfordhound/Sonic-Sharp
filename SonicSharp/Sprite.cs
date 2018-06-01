using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SonicSharp
{
    public class Sprite
    {
        // Variables/Constants
        public Rectangle[] Frames;
        public Vector2 Origin = Vector2.Zero;
        public Texture2D Texture;
        public bool DoesLoop = true;
        public bool HasLooped { get; protected set; } = false;

        public int CurrentFrame = 0, FrameDelay = 0;
        protected int frameCounter = 0;
        public const string SpritesDir = "Sprites";

        // Constructors
        public Sprite(string texPath, int frameWidth = 16, int frameHeight = 16,
            Vector2? origin = null, int frameDelay = 0, bool doesLoop = true) : this(
                Program.Content.Load<Texture2D>(texPath), frameWidth,
                frameHeight, origin, frameDelay, doesLoop) { }

        public Sprite(Texture2D tex, int frameWidth = 16, int frameHeight = 16,
            Vector2? origin = null, int frameDelay = 0, bool doesLoop = true)
        {
            Frames = new Rectangle[(tex.Width / frameWidth) *
                (tex.Height / frameHeight)];

            DoesLoop = doesLoop;
            Texture = tex;
            Origin = origin ?? Vector2.Zero;
            FrameDelay = frameDelay;

            int i = -1;
            for (int y = 0; y < tex.Height; y += frameHeight)
            {
                for (int x = 0; x < tex.Width; x += frameWidth)
                {
                    Frames[++i] = new Rectangle(x, y, frameWidth, frameHeight);
                }
            }
        }

        // Methods
        public virtual void Reset()
        {
            CurrentFrame = frameCounter = 0;
            HasLooped = false;
        }

        public virtual void Animate(float? frameDelay = null)
        {
            float delay = frameDelay ?? FrameDelay;
            if (frameCounter >= delay)
            {
                frameCounter = 0;
                if (CurrentFrame < Frames.Length - 1)
                {
                    ++CurrentFrame;
                }
                else
                {
                    CurrentFrame = (DoesLoop) ? 0 : CurrentFrame;
                    HasLooped = true;
                }
            }
            else
            {
                ++frameCounter;
            }
        }

        public virtual void Draw(float x, float y, SpriteEffects effects =
            SpriteEffects.None, Color? color = null)
        {
            Draw(new Vector2(x, y), effects, color);
        }

        public virtual void Draw(Vector2 position, SpriteEffects effects =
            SpriteEffects.None, Color? color = null)
        {
            if (Texture == null)
                return;

            GameWindow.SpriteBatch.Draw(
                Texture, position,
                Frames[CurrentFrame],
                color ?? Color.White, 0,
                Origin, 1, effects, 0);
        }
    }
}