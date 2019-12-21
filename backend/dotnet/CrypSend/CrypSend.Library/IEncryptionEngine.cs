using System.Collections.Generic;

namespace CrypSend.Library
{
    public interface IEncryptionEngine
    {
        byte[] Encrypt(string plaintext);

        string Decrypt(byte[] encryptedPayload, Dictionary<string, object> properties);
    }
}
