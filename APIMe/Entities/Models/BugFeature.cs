using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class BugFeature
    {
        public int Id { get; set; }
        public string Summary { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsBug { get; set; }
        public string? StepsToReproduce { get; set; }
        public string? ExpectedResults { get; set; }
        public string? ActualResults { get; set; }
        public string? Environment { get; set; }
        public int SeverityId { get; set; }
        public string? Priority { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; } = null!;
    }
}
