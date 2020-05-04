using CrypSend.Library.SecretMetadata;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public interface IEncryptionEngine
    {
        Task<byte[]> EncryptAsync(string plaintext, SecretMetadataDocument secretMetadata);

        Task<string> DecryptAsync(SecretPayload secret, SecretMetadataDocument secretMetadata);
    }
}
