using CrypSend.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library.SecretMetadata
{
    public class SecretMetadataDocument : DocumentBase
    {
        public string OneTimeCode { get; set; }

        public byte[] EncryptionKey { get; set; }
    }
}
