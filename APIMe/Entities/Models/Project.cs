using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class Project
    {
        public Project()
        {
            BugFeatures = new HashSet<BugFeature>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public bool IsCompleted { get; set; }

        public virtual AspNetUser User { get; set; } = null!;
        public virtual ICollection<BugFeature> BugFeatures { get; set; }
    }
}
