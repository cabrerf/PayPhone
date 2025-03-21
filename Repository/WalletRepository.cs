using DbAccess;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;

        public WalletRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> Create(string documentId, string name, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (balance <= 0)
                throw new ArgumentException("Balance must be greater than zero.");

            var newWallet = new Wallet
            {
                DocumentId = documentId,
                Name = name,
                Balance = balance,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Wallets.Add(newWallet);
            await _context.SaveChangesAsync();

            return newWallet;
        }

        public async Task<bool> Delete(int id)
        {
            var walletToDelete = await _context.Wallets.FindAsync(id);

            if (walletToDelete == null)
                return false;

            _context.Wallets.Remove(walletToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Wallet>> Get()
        {
            return await _context.Wallets.ToListAsync();
        }

        public async Task<Wallet?> GetId(int id)
        {
            return await _context.Wallets.FindAsync(id);
        }

        public async Task<Wallet?> Put(int id, string documentId, string name, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (balance <= 0)
                throw new ArgumentException("Balance must be greater than zero.");

            var walletToUpdate = await _context.Wallets.FindAsync(id);

            if (walletToUpdate == null)
                return null;

            walletToUpdate.DocumentId = documentId;
            walletToUpdate.Name = name;
            walletToUpdate.Balance = balance;
            walletToUpdate.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return walletToUpdate;
        }
    }
}
