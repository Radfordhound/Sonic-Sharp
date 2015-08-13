using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SonicSharp
{
    /// <summary>
    /// A custom font class.
    ///     "Cuz' SpriteFonts stink!" - Radfordhound, 2015
    /// </summary>
    public class Font
    {
        Texture2D texture;
        Rectangle[] chartexturepieces;
        char[] characters;

        public Font(Texture2D texture, char[] characters, int charsperrow, int charspercolumn, int charwidth, int charheight)
        {
            int i = 0;
            chartexturepieces = new Rectangle[characters.Length];
            this.characters = characters;
            this.texture = texture;

            for (int y = 0; y < charspercolumn*charheight; y+= charheight)
            {
                for (int x = 0; x < charsperrow*charwidth; x+= charwidth)
                {
                    chartexturepieces[i] = new Rectangle(x,y,charwidth,charheight);
                    i++;
                }
            }
        }

        public Font(Texture2D texture, char[] characters, Rectangle[] chartexturepieces)
        {
            this.texture = texture;
            this.characters = characters;
            this.chartexturepieces = chartexturepieces;
        }

        public int GetWidth(string text)
        {
            int width = 0;
            for (int i = 0; i < text.Length; i++)
            {
                width += chartexturepieces[findcharindex(text[i])].Width + 1;
            }
            return width;
        }

        public int GetHeight(string text)
        {
            int height = 0;
            for (int i = 0; i < text.Length; i++)
            {
                height += chartexturepieces[findcharindex(text[i])].Height + 1;
            }
            return height;
        }

        public void Draw(string text, float x, float y)
        {
            float newx = x, newy = y;

            for (int i = 0; i < text.Length; i++)
            {
                Main.spriteBatch.Draw(texture: texture, position: new Vector2(newx, newy), sourceRectangle: chartexturepieces[findcharindex(text[i])]);

                if ((newx + chartexturepieces[findcharindex(text[i])].Width+1)*Main.scalemodifier < Camera.pos.X + Program.game.Window.ClientBounds.Width)
                {
                    newx += chartexturepieces[findcharindex(text[i])].Width+1;
                }
                else
                {
                    newy += chartexturepieces[findcharindex(text[i])].Height+1;
                    newx = x;
                }
            }
        }

        private int findcharindex(char ch)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] == ch) { return i; }
            }
            return 0;
        }
    }
}
