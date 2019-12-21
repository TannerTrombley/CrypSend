using CrypSend.Repository;
using System;
using System.Collections.Generic;

namespace CrypSend.Library
{
    public class SecretPayload : DocumentBase
    {
        public EncryptionType EncryptionType { get; set; }

        public Guid KeyId { get; set; }

        [EntityJsonPropertyConverter]
        public byte[] EncryptedPayload { get; set; }

        [EntityJsonPropertyConverter]
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
