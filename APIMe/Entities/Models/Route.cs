using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class Route
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int RouteTypeId { get; set; }
        public string DataTableName { get; set; } = null!;
        public bool IsVisible { get; set; }
        public virtual RouteType RouteType { get; set; } = null!;
    }
}
