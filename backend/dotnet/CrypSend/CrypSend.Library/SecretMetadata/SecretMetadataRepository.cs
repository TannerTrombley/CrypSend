using CrypSend.Library.Settings;
using CrypSend.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.SecretMetadata
{
    public class SecretMetadataRepository : RepositoryBase<SecretMetadataDocument>
    {
        public SecretMetadataRepository(ISettingsProvider settings) :
            base(settings.GetRequiredSetting("SecretMetadataConnection"), settings.GetRequiredSetting("SecretMetadataTableName"))
        { }
    }
}
