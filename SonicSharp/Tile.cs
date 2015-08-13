using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SonicSharp
{
    public class Tile
    {
        public int textureid;
        public Rectangle tilesetsection;
        public Vector2 pos = Vector2.Zero;

        public Tile(int textureid, Rectangle tilesetsection, Vector2 pos) { this.textureid = textureid; this.tilesetsection = tilesetsection; this.pos = pos; }

        public void Draw()
        {
            Main.spriteBatch.Draw(texture: Level.tilesets[textureid], position: pos, sourceRectangle: tilesetsection);
        }
    }
}
