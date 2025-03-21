using Entities;
using Repository.Interfaces;

namespace Repository
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly IWalletRepository _walletRepository;
        private static List<TransactionHistory> transactionHistories = new List<TransactionHistory>();

        public TransactionHistoryRepository(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public Task<TransactionHistory> Create(int walletId, decimal amount, string type)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");

            return Task<TransactionHistory>.Run(async () =>
            {
                var wallet = await _walletRepository.GetId(walletId);

                if (wallet is null)
                    throw new ArgumentException("Wallet does not exist.");

                if (wallet.Balance < amount)
                    throw new InvalidOperationException("Insufficient balance.");

                var newTransaction = new TransactionHistory()
                {
                    Id = transactionHistories.Any() ? transactionHistories.Max(th => th.Id) + 1 : 1,
                    WalletId = walletId,
                    Amount = amount,
                    Type = type,
                    CreatedAt = DateTime.Now
                };

                wallet.Balance -= amount;
                await _walletRepository.Put(wallet.Id, wallet.DocumentId, wallet.Name, wallet.Balance);
                transactionHistories.Add(newTransaction);

                return newTransaction;
            });
        }

        public Task<bool> Delete(int id)
        {
            return Task<bool>.Run(() =>
            {
                var transactionToDelete = transactionHistories.FirstOrDefault(th => th.Id == id);

                if (transactionToDelete is null)
                    return false;

                return transactionHistories.Remove(transactionToDelete);
            });
        }

        public Task<IEnumerable<TransactionHistory>> Get()
        {
            return Task.FromResult(transactionHistories.AsEnumerable());
        }

        public Task<TransactionHistory?> GetId(int id)
        {
            var transaction = transactionHistories.FirstOrDefault(th => th.Id == id);

            return Task.FromResult(transaction);
        }

        public Task<IEnumerable<TransactionHistory>> GetByWalletId(int walletId)
        {
            var transactions = transactionHistories.Where(th => th.WalletId == walletId);

            return Task.FromResult(transactions.AsEnumerable());
        }

        public Task<TransactionHistory?> Update(int id, decimal amount, string type)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");

            return Task<TransactionHistory>.Run(async () =>
            {
                var transactionToUpdate = transactionHistories.FirstOrDefault(th => th.Id == id);

                if (transactionToUpdate is null)
                    return null;

                var wallet = await _walletRepository.GetId(transactionToUpdate.WalletId);

                if (wallet is null)
                    throw new ArgumentException("Wallet does not exist.");

                if (wallet.Balance < amount)
                    throw new InvalidOperationException("Insufficient balance.");

                transactionToUpdate.Amount = amount;
                transactionToUpdate.Type = type;
                transactionToUpdate.CreatedAt = DateTime.Now;

                wallet.Balance -= amount;
                await _walletRepository.Put(wallet.Id, wallet.DocumentId, wallet.Name, wallet.Balance);

                return transactionToUpdate;
            });
        }
    }
}
