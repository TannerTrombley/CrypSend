namespace CrypSend.Library
{
    public interface IEncryptionEngineFactory
    {
        IEncryptionEngine GetEncryptionEngine(EncryptionType type);
    }
}
