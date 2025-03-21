using Entities;

namespace Repository.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> Create(string documentId, string name, decimal balance);
        Task<bool> Delete(int id);
        Task<IEnumerable<Wallet>> Get();
        Task<Wallet?> GetId(int id);
        Task<Wallet?> Put(int id, string documentId, string name, decimal balance);
    }
}
