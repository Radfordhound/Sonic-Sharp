using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

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

        public const uint DefaultRowCount = 16, DefaultColumnCount = 128;

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