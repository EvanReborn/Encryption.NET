using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vigilante
{
    public static class Encryption
    {
        // Dynamic key used for encrypting session data.
        internal static string Key { get; set; }

        // Dynamic iv used for encrypting session data.
        internal static string IV { get; set; }

        // Private random generated with a custom seed (You can change it)
        internal static Random random = new Random();

        // Keeps track of sessions.
        internal static bool SessionStarted;

        /// <summary>
        /// Generates a random string using LINQ.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RandomString(int length)
        {
            return new string(Enumerable.Repeat("0123456789abcdefABCDEF", length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        public static void StartSession()
        {
            if (SessionStarted)
            {
                MessageBox.Show($"Warning: A session is already in progress and cannot be restarted!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionStarted  = true;
            Key             = Convert.ToBase64String(Encoding.Default.GetBytes(RandomString(32)));
            IV              = Convert.ToBase64String(Encoding.Default.GetBytes(RandomString(16)));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void EndSession()
        {
            if (!SessionStarted)
            {
                MessageBox.Show($"Warning: No session is in progress and cannot be ended!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SessionStarted  = true;
            Key             = null;
            IV              = null;
        }

        /// <summary>
        /// Front-end encryption method.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            if (!SessionStarted)
            {
                MessageBox.Show($"Error: You must start an encryption session to encrypt/decrypt!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return EncryptString(value, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(Key)))), Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(IV))));
        }

        /// <summary>
        /// Front-end decryption method.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            if (!SessionStarted)
            {
                MessageBox.Show($"Error: You must start an encryption session to encrypt/decrypt!", "Vigilante", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return DecryptString(value, SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(Key)))), Encoding.ASCII.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(IV))));
        }

        /// <summary>
        /// Core encryption algorithm.
        /// </summary>
        /// <param name="plainText">String to encrypt.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Encryption iv.</param>
        /// <returns>The encrypted string.</returns>
        private static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using (var encryptor = Aes.Create())
            {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var aesEncryptor = encryptor.CreateEncryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write))
                        {

                            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] cipherBytes = memoryStream.ToArray();

                            return Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Core decryption algorithm.
        /// </summary>
        /// <param name="cipherText">Encrypted text.</param>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Encryption iv.</param>
        /// <returns>The plain text string.</returns>
        private static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            using (var encryptor = Aes.Create())
            {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    using (var aesDecryptor = encryptor.CreateDecryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write))
                        {

                            byte[] cipherBytes = Convert.FromBase64String(cipherText);

                            cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] plainBytes = memoryStream.ToArray();

                            return Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                        }
                    }
                }
            }
        }
    }
}
