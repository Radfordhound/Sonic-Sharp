using System.IO;

namespace SonicSharp
{
    /// <summary>
    /// A collection of 64 Tiles (8 per row) which can be placed within a stage.
    /// </summary>
    public class Block
    {
        // Variables/Constants
        public ushort[] Tiles = new ushort[TileCount];
        public const uint TileCount = 64, TilesPerRow = 8,
            BlockSize = (TileCount * Tile.TileSize) / TilesPerRow;

        // Constructors
        public Block() { }
        public Block(BinaryReader reader)
        {
            Read(reader);
        }

        // Methods
        public virtual void Read(BinaryReader reader)
        {
            for (uint i = 0; i < TileCount; ++i)
            {
                Tiles[i] = reader.ReadUInt16();
            }
        }

        public virtual void Write(BinaryWriter writer)
        {
            for (uint i = 0; i < TileCount; ++i)
            {
                writer.Write(Tiles[i]);
            }
        }
    }
}