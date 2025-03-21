using Entities;
using Repository.Interfaces;

namespace Repository
{
    public class WalletRepository : IWalletRepository
    {
        private static List<Wallet> wallets = new List<Wallet>
        {
            new Wallet { Id = 1, DocumentId = "DOC123", Name = "Personal Wallet", Balance = 1000.00m, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Wallet { Id = 2, DocumentId = "DOC456", Name = "Business Wallet", Balance = 5000.00m, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Wallet { Id = 3, DocumentId = "DOC789", Name = "Savings Wallet", Balance = 3000.00m, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        };

        public Task<Wallet> Create(string documentId, string name, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (balance <= 0)
                throw new ArgumentException("Balance must be greater than zero.");

            return Task<Wallet>.Run(() =>
            {
                var newWallet = new Wallet()
                {
                    Id = wallets.Max(wallet => wallet.Id) + 1,
                    DocumentId = documentId,
                    Name = name,
                    Balance = balance,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                wallets.Add(newWallet);

                return newWallet;
            });
        }

        public Task<bool> Delete(int id)
        {
            return Task<bool>.Run(() =>
            {
                Wallet? walletToDelete = wallets.FirstOrDefault(wallet => wallet.Id == id);

                if (walletToDelete is null)
                    return false;

                return wallets.Remove(walletToDelete);
            });
        }

        public Task<IEnumerable<Wallet>> Get()
        {
            return Task.FromResult(wallets.AsEnumerable());
        }

        public Task<Wallet?> GetId(int id)
        {
            Wallet? wallet = wallets.AsParallel().FirstOrDefault(wallet => wallet.Id == id);

            return Task.FromResult(wallet);
        }

        public Task<Wallet?> Put(int id, string documentId, string name, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (balance <= 0)
                throw new ArgumentException("Balance must be greater than zero.");

            return Task<Wallet>.Run(() =>
            {
                var walletToUpdate = wallets.FirstOrDefault(wallet => wallet.Id == id);

                if (walletToUpdate is null)
                    return null;

                walletToUpdate.DocumentId = documentId;
                walletToUpdate.Name = name;
                walletToUpdate.Balance = balance;
                walletToUpdate.UpdatedAt = DateTime.Now;

                return walletToUpdate;
            });
        }


    }
}
