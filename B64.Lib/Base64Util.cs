using System;
using System.Collections.Generic;
using System.Text;

namespace B64.Lib
{
    public static class Base64Util
    {
        private static readonly int BlockSize = 3;

        public static byte[] Decode(string input)
        {
            if (input.Length % 4 != 0)
            {
                throw new ArgumentException("Input length must be a multiple of 4.", nameof(input));
            }
            var decoded = new List<byte>();
            // Replace padding with 'A' (which is 0)
            int paddingIndex = input.IndexOf('=');
            int paddingLength = 0;
            if (paddingIndex > -1)
            {
                paddingLength = input.Length - paddingIndex;
            }
            input = input.Replace("=", "A");
            for (int i = 0; i < input.Length; i += 4)
            {
                // Calculate the four 6-bit indexes
                int index1 = Base64Table.Characters.IndexOf(input[i]);
                int index2 = Base64Table.Characters.IndexOf(input[i + 1]);
                int index3 = Base64Table.Characters.IndexOf(input[i + 2]);
                int index4 = Base64Table.Characters.IndexOf(input[i + 3]);
                // Combine the four 6-bit indexes into a 24-bit number
                int combined = (index1 << 18) + (index2 << 12) + (index3 << 6) + index4;
                // Split the 24-bit number into three 8-bit characters (bytes)
                decoded.Add((byte) ((combined >> 16) & 0xFF));
                decoded.Add((byte)((combined >> 8) & 0xFF));
                decoded.Add((byte) (combined & 0xFF));
            }
            decoded.RemoveRange(decoded.Count - paddingLength, paddingLength);
            return decoded.ToArray();
        }

        public static string Encode(byte[] bytes)
        {
            var sb = new StringBuilder();
            int paddingLength = BlockSize - (bytes.Length % BlockSize);
            paddingLength = paddingLength == BlockSize ? 0 : paddingLength;
            bytes = bytes.Pad<byte>(BlockSize, 0x00);
            for (int i = 0; i < bytes.Length; i += BlockSize)
            {
                var b1 = bytes[i];
                var b2 = bytes[i + 1];
                var b3 = bytes[i + 2];

                // Combine the three 8-bit values into one 24-bit number
                int combined = (b1 << 16) + (b2 << 8) + b3;

                // Split the 24-bit number into four 6-bit values
                var index = (combined >> 18) & 0x3F;
                sb.Append(Base64Table.Characters[index]);
                index = (combined >> 12) & 0x3F;
                sb.Append(Base64Table.Characters[index]);
                index = (combined >> 6) & 0x3F;
                sb.Append(Base64Table.Characters[index]);
                index = combined & 0x3F;
                sb.Append(Base64Table.Characters[index]);
            }
            if (paddingLength > 0)
            {
                for (int i = 0; i < paddingLength; i++)
                {
                    sb[sb.Length - i - 1] = '=';
                }
            }
            var encoded = sb.ToString();
            return encoded;
        }
    }
}
