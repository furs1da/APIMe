using System;
using System.Collections.Generic;

namespace APIMe.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Sections = new HashSet<Section>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual ICollection<Section> Sections { get; set; }
    }
}
