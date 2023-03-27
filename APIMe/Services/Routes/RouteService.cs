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

        public async Task<IList<object>> GetRecordsFromDataTableAsync(string tableName, int numberOfRecords)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var records = await table.Cast<object>().Take(numberOfRecords).ToListAsync();
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
                routeDto.Records = await GetRecordsFromDataTableAsync(route.DataTableName, 5);
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


        public async Task<TestRouteResponse> TestRouteAsync(int routeId)
        {
            var route = await _context.Routes.Include(r => r.RouteType).FirstOrDefaultAsync(r => r.Id == routeId);
            if (route == null)
            {
                return new TestRouteResponse { StatusCode = 404, Message = "Route not found.", Records = null };
            }

            var routeType = route.RouteType.Name;
            var statusCode = int.Parse(route.RouteType.ResponseCode);
            var dataTable = route.DataTableName;

            if (routeType.StartsWith("GET") && statusCode != 204 && statusCode != 400)
            {
                var records = await GetRecordsFromDataTableAsync(dataTable, 20);
                return new TestRouteResponse { StatusCode = statusCode, Message = $"Successfully retrieved {records.Count} records.", Records = records };
            }
            else if (routeType.StartsWith("POST") || routeType.StartsWith("PUT"))
            {
                var record = await GetRecordsFromDataTableAsync(dataTable, 1);
                return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully added/updated a record.", Records = record };
            }
            else if (routeType.StartsWith("DELETE"))
            {
                return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully deleted a record.", Records = null };
            }
            else
            {
                return new TestRouteResponse { StatusCode = statusCode, Message = "An error occurred.", Records = null };
            }
        }

        public async Task<List<Property>> GetPropertiesByRouteIdAsync(int routeId)
        {
            var properties = new List<Property>();

            var route = await _context.Routes.FirstOrDefaultAsync(rt => rt.Id == routeId);
            if (route == null)
            {
                return properties;
            }

            var dataTable = route.DataTableName;
            if (!string.IsNullOrEmpty(dataTable))
            {
                var entityType = _context.Model.FindEntityType(dataTable).ClrType;
                foreach (var prop in entityType.GetProperties())
                {
                    properties.Add(new Property { Name = prop.Name, Type = prop.PropertyType.Name });
                }
            }

            return properties;
        }



    }
}
