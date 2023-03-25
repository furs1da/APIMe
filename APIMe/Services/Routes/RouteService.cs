using APIMe.Entities.Constants;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace APIMe.Services.Routes
{
    public class RouteService
    {
        private readonly APIMeContext _context;
        private readonly IMapper _mapper;

        public RouteService(APIMeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Other methods for managing routes, e.g., GetRoutes, CreateRoute, etc.

        public async Task<IList<object>> GetRecordsFromDataTableAsync(string tableName)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var records = await table.Cast<object>().ToListAsync();
            return records;
        }

        public async Task<Entities.Models.Route> CreateRouteAsync(Entities.Models.Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<Entities.Models.Route> UpdateRouteAsync(Entities.Models.Route updatedRoute)
        {
            _context.Entry(updatedRoute).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatedRoute;
        }

        public bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.Id == id);
        }

        public async Task<bool> DeleteRouteAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return false;
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleVisibilityAsync(int id)
        {
            var route = await _context.Routes.FindAsync(id);

            if (route == null)
            {
                return false;
            }

            route.IsVisible = !route.IsVisible;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RouteDto>> GetRoutesAsync()
        {
            var routes = await _context.Routes.Include(r => r.RouteType).ToListAsync();
            var routeDtos = _mapper.Map<IEnumerable<RouteDto>>(routes);

            foreach (var routeDto in routeDtos)
            {
                var route = routes.First(r => r.Id == routeDto.Id);
                routeDto.Records = await GetRecordsFromDataTableAsync(route.DataTableName);
            }

            return routeDtos;
        }

        public async Task<IEnumerable<RouteTypeDto>> GetRouteTypesAsync()
        {
            var routes = await _context.RouteTypes.ToListAsync();
            var routeDtos = _mapper.Map<IEnumerable<RouteTypeDto>>(routes);

            return routeDtos;
        }
        public async Task<IEnumerable<DataSourceDTO>> GetDataSourcesAsync()
        {
            return DataSourceTables.DataSources.ToList();
        }

    }
}
