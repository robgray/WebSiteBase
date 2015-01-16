using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Common
{
    public class Cryptography
    {
        private static readonly RandomNumberGenerator rng = new RNGCryptoServiceProvider();

        public static byte[] HexStringToByteArray(string input)
        {
            byte[] result = new byte[input.Length / 2];

            for (int idx = 0; idx < input.Length; idx += 2)
                result[idx / 2] = byte.Parse(input.Substring(idx, 2), System.Globalization.NumberStyles.HexNumber);

            return result;
        }

        public static string ByteArrayToHexString(byte[] input)
        {
            string result = "";
            foreach (byte b in input)
            {
                string num = b.ToString("X");
                while (num.Length < 2)
                    num = "0" + num;
                result += num;
            }
            return result;
        }

        public static byte[] GetHMACMD5(string strKey, string strText)
        {
            byte[] bKey = Encoding.ASCII.GetBytes(strKey);
            return GetHMACMD5(bKey, strText);
        }

        public static byte[] GetHMACMD5(byte[] bKey, string strText)
        {
            MD5CryptoServiceProvider cspMD5 = new MD5CryptoServiceProvider();

            byte[] bText = Encoding.ASCII.GetBytes(strText);
            byte[] ipad = new byte[64];
            byte[] opad = new byte[64];
            byte[] idata = new byte[64 + bText.Length];
            byte[] odata = new byte[64 + 16];

            if (bKey.Length > 64)
                bKey = cspMD5.ComputeHash(bKey);

            //byte[] bPass1 = cspMD5.ComputeHash(bKey);

            for (int i = 0; i < 64; i++)
            {
                idata[i] = ipad[i] = 0x36;
                odata[i] = opad[i] = 0x5C;
            }

            for (int i = 0; i < bKey.Length; i++)
            {
                ipad[i] ^= bKey[i];
                opad[i] ^= bKey[i];

                idata[i] = (ipad[i] &= 0xFF);
                odata[i] = (opad[i] &= 0xFF);
            }

            for (int i = 0; i < bText.Length; i++)
                idata[64 + i] = (bText[i] &= 0xFF);

            byte[] innerhashout = cspMD5.ComputeHash(idata);

            for (int i = 0; i < 16; i++)
                odata[64 + i] = innerhashout[i];

            return cspMD5.ComputeHash(odata);
        }

        public enum HashingAlgorithm
        {
            MD5,
            SHA1,
            SHA256,
            SHA384,
            SHA512
        }

        public static string GetHash(string plainText, HashingAlgorithm hashAlgorithm)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm hash;
            switch (hashAlgorithm)
            {
                case HashingAlgorithm.SHA1:
                    hash = new SHA1Managed();
                    break;

                case HashingAlgorithm.SHA256:
                    hash = new SHA256Managed();
                    break;

                case HashingAlgorithm.SHA384:
                    hash = new SHA384Managed();
                    break;

                case HashingAlgorithm.SHA512:
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public static byte[] GetRandomBytes(int count)
        {
            byte[] result = new byte[count];
            rng.GetBytes(result);
            return result;
        }

        public static Int32 GetRandomInt32()
        {
            return BitConverter.ToInt32(GetRandomBytes(sizeof(Int32)), 0);
        }

        public static string GetRandomCapitals(int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException("Length must be a positive (non-zero) integer.");
            }

            // Input (randomness) storage
            byte[] randomBytes = null;
            int randomIndex = 0;

            // Result storage
            char[] result = new char[length];
            int resultIndex = 0;

            while (resultIndex < length)
            {
                // Fill randomBytes when empty or read past end
                if (null == randomBytes || randomIndex >= randomBytes.Length)
                {
                    randomBytes = Cryptography.GetRandomBytes((int)Math.Round((length - resultIndex) / 26.0 * 32.0)); // Estimate required length
                    randomIndex = 0;
                }

                // Zero bits 1,3; set bit 7 - ensures byte is not far away from ASCII capital letter range without biasing randomness of result.
                randomBytes[randomIndex] &= 0x5f;
                randomBytes[randomIndex] |= 0x40;

                // Record only capital A-Z
                if (randomBytes[randomIndex] >= 0x41 && randomBytes[randomIndex] <= 0x5a)
                {
                    // Copy character to result
                    resultIndex += Encoding.ASCII.GetChars(randomBytes, randomIndex, 1, result, resultIndex);
                }
                randomIndex++;
            }
            return new string(result);
        }


        public static string GetRandomAlphaNumeric(int length)
        {
            var result = new StringBuilder(length);

            if (length < 1)
            {
                throw new ArgumentOutOfRangeException("Length must be a positive (non-zero) integer.");
            }

            // Input (randomness) storage
            var data = new byte[1];

            // CharPool for AlphaNumeric Values
            var charPool = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            // Result storage
            var crypto = new RNGCryptoServiceProvider();
            data = new byte[length];
            crypto.GetNonZeroBytes(data);

            data.ToList().ForEach(b => result.Append(charPool[b % (charPool.Length - 1)]));

            return result.ToString();

        }
    }
}
