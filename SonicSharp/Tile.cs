using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SonicSharp
{
    public class Tileset
    {
        public Texture2D texture;
        public List<Dictionary<string, string>> tileproperties = new List<Dictionary<string, string>>();
        public List<Rectangle> tilesetparts = new List<Rectangle>();

        public Tileset(Texture2D texture) { this.texture = texture; }
    }

    public class Tile
    {
        public int textureid, tileid;
        public Rectangle tilesetsection;
        public Vector2 pos = Vector2.Zero;

        public Tile(int textureid, int tileid, Rectangle tilesetsection, Vector2 pos) { this.textureid = textureid; this.tileid = tileid; this.tilesetsection = tilesetsection; this.pos = pos; }

        public void Draw()
        {
            Main.spriteBatch.Draw(texture: Level.tilesets[textureid].texture, position: pos, sourceRectangle: tilesetsection);
        }
    }
}
