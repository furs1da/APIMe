using System;
using System.Collections.Generic;

namespace APIMe.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string Quantity { get; set; } = null!;
    }
}
