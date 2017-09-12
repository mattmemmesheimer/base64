using System.Collections.Generic;

namespace B64.Lib
{
    public static class ArrayExtensions
    {
        public static T[] Pad<T>(this T[] array, int blockSize, T paddingValue)
        {
            if (array.Length % blockSize == 0)
            {
                return array;
            }
            var paddingLength = blockSize - (array.Length % 3);
            var padded = new List<T>(array);
            for (int i = 0; i < paddingLength; i++)
            {
                padded.Add(paddingValue);
            }
            return padded.ToArray();
        }
    }
}
