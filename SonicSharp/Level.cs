using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NTiled;
using System.IO;

namespace SonicSharp
{
    public static class Level
    {
        public static List<Tileset> tilesets = new List<Tileset>();
        public static List<Tile> tiles = new List<Tile>();
        public static int onscreentilecount = 0;

        public static void Load(string leveldir, string filename)
        {
            string fullname = Main.startdir + "\\Levels\\" + leveldir + "\\" + filename;
            if (File.Exists(fullname))
            {
                //Load the map using NTiled
                XDocument document = XDocument.Load(fullname);
                TiledMap map = new TiledReader().Read(document);

                //Tilesets
                foreach (TiledTileset tileset in map.Tilesets)
                {
                    if (tileset.Image != null && File.Exists(Main.startdir + "\\Levels\\" + leveldir + "\\" + tileset.Image.Source))
                    {
                        Program.game.Content.RootDirectory = "Levels";
                        Tileset ts = new Tileset(Program.game.Content.Load<Texture2D>(leveldir + "\\" + new FileInfo(tileset.Image.Source).Name));

                        //Generate all the tile rectangles within the tileset.
                        int i = 0;
                        for (int y = 0; y < tileset.Image.Height; y+=16)
                        {
                            for (int x = 0; x < tileset.Image.Width; x+=16)
                            {
                                ts.tilesetparts.Add(new Rectangle(x,y,16,16));
                                foreach (TiledTile tile in tileset.Tiles) { if (tile.Id == i && tile.Properties.Count > 0) { ts.tileproperties.Add(tile.Properties);} }
                                i++;
                            }
                        }

                        tilesets.Add(ts);
                    }
                }

                //Layers
                foreach (TiledLayer layer in map.Layers)
                {
                    TiledTileLayer tlayer = layer as TiledTileLayer;

                    if (tlayer != null)
                    {
                        int i = 0, tilesetid = 0;
                        for (int y = 0; y < layer.Height * 16; y += 16)
                        {
                            for (int x = 0; x < layer.Width * 16; x += 16)
                            {
                                if (tlayer.Tiles[i] != 0)
                                {
                                    //TODO: Find the correct tileset for each tile.
                                    //OLD CODE:

                                    //if (!tilesets[tilesetid].tilesetparts.Count > !tilesets[tilesetid].tileids.Contains(tlayer.Tiles[i]))
                                    //{
                                    //    Console.WriteLine(tlayer.Tiles[i]);
                                    //    //tilesetid = -1;
                                    //    for (int tsi = 0; tsi < tilesets.Count; tsi++)
                                    //    {
                                    //        if (tilesets[tsi].tileids.Contains(tlayer.Tiles[i])) { tilesetid = tsi; break; }
                                    //    }
                                    //}
                                    //else { Console.WriteLine(tlayer.Tiles[i]); }

                                    //Spawn all the tiles
                                    if (tilesetid != -1)
                                    {
                                        tiles.Add(new Tile(tilesetid, tilesets[tilesetid].tilesetparts[tlayer.Tiles[i] - 1], new Vector2(x, y)));
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }

                foreach (Player plr in Main.players)
                {
                    if (plr.GetType() == typeof(Sonic) && map.Properties["Sonic Player Start"] != null)
                    {
                        plr.pos = new Vector2(Convert.ToSingle(map.Properties["Sonic Player Start"].Split(',')[0]), Convert.ToSingle(map.Properties["Sonic Player Start"].Split(',')[1]));
                    }
                    else if (plr.GetType() == typeof(Tails) && map.Properties["Tails Player Start"] != null)
                    {
                        plr.pos = new Vector2(Convert.ToSingle(map.Properties["Tails Player Start"].Split(',')[0]), Convert.ToSingle(map.Properties["Tails Player Start"].Split(',')[1]));
                    }
                    else if (plr.GetType() == typeof(Knuckles) && map.Properties["Knuckles Player Start"] != null)
                    {
                        plr.pos = new Vector2(Convert.ToSingle(map.Properties["Knuckles Player Start"].Split(',')[0]), Convert.ToSingle(map.Properties["Knuckles Player Start"].Split(',')[1]));
                    }

                    plr.active = true;
                }

                Camera.pos.X = (304*16)*2; Camera.pos.Y = (55 * 16) * 2; //TODO: Remove this temporary line.
                Program.game.Content.RootDirectory = "Content";
            }
            else
            {
                MessageBox.Show("ERROR: The given level (\"" + filename + "\") does not exist!","SoniC#",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public static void UnLoad()
        {
            //De-activate all the players
            for (int i = 0; i < Main.players.Count; i++)
            {
                Main.players[i].active = false;
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
            if (Main.debugmode) { onscreentilecount = 0; }

            //Draw the tiles...
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].pos.X + 32 >= Camera.pos.X / Main.scalemodifier && tiles[i].pos.X - 32 <= Camera.pos.X/Main.scalemodifier + Program.game.Window.ClientBounds.Width / Main.scalemodifier && tiles[i].pos.Y + 32 >= Camera.pos.Y / Main.scalemodifier && tiles[i].pos.Y - 32 <= Camera.pos.Y / Main.scalemodifier + Program.game.Window.ClientBounds.Height / Main.scalemodifier)
                {
                    tiles[i].Draw();
                    if (Main.debugmode) { onscreentilecount++; }
                }
            }

            //Then the players.
            for (int i = 0; i < Main.players.Count; i++)
            {
                Main.players[i].Draw();
            }
        }
    }
}
