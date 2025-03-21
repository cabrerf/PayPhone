using DbAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWalletRepository _walletRepository;

        public TransactionHistoryRepository(ApplicationDbContext context, IWalletRepository walletRepository)
        {
            _context = context;
            _walletRepository = walletRepository;
        }

        public async Task<TransactionHistory> Create(int walletId, decimal amount, string type)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");

            var wallet = await _walletRepository.GetId(walletId);

            if (wallet is null)
                throw new ArgumentException("Wallet does not exist.");

            if (wallet.Balance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            var newTransaction = new TransactionHistory
            {
                WalletId = walletId,
                Amount = amount,
                Type = type,
                CreatedAt = DateTime.Now
            };

            wallet.Balance -= amount;
            await _walletRepository.Put(wallet.Id, wallet.DocumentId, wallet.Name, wallet.Balance);

            _context.TransactionHistory.Add(newTransaction);
            await _context.SaveChangesAsync();

            return newTransaction;
        }

        public async Task<bool> Delete(int id)
        {
            var transactionToDelete = await _context.TransactionHistory.FindAsync(id);

            if (transactionToDelete == null)
                return false;

            _context.TransactionHistory.Remove(transactionToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TransactionHistory>> Get()
        {
            return await _context.TransactionHistory.ToListAsync();
        }

        public async Task<TransactionHistory?> GetId(int id)
        {
            return await _context.TransactionHistory.FindAsync(id);
        }

        public async Task<IEnumerable<TransactionHistory>> GetByWalletId(int walletId)
        {
            return await _context.TransactionHistory.Where(th => th.WalletId == walletId).ToListAsync();
        }

        public async Task<TransactionHistory?> Update(int id, decimal amount, string type)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Type cannot be empty.");

            var transactionToUpdate = await _context.TransactionHistory.FindAsync(id);

            if (transactionToUpdate == null)
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

            await _context.SaveChangesAsync();

            return transactionToUpdate;
        }
    }
}