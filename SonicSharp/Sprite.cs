using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SonicSharp
{
    public class Sprite
    {
        public Texture2D texture;
        public Rectangle[] frames;
        public int currentframe = 0, curfrm = 0;
        public float framerate = 1;

        public Sprite(Texture2D texture, int framecount, int framewidth, int frameheight, int framesperrow, int framespercolumn, float framerate = 1)
        {
            frames = new Rectangle[framecount];
            this.texture = texture;
            this.framerate = framerate;
            int i = 0;

            for (int y=0;y<framespercolumn*frameheight; y+=frameheight)
            {
                for (int x=0;x<framesperrow*framewidth;x+=framewidth)
                {
                    if (i < framecount)
                    {
                        frames[i] = new Rectangle(x,y,framewidth,frameheight);
                        i++;
                    }
                    else break;
                }
            }
        }

        public Sprite(Texture2D texture, Rectangle[] frames, float framerate = 1)
        {
            this.frames = frames;
            this.texture = texture;
            this.framerate = framerate;
        }

        /// <summary>
        /// Changes the sprite frame appropriately when a certain number of frames have passed.
        /// </summary>
        /// <param name="framerate">How many frames must pass before the sprite's current frame is changed.</param>
        public void Animate(float framerate = 1)
        {
            if (curfrm < framerate)
            {
                curfrm++;
            }
            else if (curfrm >= framerate)
            {
                curfrm = 0;
                currentframe = (currentframe < frames.Length-1)?currentframe+1:0;
	        }
        }
    }
}
