using APIMe.Entities.DataTransferObjects.Admin.Section;

namespace APIMe.Entities.DataTransferObjects.Admin.Student
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StudentId { get; set; }
        public string ApiKey { get; set; }
        public List<SectionDTO>? Sections { get; set; }
    }

}
