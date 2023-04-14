namespace APIMe.Entities.Models
{
    public partial class Orders
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }

}
