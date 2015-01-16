using Domain.Interfaces;
using Infrastructure.Common;

namespace Domain.Services
{
    public class CryptographyProvider : ICryptographyProvider
    {
        public string GetRandomString(int length)
        {
            return Text.GetRandomString(length);
        }

        public string GetHash(string salt, string plainText)
        {
            return Cryptography.GetHash(plainText + salt, Cryptography.HashingAlgorithm.SHA1);
        }

        public byte[] GetRandomBytes(int size)
        {
            return Cryptography.GetRandomBytes(size);
        }

    }
}
