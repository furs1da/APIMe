using System;
using System.Collections.Generic;

namespace APIMe.Models
{
    public partial class Section
    {
        public int Id { get; set; }
        public string SectionName { get; set; } = null!;
        public int ProfessorId { get; set; }
        public string AccessCode { get; set; } = null!;

        public virtual Professor Professor { get; set; } = null!;

        public ICollection<Student> Students { get; set; }

        public virtual ICollection<StudentSection> StudentSections { get; set; }

    }
}
