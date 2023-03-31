namespace APIMe.Entities.Models
{
    public partial class Inventory
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int ReorderLevel { get; set; }
    }

}
