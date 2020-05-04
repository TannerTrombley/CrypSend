using CrypSend.Library.Encryption;
using System;

namespace CrypSend.Library
{
    public class EncryptionEngineFactory : IEncryptionEngineFactory
    {
        public IEncryptionEngine GetEncryptionEngine(EncryptionType type)
        {
            switch (type)
            {
                case EncryptionType.None:
                    return new NoneEncyptionEngine();
                case EncryptionType.OneTimePad:
                    return new OneTimePadEncryptionEngine();
                default:
                    throw new ArgumentException($"No Encryption Engine to handle {type.ToString()}");
            }
        }
    }
}
