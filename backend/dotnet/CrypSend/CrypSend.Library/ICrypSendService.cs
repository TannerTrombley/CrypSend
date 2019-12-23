using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypSend.Library
{
    public interface ICrypSendService
    {
        Task<StoreSecretResponse> StoreSecretAsync(StoreSecretRequest request);

        Task<FetchStoredSecretResponse> FetchStoredSecretAsync(string id);
    }
}
