
using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;

namespace Task_3
{
    public class RandomGenerator
    {
        public static byte[] GenerateKey(int bytes = 32)
        {
            var key = new byte[bytes];
            RandomNumberGenerator.Fill(key);
            return key;
        }

        public static int SecureRandomInt(int max)
        {
            var buffer = new byte[4];
            int result;

            do
            {
                RandomNumberGenerator.Fill(buffer);
                result = BitConverter.ToInt32(buffer, 0) & int.MaxValue;
            } while (result >= max * (int.MaxValue / max));

            return result % max;
        }
    }

    public class HMACGenerator
    {
        public static string GenerateHMAC(byte[] key, string message)
        {
            HMac hmac = new HMac(new Sha3Digest(256));
            hmac.Init(new KeyParameter(key));

            byte[] input = Encoding.UTF8.GetBytes(message);
            hmac.BlockUpdate(input, 0, input.Length);

            byte[] result = new byte[hmac.GetMacSize()];
            hmac.DoFinal(result, 0);

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
    }
}
