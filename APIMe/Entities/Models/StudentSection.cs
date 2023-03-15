using System;
using System.Collections.Generic;

namespace APIMe.Entities.Models
{
    public partial class StudentSection
    {
        public int SectionId { get; set; }
        public int StudentId { get; set; }

        public Section Section { get; set; } = null!;
        public Student Student { get; set; } = null!;


    }
}
