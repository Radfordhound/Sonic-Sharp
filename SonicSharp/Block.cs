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
    }
}