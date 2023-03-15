using APIMe.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIMe.Entities.Configuration
{
    internal class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> entity)
        {
            entity.HasData(
                new Section { Id = 1, SectionName = "SEC-1", ProfessorId = 1, AccessCode = "1234" },
                new Section { Id = 2, SectionName = "SEC-2", ProfessorId = 1, AccessCode = "1235" },
                new Section { Id = 3, SectionName = "SEC-3", ProfessorId = 1, AccessCode = "1237" }
            );
        }
    }
}
