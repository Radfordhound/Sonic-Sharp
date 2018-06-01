using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SonicSharp
{
    public struct Tile
    {
        // Variables/Constants
        public Rectangle MapRectangle;
        public ushort H1, H2, H3, H4, H5;
        public ushort W1, W2, W3, W4, W5;
        public byte Angle;

        public const uint TileSize = 16;

        // Constructors
        public Tile(Rectangle mapRect, byte angle = 0, ushort h1 = 0xFFFF,
            ushort h2 = 0xFFFF, ushort h3 = 0xFFFF, ushort h4 = 0xFFFF, ushort h5 = 0xFFFF,
            ushort w1 = 0xFFFF, ushort w2 = 0xFFFF, ushort w3 = 0xFFFF, ushort w4 = 0xFFFF,
            ushort w5 = 0xFFFF)
        {
            MapRectangle = mapRect;
            Angle = angle;
            H1 = h1; H2 = h2; H3 = h3; H4 = h4; H5 = h5;
            W1 = w1; W2 = w2; W3 = w3; W4 = w4; W5 = w5;
        }

        public Tile(int mapIndex, Texture2D tileMap, byte angle = 0, ushort h1 = 0xFFFF,
            ushort h2 = 0xFFFF, ushort h3 = 0xFFFF, ushort h4 = 0xFFFF, ushort h5 = 0xFFFF,
            ushort w1 = 0xFFFF, ushort w2 = 0xFFFF, ushort w3 = 0xFFFF, ushort w4 = 0xFFFF,
            ushort w5 = 0xFFFF)
        {
            mapIndex *= (int)TileSize;
            if (mapIndex >= tileMap.Width)
            {
                int h = (mapIndex / tileMap.Width);
                mapIndex -= tileMap.Width * h;

                MapRectangle = new Rectangle(mapIndex, h * (int)TileSize,
                    (int)TileSize, (int)TileSize);
            }
            else
            {
                MapRectangle = new Rectangle(mapIndex, 0,
                    (int)TileSize, (int)TileSize);
            }

            Angle = angle;
            H1 = h1; H2 = h2; H3 = h3; H4 = h4; H5 = h5;
            W1 = w1; W2 = w2; W3 = w3; W4 = w4; W5 = w5;
        }

        // Methods
        // TODO

        public bool CheckWidthMap(byte index)
        {
            // TODO
            return true;
        }
    }
}