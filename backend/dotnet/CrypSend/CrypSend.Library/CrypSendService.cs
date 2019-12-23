using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public class CrypSendService : ICrypSendService
    {
        private readonly IEncryptionEngineFactory _encryptionEngineFactory;

        public CrypSendService(IEncryptionEngineFactory encryptionEngineFactory)
        {
            _encryptionEngineFactory = encryptionEngineFactory;
        }

        public Task<FetchStoredSecretResponse> FetchStoredSecretAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task StoreSecretAsync(string plaintext)
        {
            throw new NotImplementedException();
        }
    }
}
