namespace APIMe.Entities.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public int ReorderLevel { get; set; }
    }

}
