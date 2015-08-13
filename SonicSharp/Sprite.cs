using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SonicSharp
{
    public class Sprite
    {
        public Texture2D texture;
        public Rectangle[] frames;
        public int currentframe = 0;
        public int curfrm = 0;

        public Sprite(Texture2D texture, int framecount, int framewidth, int frameheight, int framesperrow, int framespercolumn)
        {
            frames = new Rectangle[framecount];
            this.texture = texture;
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

        public Sprite(Texture2D texture, Rectangle[] frames)
        {
            this.frames = frames;
            this.texture = texture;
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
