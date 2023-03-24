namespace APIMe.Entities.DataTransferObjects.Admin.Route
{
    public class RouteTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ResponseCode { get; set; }
        public string? CrudActionName { get; set; }
        public int? CrudActionId { get; set; }
    }
}
