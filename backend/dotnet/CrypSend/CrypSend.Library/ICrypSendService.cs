using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public interface ICrypSendService
    {
        Task StoreSecretAsync(string plaintext);

        Task<FetchStoredSecretResponse> FetchStoredSecretAsync(string id);
    }
}
