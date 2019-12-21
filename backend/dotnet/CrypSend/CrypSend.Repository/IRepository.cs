using System.Threading.Tasks;

namespace CrypSend.Repository
{
    public interface IRepository<T>
    {
        Task<T> CreateDocumentAsync(T document);

        Task<T> GetDocumentAsync(string id, string partitionKey);

        Task<T> DeleteDocumentAsync(string id, string partitionKey);

        Task<T> UpsertDocumentAsync(T document);
    }
}
