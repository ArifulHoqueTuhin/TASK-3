
using System;

namespace Task_3
{
    public class FairNumberProtocol
    {
        public int ComputerNumber { get; private set; }
        public byte[] Key { get; private set; } = null!;
        public string? Hmac { get; private set; }

        public void Init(int range)
        {
            Key = RandomGenerator.GenerateKey();
            ComputerNumber = RandomGenerator.SecureRandomInt(range);
            Hmac = HMACGenerator.GenerateHMAC(Key, ComputerNumber.ToString());


        }

        public int Finalize(int userNumber, int range)
        {
            Console.WriteLine($"Secret Key: {Convert.ToBase64String(Key)}");
            Console.WriteLine($"Computer Number: {ComputerNumber}");
            return (userNumber + ComputerNumber) % range;
        }
    }
}
