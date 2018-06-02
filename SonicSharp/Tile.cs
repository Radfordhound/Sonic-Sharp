using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace SonicSharp
{
    /// <summary>
    /// A 16x16 drawable entity which contains collision that
    /// gets used within "blocks" to make up a stage.
    /// </summary>
    public struct Tile
    {
        // Variables/Constants
        public Rectangle MapRectangle { get; private set; }

        /*
         * Height/Width Maps work like this:
         * http://info.sonicretro.org/SPG:Solid_Tiles#Height_Masks
         * 
         * Put simply, they're an array of 16 values, one for each "column" in
         * a tile from left-to-right, each one being a value ranging from 0-16 which
         * represents how "tall" that column is. They're used for slopes, etc.
         * 
         * Due to the nature of binary, a minimum of 5 bits are required to represent
         * the number 16. So in an effort to be as efficient as possible, SoniC#
         * stores these height/width maps as 5 unsigned shorts (16 bit values) like so:
         * 
         * H1:  AAAA ABBB BBCC CCCD
         * H2:  DDDD EEEE EFFF FFGG
         * H3:  GGGH HHHH IIII IJJJ
         * H4:  JJKK KKKL LLLL MMMM
         * H5:  MNNN NNOO OOOP PPPP
         * 
         * Where every 5 letters represents the 5 bits required to represent one column.
         */
        public ushort H1, H2, H3, H4, H5;
        public ushort W1, W2, W3, W4, W5;
        public byte Angle;

        /// <summary>
        /// Mask used to get the first column in a height/width map.
        /// 0xF800 == AAAA A000 0000 0000
        /// </summary>
        public const ushort MaskA = 0xF800;

        /// <summary>
        /// Mask used to get the second column in a height/width map.
        /// 0x7C0 == 0000 0BBB BB00 0000
        /// </summary>
        public const ushort MaskB = 0x7C0;

        /// <summary>
        /// Mask used to get the third column in a height/width map.
        /// 0x3E == 0000 0000 00CC CCC0
        /// </summary>
        public const ushort MaskC = 0x3E;

        /// <summary>
        /// Mask used to get the first part of the fourth column in a height/width map.
        /// 1 == 0000 0000 0000 000D
        /// </summary>
        public const ushort MaskD1 = 1;

        /// <summary>
        /// Mask used to get the second part of the fourth column in a height/width map.
        /// 0xF000 == DDDD 0000 0000 0000
        /// </summary>
        public const ushort MaskD2 = 0xF000;

        /// <summary>
        /// Mask used to get the fifth column in a height/width map.
        /// 0xF80 == 0000 EEEE E000 0000
        /// </summary>
        public const ushort MaskE = 0xF80;

        /// <summary>
        /// Mask used to get the sixth column in a height/width map.
        /// 0x7C == 0000 0000 0FFF FF00
        /// </summary>
        public const ushort MaskF = 0x7C;

        /// <summary>
        /// Mask used to get the first part of the seventh column in a height/width map.
        /// 3 == 0000 0000 0000 00GG
        /// </summary>
        public const ushort MaskG1 = 3;

        /// <summary>
        /// Mask used to get the second part of the seventh column in a height/width map.
        /// 0xE000 == GGG0 0000 0000 0000
        /// </summary>
        public const ushort MaskG2 = 0xE000;

        /// <summary>
        /// Mask used to get the eighth column in a height/width map.
        /// 0x1F00 == 000H HHHH 0000 0000
        /// </summary>
        public const ushort MaskH = 0x1F00;

        /// <summary>
        /// Mask used to get the ninth column in a height/width map.
        /// 0xF8 == 0000 0000 IIII I000
        /// </summary>
        public const ushort MaskI = 0xF8;

        /// <summary>
        /// Mask used to get the first part of the tenth column in a height/width map.
        /// 7 == 0000 0000 0000 0JJJ
        /// </summary>
        public const ushort MaskJ1 = 7;

        /// <summary>
        /// Mask used to get the second part of the tenth column in a height/width map.
        /// 0xC000 == JJ00 0000 0000 0000
        /// </summary>
        public const ushort MaskJ2 = 0xC000;

        /// <summary>
        /// Mask used to get the eleventh column in a height/width map.
        /// 0x3E00 == 00KK KKK0 0000 0000
        /// </summary>
        public const ushort MaskK = 0x3E00;

        /// <summary>
        /// Mask used to get the twelfth column in a height/width map.
        /// 0x1F0 == 0000 000L LLLL 0000
        /// </summary>
        public const ushort MaskL = 0x1F0;

        /// <summary>
        /// Mask used to get the first part of the thirteenth column in a height/width map.
        /// 0xF == 0000 0000 0000 MMMM
        /// </summary>
        public const ushort MaskM1 = 0xF;

        /// <summary>
        /// Mask used to get the second part of the thirteenth column in a height/width map.
        /// 0x8000 == M000 0000 0000 0000
        /// </summary>
        public const ushort MaskM2 = 0x8000;

        /// <summary>
        /// Mask used to get the fourteenth column in a height/width map.
        /// 0x7C00 == 0NNN NN00 0000 0000
        /// </summary>
        public const ushort MaskN = 0x7C00;

        /// <summary>
        /// Mask used to get the fifteenth column in a height/width map.
        /// 0x3E0 == 0000 00OO OOO0 0000
        /// </summary>
        public const ushort MaskO = 0x3E0;

        /// <summary>
        /// Mask used to get the sixteenth column in a height/width map.
        /// 0x1F == 0000 0000 000P PPPP
        /// </summary>
        public const ushort MaskP = 0x1F;

        /// <summary>
        /// The width and height of each tile. To change this without breaking
        /// anything, the height/width map system must also be changed.
        /// </summary>
        public const uint TileSize = 16;
        private const ushort max1 = 0x8421, max2 = 0x842,
            max3 = 0x1084, max4 = 0x2108, max5 = 0x4210;

        // Constructors
        public Tile(Rectangle mapRect, byte angle = 0, ushort h1 = max1,
            ushort h2 = max2, ushort h3 = max3, ushort h4 = max4, ushort h5 = max5,
            ushort w1 = max1, ushort w2 = max2, ushort w3 = max3, ushort w4 = max4,
            ushort w5 = max5)
        {
            MapRectangle = mapRect;
            Angle = angle;
            H1 = h1; H2 = h2; H3 = h3; H4 = h4; H5 = h5;
            W1 = w1; W2 = w2; W3 = w3; W4 = w4; W5 = w5;
        }

        public Tile(int mapIndex, Texture2D tileMap, byte angle = 0, ushort h1 = max1,
            ushort h2 = max2, ushort h3 = max3, ushort h4 = max4, ushort h5 = max5,
            ushort w1 = max1, ushort w2 = max2, ushort w3 = max3, ushort w4 = max4,
            ushort w5 = max5)
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

        public Tile(int mapIndex, Texture2D tileMap, BinaryReader reader)
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

            // Read the height/width map and angle
            H1 = reader.ReadUInt16();
            H2 = reader.ReadUInt16();
            H3 = reader.ReadUInt16();
            H4 = reader.ReadUInt16();
            H5 = reader.ReadUInt16();

            W1 = reader.ReadUInt16();
            W2 = reader.ReadUInt16();
            W3 = reader.ReadUInt16();
            W4 = reader.ReadUInt16();
            W5 = reader.ReadUInt16();

            Angle = reader.ReadByte();
        }

        // Methods
        public byte GetHeight(byte index)
        {
            switch (index)
            {
                case 0:
                    return (byte)((H1 & MaskA) >> 11);

                case 1:
                    return (byte)((H1 & MaskB) >> 6);

                case 2:
                    return (byte)((H1 & MaskC) >> 1);

                case 3:
                    return (byte)(((H1 & MaskD1) << 4) | ((H2 & MaskD2) >> 12));

                case 4:
                    return (byte)((H2 & MaskE) >> 7);

                case 5:
                    return (byte)((H2 & MaskF) >> 2);

                case 6:
                    return (byte)(((H2 & MaskG1) << 3) | ((H3 & MaskG2) >> 13));

                case 7:
                    return (byte)((H3 & MaskH) >> 8);

                case 8:
                    return (byte)((H3 & MaskI) >> 3);

                case 9:
                    return (byte)(((H3 & MaskJ1) << 2) | ((H4 & MaskJ2) >> 14));

                case 10:
                    return (byte)((H4 & MaskK) >> 9);

                case 11:
                    return (byte)((H4 & MaskL) >> 4);

                case 12:
                    return (byte)(((H4 & MaskM1) << 1) | ((H5 & MaskM2) >> 15));

                case 13:
                    return (byte)((H5 & MaskN) >> 10);

                case 14:
                    return (byte)((H5 & MaskO) >> 5);

                case 15:
                    return (byte)(H5 & MaskP);

                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        public byte GetWidth(byte index)
        {
            switch (index)
            {
                case 0:
                    return (byte)((W1 & MaskA) >> 11);

                case 1:
                    return (byte)((W1 & MaskB) >> 6);

                case 2:
                    return (byte)((W1 & MaskC) >> 1);

                case 3:
                    return (byte)(((W1 & MaskD1) << 4) | ((W2 & MaskD2) >> 12));

                case 4:
                    return (byte)((W2 & MaskE) >> 7);

                case 5:
                    return (byte)((W2 & MaskF) >> 2);

                case 6:
                    return (byte)(((W2 & MaskG1) << 3) | ((W3 & MaskG2) >> 13));

                case 7:
                    return (byte)((W3 & MaskH) >> 8);

                case 8:
                    return (byte)((W3 & MaskI) >> 3);

                case 9:
                    return (byte)(((W3 & MaskJ1) << 2) | ((W4 & MaskJ2) >> 14));

                case 10:
                    return (byte)((W4 & MaskK) >> 9);

                case 11:
                    return (byte)((W4 & MaskL) >> 4);

                case 12:
                    return (byte)(((W4 & MaskM1) << 1) | ((W5 & MaskM2) >> 15));

                case 13:
                    return (byte)((W5 & MaskN) >> 10);

                case 14:
                    return (byte)((W5 & MaskO) >> 5);

                case 15:
                    return (byte)(W5 & MaskP);

                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        public bool CheckHeightMap(byte index)
        {
            switch (index)
            {
                case 0:
                    return (H1 & MaskA) != 0;

                case 1:
                    return (H1 & MaskB) != 0;

                case 2:
                    return (H1 & MaskC) != 0;

                case 3:
                    return ((H1 & MaskD1) | (H2 & MaskD2)) != 0;

                case 4:
                    return (H2 & MaskE) != 0;

                case 5:
                    return (H2 & MaskF) != 0;

                case 6:
                    return ((H2 & MaskG1) | (H3 & MaskG2)) != 0;

                case 7:
                    return (H3 & MaskH) != 0;

                case 8:
                    return (H3 & MaskI) != 0;

                case 9:
                    return ((H3 & MaskJ1) | (H4 & MaskJ2)) != 0;

                case 10:
                    return (H4 & MaskK) != 0;

                case 11:
                    return (H4 & MaskL) != 0;

                case 12:
                    return ((H4 & MaskM1) | (H5 & MaskM2)) != 0;

                case 13:
                    return (H5 & MaskN) != 0;

                case 14:
                    return (H5 & MaskO) != 0;

                case 15:
                    return (H5 & MaskP) != 0;

                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        public bool CheckWidthMap(byte index)
        {
            switch (index)
            {
                case 0:
                    return (W1 & MaskA) != 0;

                case 1:
                    return (W1 & MaskB) != 0;

                case 2:
                    return (W1 & MaskC) != 0;

                case 3:
                    return ((W1 & MaskD1) | (W2 & MaskD2)) != 0;

                case 4:
                    return (W2 & MaskE) != 0;

                case 5:
                    return (W2 & MaskF) != 0;

                case 6:
                    return ((W2 & MaskG1) | (W3 & MaskG2)) != 0;

                case 7:
                    return (W3 & MaskH) != 0;

                case 8:
                    return (W3 & MaskI) != 0;

                case 9:
                    return ((W3 & MaskJ1) | (W4 & MaskJ2)) != 0;

                case 10:
                    return (W4 & MaskK) != 0;

                case 11:
                    return (W4 & MaskL) != 0;

                case 12:
                    return ((W4 & MaskM1) | (W5 & MaskM2)) != 0;

                case 13:
                    return (W5 & MaskN) != 0;

                case 14:
                    return (W5 & MaskO) != 0;

                case 15:
                    return (W5 & MaskP) != 0;

                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(H1);
            writer.Write(H2);
            writer.Write(H3);
            writer.Write(H4);
            writer.Write(H5);

            writer.Write(W1);
            writer.Write(W2);
            writer.Write(W3);
            writer.Write(W4);
            writer.Write(W5);

            writer.Write(Angle);
        }
    }
}