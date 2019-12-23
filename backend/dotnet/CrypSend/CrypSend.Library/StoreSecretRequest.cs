using System;
using System.Collections.Generic;
using System.Text;

namespace CrypSend.Library
{
    public class StoreSecretRequest
    {
        public string PlainText { get; set; }

        public IEnumerable<RetrievalCondition> RetrievalConditions { get; set; }

        public EncryptionType EncryptionType { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }
    }
}
