using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public interface IEncryptionEngine
    {
        Task<byte[]> EncryptAsync(string plaintext);

        Task<string> DecryptAsync(SecretPayload secret);
    }
}
