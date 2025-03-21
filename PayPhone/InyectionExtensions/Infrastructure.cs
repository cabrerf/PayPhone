using Repository;
using Repository.Interfaces;

namespace TodoList.InyectionExtensions
{
    public static class Infrastructure
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();
        }
    }
}
