using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SonicSharp
{
    /// <summary>
    /// A playable level made up of a bunch of blocks (groups of tiles)
    /// on two different "layers": The background layer and the foreground layer.
    /// </summary>
    public class Stage
    {
        // Variables/Constants
        public Texture2D TileMap;
        public uint RowCount { get; protected set; }
        public uint ColumnCount { get; protected set; }

        public List<Block> Blocks = new List<Block>();
        public List<Tile> Tiles = new List<Tile>();
        protected byte[,] background, foreground;

        public const string Extension = ".sharp-stage";
        public const uint Signature = 0x475453;
        public const uint DefaultRowCount = 16, DefaultColumnCount = 128;
        public const byte Version = 0; // DEBUG, will change to 1 later.

        // Constructors
        public Stage(Texture2D tileMap, uint rowCount = DefaultRowCount,
            uint columnCount = DefaultColumnCount) : this(rowCount, columnCount)
        {
            TileMap = tileMap;
        }

        public Stage(uint rowCount = DefaultRowCount,
            uint columnCount = DefaultColumnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;

            background = new byte[rowCount, columnCount];
            foreground = new byte[rowCount, columnCount];
        }

        // Methods
        public virtual void SetBlockIndex(uint row, uint column,
            byte index, bool onBackground = false)
        {
            if (row >= RowCount)
                throw new ArgumentOutOfRangeException("row");

            if (column >= ColumnCount)
                throw new ArgumentOutOfRangeException("column");

            if (onBackground)
            {
                background[row, column] = index;
            }
            else
            {
                foreground[row, column] = index;
            }
        }

        public virtual Block GetBlock(byte index)
        {
            return (index == 0) ? null : Blocks[index - 1];
        }

        public virtual Block GetBlock(uint row, uint column,
            bool fromBackground = false)
        {
            byte index = GetBlockIndex(row, column, fromBackground);
            return GetBlock(index);
        }

        public virtual byte GetBlockIndex(uint row, uint column,
            bool fromBackground = false)
        {
            if (row >= RowCount)
                throw new ArgumentOutOfRangeException("row");

            if (column >= ColumnCount)
                throw new ArgumentOutOfRangeException("column");

            return (fromBackground) ? background[row, column] :
                foreground[row, column];
        }

        public virtual void Load(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            {
                Load(fs);
            }
        }

        public virtual void Load(Stream fs)
        {
            var reader = new BinaryReader(fs, Encoding.UTF8);
            uint sig = reader.ReadUInt32();
            byte ver = (byte)((sig & 0xFF000000) >> 24);
            sig &= 0xFFFFFF;

            if (sig != Signature)
            {
                throw new InvalidDataException(
                    "The given file is not a valid SoniC# stage!");
            }

            if (ver > Version)
            {
                throw new NotImplementedException(
                    $"The given SoniC# stage is of an unsupported version (v{ver})!");
            }

            // Tiles
            int i, count; // Two generic ints we reuse
            count = reader.ReadInt32();
            Tiles.Clear();

            for (i = 0; i < count; ++i)
            {
                Tiles.Add(new Tile(i, TileMap, reader));
            }

            // Blocks
            count = reader.ReadInt32();
            Blocks.Clear();

            for (i = 0; i < count; ++i)
            {
                Blocks.Add(new Block(reader));
            }

            // Layers
            uint i2, i3;
            uint rowCount = reader.ReadUInt32();
            uint columnCount = reader.ReadUInt32();

            if (rowCount != RowCount || columnCount != ColumnCount)
            {
                background = new byte[rowCount, columnCount];
                foreground = new byte[rowCount, columnCount];
            }

            ReadLayer(background);
            ReadLayer(foreground);

            // Sub-Methods
            void ReadLayer(byte[,] layer)
            {
                // Read the list of rows present in the file
                uint presentRowsLen = Bitwise.GetRequiredBytes(rowCount);
                var presentRows = reader.ReadBytes((int)presentRowsLen);
                byte presentRowsBitIndex = 0;
                i = 0;

                // Read each row
                for (i2 = 0; i2 < rowCount; ++i2)
                {
                    // Read each column if the row it belongs to is present in the file
                    if ((presentRows[i] & (0x80 >> presentRowsBitIndex)) != 0)
                    {
                        columnCount = reader.ReadUInt32();
                        for (i3 = 0; i3 < columnCount; ++i3)
                        {
                            layer[i2, i3] = reader.ReadByte();
                        }
                    }

                    if (++presentRowsBitIndex > 7)
                    {
                        presentRowsBitIndex = 0;
                        ++i;
                    }
                }
            }
        }

        public virtual void Save(string filePath)
        {
            using (var fs = File.Create(filePath))
            {
                Save(fs);
            }
        }

        public virtual void Save(Stream fs)
        {
            var writer = new BinaryWriter(fs, Encoding.UTF8);
            writer.Write(Signature | (Version << 24));

            // Tiles
            int i, count = Tiles.Count; // Two generic ints we reuse
            writer.Write(count);

            for (i = 0; i < count; ++i)
            {
                Tiles[i].Write(writer);
            }

            // Blocks
            count = Blocks.Count;
            writer.Write(count);

            for (i = 0; i < count; ++i)
            {
                Blocks[i].Write(writer);
            }

            // Layers
            uint i2, i3;
            writer.Write(RowCount);
            writer.Write(ColumnCount);

            WriteLayer(background);
            WriteLayer(foreground);

            // Sub-Methods
            void WriteLayer(byte[,] layer)
            {
                // We're going to write a byte array called presentRows, with each bit
                // representing whether the corresponding layer is present in the file.
                long pos = fs.Position;
                byte presentRowsBitIndex = 0;
                i = 0;

                uint presentRowsLen = Bitwise.GetRequiredBytes(RowCount);
                var presentRows = new byte[presentRowsLen];
                writer.Write(presentRows);

                for (i2 = 0; i2 < RowCount; ++i2)
                {
                    // Do a very basic cutoff check to avoid writing tons of nulls
                    uint columnCount = 0;
                    for (i3 = 0; i3 < ColumnCount; ++i3)
                    {
                        if (layer[i2, i3] != 0)
                            columnCount = i3 + 1;
                    }

                    if (columnCount != 0)
                    {
                        // Set the appropriate bit stating this row is present
                        presentRows[i] |= (byte)(0x80 >> presentRowsBitIndex);

                        // Write the block indices that make up the column
                        writer.Write(columnCount);
                        for (i3 = 0; i3 < columnCount; ++i3)
                        {
                            writer.Write(layer[i2, i3]);
                        }
                    }

                    if (++presentRowsBitIndex > 7)
                    {
                        presentRowsBitIndex = 0;
                        ++i;
                    }
                }

                // Write presentRows
                long pos2 = fs.Position;
                fs.Position = pos;
                writer.Write(presentRows);
                fs.Position = pos2;
            }
        }

        public virtual void Draw(Camera2D cam)
        {
            var bounds = cam.BoundingRectangle;
            bounds.Inflate(Block.BlockSize, Block.BlockSize);

            // Draw Blocks
            if (TileMap != null)
            {
                // Calculate Camera Boundries so we only draw what's necessary
                int bl = (int)(bounds.Left / Block.BlockSize);
                int br = (int)(bounds.Right / Block.BlockSize);
                int bt = (int)(bounds.Top / Block.BlockSize);
                int bb = (int)(bounds.Bottom / Block.BlockSize);

                if (bl < 0)
                    bl = 0;

                if (bt < 0)
                    bt = 0;

                if (br > ColumnCount)
                    br = (int)ColumnCount;

                if (bb > RowCount)
                    bb = (int)RowCount;

                // Draw Background/Foreground layers
                Vector2 pos;
                int blocksPerColumn = (br - bl);

                DrawLayer(background);
                DrawLayer(foreground);

                // Sub-Methods
                void DrawLayer(byte[,] layer)
                {
                    // Draw each block currently within the on-screen boundries
                    byte blockIndex;
                    pos = new Vector2(bl * Block.BlockSize, bt * Block.BlockSize);

                    for (int row = bt; row < bb; ++row)
                    {
                        for (int column = bl; column < br; ++column)
                        {
                            blockIndex = layer[row, column];
                            if (blockIndex != 0)
                                DrawBlock(Blocks[--blockIndex], pos);

                            pos.X += Block.BlockSize;
                        }

                        pos.X -= (Block.BlockSize * blocksPerColumn);
                        pos.Y += Block.BlockSize;
                    }
                }
            }

            // TODO: Draw Objects

            // Draw Players
            foreach (var plr in GameWindow.LocalPlayers)
            {
                if (plr != null && bounds.Contains(plr.Position))
                    plr.Draw();
            }

            // TODO: Draw Online Players too
        }

        public virtual void DrawBlock(Block block, Vector2 pos)
        {
            // Draw each tile within the block
            for (uint i = 0; i < Block.TileCount;)
            {
                for (uint x = 0; x < Block.TilesPerRow; ++x)
                {
                    DrawTile(block.Tiles[i], pos);
                    pos.X += Tile.TileSize;
                    ++i;
                }

                pos.X -= (Block.TilesPerRow * Tile.TileSize);
                pos.Y += Tile.TileSize;
            }
        }

        public virtual void DrawTile(ushort tileIndex, Vector2 pos)
        {
            // Draw the tile
            GameWindow.SpriteBatch.Draw(TileMap, pos,
                Tiles[tileIndex].MapRectangle, Color.White);
        }
    }
}