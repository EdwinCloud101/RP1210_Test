using System;
using System.Collections.Generic;

namespace Peak.Classes
{
    public static class SubArray
    {
        public static byte[] GetBytes(byte[] source, int startIndex) => SubArray.GetArray<byte>(source, startIndex, source.Length - startIndex);

        public static byte[] GetBytes(byte[] source, int startIndex, int length) => SubArray.GetArray<byte>(source, startIndex, length);

        public static type[] GetArray<type>(type[] source, int startIndex) => SubArray.GetArray<type>(source, startIndex, source.Length - startIndex);

        public static type[] GetArray<type>(type[] source, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || startIndex + length > source.Length)
                throw new ArgumentOutOfRangeException();
            type[] typeArray = new type[length];
            for (int index = 0; index < length; ++index)
                typeArray[index] = source[startIndex + index];
            return typeArray;
        }

        public static List<type> GetArray<type>(List<type> source, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || startIndex + length > source.Count)
                throw new ArgumentOutOfRangeException();
            List<type> typeArray = new List<type>(length);
            for (int index = 0; index < length; ++index)
                typeArray[index] = source[startIndex + index];
            return typeArray;
        }
    }
}
