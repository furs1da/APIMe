using System;
using System.Collections.Generic;

namespace APIMe.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int StudentId { get; set; }
        public string ApiKey { get; set; } = null!;
        public ICollection<Section> Sections { get; set; }
        public virtual ICollection<StudentSection> StudentSections { get; set; }
    }
}
