using System;
using System.Text;

namespace Infrastructure.Common
{
    public class Text
    {
        private static Random _random = new Random(Environment.TickCount);

        public static string GetRandomString(int length)
        {
            return GetRandomString("abcdefghjkmnpqrtuvwxyzABCDEFGHJKMNPQRTUVWXYZ2346789", length);
        }

        /// <summary>
        /// Return a random string of the given length with the specified character set
        /// </summary>
        public static string GetRandomString(string characterSet, int length)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
                sBuilder.Append(characterSet[_random.Next(0, characterSet.Length)]);

            return sBuilder.ToString();
        }
    }
}
