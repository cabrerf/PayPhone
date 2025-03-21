using Entities;

namespace Repository.Interfaces
{
    public interface ITransactionHistoryRepository
    {
        Task<TransactionHistory> Create(int walletId, decimal amount, string type);
        Task<bool> Delete(int id);
        Task<IEnumerable<TransactionHistory>> Get();
        Task<TransactionHistory?> GetId(int id);
        Task<IEnumerable<TransactionHistory>> GetByWalletId(int walletId);
        Task<TransactionHistory?> Update(int id, decimal amount, string type);
    }
}
