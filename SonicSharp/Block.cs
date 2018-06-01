namespace SonicSharp
{
    public class Block
    {
        // Variables/Constants
        public ushort[] Tiles = new ushort[TileCount];
        public const uint TileCount = 64, TilesPerRow = 8,
            BlockSize = (TileCount * Tile.TileSize) / TilesPerRow;

        // Methods
        // TODO
    }
}