using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class RouteType
    {
        public RouteType()
        {
            Routes = new HashSet<Route>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ResponseCode { get; set; } = null!;
        public int CrudId { get; set; }

        public virtual ICollection<Route> Routes { get; set; }
    }
}
