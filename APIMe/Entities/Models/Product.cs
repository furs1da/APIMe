using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
    }
}
