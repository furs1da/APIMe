using APIMe.Entities.Models;

namespace APIMe.Interfaces
{
    public interface IRouteLogService
    {
        Task AddRouteLogAsync(RouteLog routeLog);
        Task LogRequestAsync(HttpContext context, string tableName, int? recordId = null);
        Task<IEnumerable<RouteLog>> GetAllRouteLogsAsync();
        Task<IEnumerable<RouteLog>> GetRouteLogsByUserIdAsync(int userId);
    }
}
