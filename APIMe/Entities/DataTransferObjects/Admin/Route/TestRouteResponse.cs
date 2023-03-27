namespace APIMe.Entities.DataTransferObjects.Admin.Route
{
    public class TestRouteResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Object> Records { get; set; } = new List<Object>();
    }
}
