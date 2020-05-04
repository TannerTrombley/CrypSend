using CrypSend.Repository;
using System;
using System.Collections.Generic;

namespace CrypSend.Library
{
    public class SecretPayload : DocumentBase
    {
        [EntityJsonPropertyConverter]
        public EncryptionType EncryptionType { get; set; }

        public Guid MetadataId { get; set; }

        public byte[] EncryptedPayload { get; set; }

        public bool IsLocked { get; set; } = true;

        public int Attempts { get; set; }
    }
}
