using APIMe.Entities.DataTransferObjects.Admin.Student;

namespace APIMe.Entities.DataTransferObjects.Admin.Section
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public string ProfessorName { get; set; }
        public string AccessCode { get; set; }
        public int? NumberOfStudents { get; set; }

        public List<StudentDto>? Students { get; set; }
    }
}
