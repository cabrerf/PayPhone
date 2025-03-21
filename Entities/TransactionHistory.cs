namespace Entities
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // This also can be an enum
        public DateTime CreatedAt { get; set; }
    }
}
