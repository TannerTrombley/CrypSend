using System;

namespace CrypSend.Library
{
    public class EncyptionEngineFactory : IEncryptionEngineFactory
    {
        public IEncryptionEngine GetEncryptionEngine(EncryptionType type)
        {
            switch (type)
            {
                case EncryptionType.None:
                    return new NoneEncyptionEngine();
                default:
                    throw new ArgumentException($"No Encryption Engine to handle {type.ToString()}");
            }
        }
    }
}
