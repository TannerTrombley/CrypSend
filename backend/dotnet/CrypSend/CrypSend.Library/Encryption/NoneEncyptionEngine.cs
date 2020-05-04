using CrypSend.Library.SecretMetadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public class NoneEncyptionEngine : IEncryptionEngine
    {

        public Task<string> DecryptAsync(SecretPayload secret, SecretMetadataDocument secretMetadata)
        {
            return Task.FromResult(Encoding.UTF8.GetString(secret.EncryptedPayload));
        }

        public Task<byte[]> EncryptAsync(string plaintext, SecretMetadataDocument secretMetadata)
        {
            return Task.FromResult(Encoding.UTF8.GetBytes(plaintext));
        }
    }
}
