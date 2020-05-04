using CrypSend.Library.SecretMetadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library.Encryption
{
    class OneTimePadEncryptionEngine : IEncryptionEngine
    {
        public Task<string> DecryptAsync(SecretPayload secret, SecretMetadataDocument secretMetadata)
        {
            var plaintextBytes = new byte[secret.EncryptedPayload.Length];
            for (int i = 0; i < secret.EncryptedPayload.Length; i++)
            {
                plaintextBytes[i] = (byte)(secret.EncryptedPayload[i] ^ secretMetadata.EncryptionKey[i]);
            }

            return Task.FromResult(Encoding.UTF8.GetString(plaintextBytes));
        }

        public Task<byte[]> EncryptAsync(string plaintext, SecretMetadataDocument secretMetadata)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            secretMetadata.EncryptionKey = GenerateKey(plaintext.Length);
            var encryptedBytes = new byte[plaintext.Length];
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                encryptedBytes[i] = (byte)(plaintextBytes[i] ^ secretMetadata.EncryptionKey[i]);
            }
            return Task.FromResult(encryptedBytes);
        }

        private byte[] GenerateKey(int length)
        {
            var rand = new Random();
            var key = new byte[length];
            rand.NextBytes(key);
            return key;
        }
    }
}
