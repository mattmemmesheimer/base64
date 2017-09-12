using System;
using B64.Lib;

namespace B64.Console
{
    class Program
    {
        private static readonly int NumRandomTests = 1000;
        private static readonly int MinNumRandomBytes = 1;
        private static readonly int MaxNumRandomBytes = 2048;

        static void Main(string[] args)
        {
            System.Console.WriteLine($"Running {NumRandomTests} random byte encoding tests...");
            var encodeTestPassed = TestEncodeRandomBytes(NumRandomTests);
            System.Console.WriteLine(encodeTestPassed
                ? "All encoding tests passed."
                : "One or more encoding tests failed.");

            System.Console.WriteLine("");

            System.Console.WriteLine($"Running {NumRandomTests} random byte decoding tests...");
            var decodedTestPassed = TestDecodeRandomBytes(NumRandomTests);
            System.Console.WriteLine(encodeTestPassed
                ? "All decoding tests passed."
                : "One or more decoding tests failed.");

            System.Console.ReadLine();
        }

        static bool TestEncodeRandomBytes(int numTests)
        {
            var rand = new Random();
            for (int i = 0; i < numTests; i++)
            {
                var size = rand.Next(MinNumRandomBytes, MaxNumRandomBytes);
                var bytes = new byte[size];
                rand.NextBytes(bytes);
                var expected = Convert.ToBase64String(bytes);
                var actual = Base64Util.Encode(bytes);
                if (expected != actual)
                {
                    return false;
                }
            }
            return true;
        }

        static bool TestDecodeRandomBytes(int numTests)
        {
            var rand = new Random();
            for (int i = 0; i < numTests; i++)
            {
                var size = rand.Next(MinNumRandomBytes, MaxNumRandomBytes);
                var bytes = new byte[size];
                rand.NextBytes(bytes);
                var encoded = Convert.ToBase64String(bytes);
                var actual = Base64Util.Decode(encoded);
                if (!ArraysEqual(actual, bytes))
                {
                    return false;
                }
            }
            return true;
        }

        static bool ArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Input arrays must be of equal length.");
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
