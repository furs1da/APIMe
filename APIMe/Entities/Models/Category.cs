namespace APIMe.Entities.Models
{
    public partial class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

}
