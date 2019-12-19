using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _18._12._19_Homework_SQL_lite_ClassLibrary
{
    static class Statics
    {
        static public Guid GetDeterministicGuid(string input)

        {

            //use MD5 hash to get a 16-byte hash of the string:

            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();

            byte[] inputBytes = Encoding.Default.GetBytes(input);

            byte[] hashBytes = provider.ComputeHash(inputBytes);

            //generate a guid from the hash:

            Guid hashGuid = new Guid(hashBytes);

            return hashGuid;

        }



        static public int UniqeNumber(int number)
        {
            var bytes = new byte[number];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            int random = BitConverter.ToInt32(bytes, 0) % 100000000;
            return random;
        }



        /*

        // Create byte array for additional entropy when using Protect method.
        static byte[] s_aditionalEntropy = { 9, 8, 7, 6, 5 };

        public static byte[] Protect(byte[] data)
        {
            try
            {
                return ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException e)
            {
                throw new Exception($"Data was not encrypted.An error occurred.\n\n{e.Message}");
            }

        }

        public static byte[] Unprotect(byte[] data)
        {
            try
            {
                return ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException e)
            {
                throw new Exception($"Data was not encrypted.An error occurred.\n\n{e.Message}");
            }
        }

        public static byte[] Int32ToByteArr(int num)
        {
            return BitConverter.GetBytes(num);
        }
        public static int ByteArrToInt32(byte[] b)
        {
            return BitConverter.ToInt32(b, 0);

        }


        */
    }
}
