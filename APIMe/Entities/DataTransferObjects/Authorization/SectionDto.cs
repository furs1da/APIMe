using APIMe.Entities.Models;

namespace APIMe.Entities.DataTransferObjects.Authorization
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int ProfessorId { get; set; }
        public virtual Professor Professor { get; set; }
    }
}
