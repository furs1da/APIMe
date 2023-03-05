using APIMe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIMe.Data.SeedData
{
    internal class SeedProfessor : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> entity)
        {
            entity.HasData(
                new Professor { Id = 1, Email = "bbilkhu@conestogac.on.ca", FirstName = "Baljeet", LastName = "Bilkhu" },
                new Professor { Id = 2, Email = "apimeconestoga@gmail.com", FirstName = "John", LastName = "Doe" }
            );
        }
    }
}
