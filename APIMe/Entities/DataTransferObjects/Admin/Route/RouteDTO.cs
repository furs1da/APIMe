namespace APIMe.Entities.DataTransferObjects.Admin.Route
{
    public class RouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RouteTypeId { get; set; }
        public string DataTableName { get; set; }
        public bool IsVisible { get; set; }

        // RouteType properties
        public string RouteTypeName { get; set; }
        public string RouteTypeResponseCode { get; set; }

        public string RouteTypeCrudActionName { get; set; }
        public int RouteTypeCrudActionId { get; set; }

        // Records from uncertain tables
        public IList<object> Records { get; set; }
    }
}
