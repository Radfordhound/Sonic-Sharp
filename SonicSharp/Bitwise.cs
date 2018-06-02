namespace SonicSharp
{
    public static class Bitwise
    {
        // Variables/Constants
        private const uint ByteSize = 8;

        // Methods
        /// <summary>
        /// Returns the number of bytes required to store the given amount of bits
        /// (E.G. 3 bytes are required to store 20 bits).
        /// </summary>
        /// <param name="bits">The amount of bits you wish to save somewhere.</param>
        /// <returns>The number of bytes required to store the given amount of bits
        /// (E.G. 3 bytes are required to store 20 bits).</returns>
        public static uint GetRequiredBytes(uint bits)
        {
            if (bits % ByteSize == 0) return bits / ByteSize;
            return ((ByteSize - (bits % ByteSize)) + bits) / ByteSize;
        }
    }
}