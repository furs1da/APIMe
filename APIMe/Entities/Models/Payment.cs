namespace APIMe.Entities.Models
{
    public partial class Payments
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;
    }
}
