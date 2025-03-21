using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("YourFallbackConnectionString");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Wallet>().ToTable("Wallets");
            modelBuilder.Entity<TransactionHistory>().ToTable("TransactionHistories");
        }
    }
}
