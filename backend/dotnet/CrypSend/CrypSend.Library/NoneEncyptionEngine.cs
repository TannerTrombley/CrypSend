using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public class NoneEncyptionEngine : IEncryptionEngine
    {
        public string Decrypt(byte[] encryptedPayload, Dictionary<string, object> properties)
        {
            return Encoding.UTF8.GetString(encryptedPayload);
        }

        public byte[] Encrypt(string plaintext)
        {
            return Encoding.UTF8.GetBytes(plaintext);
        }
    }
}
