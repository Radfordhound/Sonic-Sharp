using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;

namespace SonicSharp
{
    class Level
    {
        public static string name;
        public static List<Texture2D> tiletextures = new List<Texture2D>();
        public static List<Tile> tiles = new List<Tile>();

        public static Point playerstartsonic;
        public static Point playerstarttails;
        public static Point playerstartknuckles;

        public static void Load(string fn,ContentManager Content)
        {
            string lvldir = Main.dir + "\\Levels\\" + fn + "\\" + fn;

            //Load the level from a given file name.
            if (Directory.Exists(Main.dir + "\\Levels") && File.Exists(lvldir + ".lvl") && File.Exists(lvldir + ".tiles"))
            {
                //Clear stuff from the last level loaded.
                tiles.Clear();
                Level.tiletextures.Clear();

                Loadlvltxt(lvldir);
                Loadlvltiles(lvldir,Content,fn);
            }
        }

        private static HeightMap Loadlvlhms(string file)
        {
            if (File.Exists(file))
            {
                return new HeightMap(File.ReadAllLines(file).Select(int.Parse).ToList());
            }
            return new HeightMap(new List<int> { 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16 });
        }

        private static void LoadlvlTexture(ContentManager Content,string fn,int tlcnt)
        {
            Content.RootDirectory = "Levels\\" + fn;

            List<string> supportedextensions = new List<string> { "bmp", "dds", "dib", "hdr", "jpg", "pfm", "png", "ppm", "tga"};

            foreach(string ext in supportedextensions)
            {
                if (File.Exists(string.Format("{0}\\Levels\\{1}\\Tiles\\sprs\\{2}.{3}",Main.dir,fn,tlcnt,ext)))
                {
                    Level.tiletextures.Add(Content.Load<Texture2D>("Tiles\\sprs\\" + tlcnt));
                    break;
                }
            }

            Content.RootDirectory = "Levels";
        }

        private static void Loadlvltxt(string lvldir)
        {
            //Load the level's main data file.
            string lvltxt = File.ReadAllText(lvldir + ".lvl");

            //Get the data from the .lvl file.
            using (XmlReader reader = XmlReader.Create(new StringReader(lvltxt)))
            {
                reader.ReadToFollowing("level");
                reader.MoveToFirstAttribute();
                name = reader.Value;

                reader.ReadToFollowing("playerstarts");
                reader.ReadToFollowing("sonic");

                reader.MoveToFirstAttribute();
                playerstartsonic.X = Convert.ToInt32(reader.Value);
                reader.MoveToNextAttribute();
                playerstartsonic.Y = Convert.ToInt32(reader.Value);

                reader.ReadToFollowing("tails");

                reader.MoveToFirstAttribute();
                playerstarttails.X = Convert.ToInt32(reader.Value);
                reader.MoveToNextAttribute();
                playerstarttails.Y = Convert.ToInt32(reader.Value);

                reader.ReadToFollowing("knuckles");

                reader.MoveToFirstAttribute();
                playerstartknuckles.X = Convert.ToInt32(reader.Value);
                reader.MoveToNextAttribute();
                playerstartknuckles.Y = Convert.ToInt32(reader.Value);
            }
        }

        private static void Loadlvltiles(string lvldir,ContentManager Content,string fn)
        {
            //Load the level's tile positions.
            string lvltiletxt = File.ReadAllText(lvldir + ".tiles");

            HeightMap hm = Loadlvlhms(lvldir + "\\HMs");

            //Get the data from the .tiles file.
            using (XmlReader reader = XmlReader.Create(new StringReader(lvltiletxt)))
            {
                int tilecnt = 0;
                while (reader.Read())
                {
                    //Load the tile's data
                    if (reader.NodeType == XmlNodeType.Element && ((IXmlLineInfo)reader).LineNumber != 1)
                    {
                        reader.MoveToFirstAttribute();
                        int tlposx = Convert.ToInt32(reader.Value);

                        reader.MoveToNextAttribute();
                        int tlposy = Convert.ToInt32(reader.Value);

                        reader.MoveToNextAttribute();
                        int tltex = Convert.ToInt32(reader.Value);

                        //Load textures
                        if (tiletextures.Count - 1 < tltex)
                        {
                            LoadlvlTexture(Content, fn, tltex);
                        }

                        //Make the tiles using the loaded data.
                        tiles.Add(new Tile(new Vector2(tlposx, tlposy), tiletextures[tltex], Loadlvlhms(Main.dir + "\\Levels\\" + fn + "\\Tiles\\HMs\\" + tltex.ToString() + ".hm")));
                        tilecnt++;
                    }
                }
            }
        }

        public static void UnLoad()
        {
            tiles.Clear();
            tiletextures.Clear();
            Main.tilecm.Unload();
        }
    }
}
