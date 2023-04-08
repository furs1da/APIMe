namespace APIMe.Entities.DataTransferObjects.Admin.RouteLog
{
    public class RouteLogDto
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public string FullName { get; set; }
        public string HttpMethod { get; set; }
        public string TableName { get; set; }
        public int? RecordId { get; set; }
        public string RoutePath { get; set; }
    }
}
