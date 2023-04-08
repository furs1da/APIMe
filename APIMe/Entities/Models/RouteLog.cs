namespace APIMe.Entities.Models
{
    public partial class RouteLog
    {
        public int Id { get; set; }
        public string Ipaddress { get; set; } = null!;
        public DateTime RequestTimestamp { get; set; }
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string HttpMethod { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public int? RecordId { get; set; }
        public string RoutePath { get; set; } = null!;

        public virtual Student? User { get; set; }
    }
}
