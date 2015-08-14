using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonicSharp
{
    public static class Level
    {
        public static List<Texture2D> tilesets = new List<Texture2D>();
        public static List<Tile> tiles = new List<Tile>();

        public static void Load()
        {
            //TODO: Add code to load level
            for (int i = 0; i < Main.players.Count; i++)
            {
                Main.players[i].active = true;
            }
        }

        public static void Update()
        {
            for (int i = 0; i < Main.players.Count; i++)
            {
                Main.players[i].Update();
            }
        }

        public static void Draw()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw();
            }

            for (int i = 0; i < Main.players.Count; i++)
            {
                Main.players[i].Draw();
            }
        }
    }
}
