using System;
using System.Security.Cryptography;
using System.Text;

namespace crmweb.Common.Extensions
{
    public static class SecurityExtensions
    {
        public static string GenerateRandomPassword()
        {
            var vProvider = new RNGCryptoServiceProvider();
            var vByteArray = new byte[4];

            vProvider.GetBytes(vByteArray);

            //convert 4 bytes to an integer
            return BitConverter.ToUInt32(vByteArray, 0).ToString().Substring(0,6);
        }

        public static string EncodeBase64(this byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (input.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(input));

            string vOutput = Convert.ToBase64String(input);
            vOutput = vOutput.Split('=')[0]; // Remove any trailing '='s
            vOutput = vOutput.Replace('+', '-'); // 62nd char of encoding
            vOutput = vOutput.Replace('/', '_'); // 63rd char of encoding
            return vOutput;
        }

        public static byte[] DecodeBase64(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException(nameof(input));

            string vOutput = input;
            vOutput = vOutput.Replace('-', '+'); // 62nd char of encoding
            vOutput = vOutput.Replace('_', '/'); // 63rd char of encoding
            switch (vOutput.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    vOutput += "==";
                    break; // Two pad chars
                case 3:
                    vOutput += "=";
                    break; // One pad char
                default:
                    throw new FormatException("Illegal base64url string!");
            }

            byte[] vConverted = Convert.FromBase64String(vOutput); // Standard base64 decoder
            return vConverted;
        }

        public static string GetHash(this string input)
        {
            HashAlgorithm vHashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] vByteValue = Encoding.UTF8.GetBytes(input);
            byte[] vByteHash = vHashAlgorithm.ComputeHash(vByteValue);

            return Convert.ToBase64String(vByteHash);
        }
        
        public static string GetMd5Hash(this string input)
        {
            using (MD5 vHash = MD5.Create())
            {

                byte[] vData = vHash.ComputeHash(Encoding.UTF8.GetBytes(input));

                var vBuilder = new StringBuilder();

                foreach (byte vByte in vData)
                    vBuilder.Append(vByte.ToString("X2"));

                return vBuilder.ToString();
            }
        }

        public static bool VerifyMd5Hash(this string input, string hash)
        {
            string vHashOfInput = GetMd5Hash(input);

            StringComparer vComparer = StringComparer.OrdinalIgnoreCase;

            return vComparer.Compare(vHashOfInput, hash) == 0;
        }
    }
}
