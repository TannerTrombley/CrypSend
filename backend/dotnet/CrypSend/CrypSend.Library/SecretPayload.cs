using CrypSend.Repository;
using System;
using System.Collections.Generic;

namespace CrypSend.Library
{
    public class SecretPayload : DocumentBase
    {
        [EntityJsonPropertyConverter]
        public EncryptionType EncryptionType { get; set; }

        public Guid KeyId { get; set; }

        public byte[] EncryptedPayload { get; set; }

        [EntityJsonPropertyConverter]
        public IEnumerable<Contact> Contacts { get; set; }

        [EntityJsonPropertyConverter]
        public IEnumerable<RetrievalCondition> RetrievalConditions { get; set; }
    }
}
