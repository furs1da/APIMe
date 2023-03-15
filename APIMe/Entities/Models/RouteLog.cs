using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class RouteLog
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int StudentId { get; set; }
        public byte[] Timestamp { get; set; } = null!;
        public int ResponseStatus { get; set; }

        public virtual Route Route { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
