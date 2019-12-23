using CrypSend.Library.Settings;
using CrypSend.Repository;
using Microsoft.Extensions.Options;

namespace CrypSend.Library
{
    public class SecretPayloadRepository : RepositoryBase<SecretPayload>
    {
        public SecretPayloadRepository(ISettingsProvider settings) :
            base(settings.GetRequiredSetting("SecretPayloadConnection"), settings.GetRequiredSetting("SecretPayloadTableName"))
        { }
    }
}
