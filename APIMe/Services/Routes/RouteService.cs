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

        public async Task<TestRouteResponse> TestRouteAsync(int routeId, object data)
        {
            var route = await _context.Routes.Include(rt => rt.RouteType).FirstOrDefaultAsync(rt => rt.Id == routeId);
            if (route == null)
            {
                return new TestRouteResponse { StatusCode = 404, Message = "Route not found." };
            }

            var routeType = route.RouteType.Name;
            var statusCode = int.Parse(route.RouteType.ResponseCode);

            try
            {
                if (routeType.StartsWith("GET"))
                {
                    if (statusCode >= 200 && statusCode < 300)
                    {
                        var records = await GetRecordsFromDataTableAsync(route.DataTableName, 20);
                        return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully retrieved records.", Records = records };
                    }
                    else
                    {
                        return new TestRouteResponse { StatusCode = statusCode, Message = $"GET request error: {routeType}" };
                    }
                }
                else if (routeType.StartsWith("POST"))
                {
                    if (statusCode >= 200 && statusCode < 300)
                    {
                        var addedRecord = await AddRecordToDataTableAsync(route.DataTableName, data);
                        return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully added a new record.", Records = new List<object> { addedRecord } };
                    }
                    else
                    {
                        return new TestRouteResponse { StatusCode = statusCode, Message = $"POST request error: {routeType}" };
                    }
                }
                else if (routeType.StartsWith("PUT"))
                {
                    if (statusCode >= 200 && statusCode < 300)
                    {
                        var updatedRecord = await UpdateRecordInDataTableAsync(route.DataTableName, data);
                        return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully updated a record.", Records = new List<object> { updatedRecord } };
                    }
                    else
                    {
                        return new TestRouteResponse { StatusCode = statusCode, Message = $"PUT request error: {routeType}" };
                    }
                }
                else if (routeType.StartsWith("PATCH"))
                {
                    if (statusCode >= 200 && statusCode < 300)
                    {
                        var patchedRecord = await UpdateRecordInDataTableAsync(route.DataTableName, data);
                        return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully patched a record.", Records = new List<object> { patchedRecord } };
                    }
                    else
                    {
                        return new TestRouteResponse { StatusCode = statusCode, Message = $"PATCH request error: {routeType}" };
                    }
                }
                else if (routeType.StartsWith("DELETE"))
                {
                    if (statusCode >= 200 && statusCode < 300)
                    {
                        await DeleteRecordFromDataTableAsync(route.DataTableName, data);
                        return new TestRouteResponse { StatusCode = statusCode, Message = "Successfully deleted a record." };
                    }
                    else
                    {
                        return new TestRouteResponse { StatusCode = statusCode, Message = $"DELETE request error: {routeType}" };
                    }
                }
                else
                {
                    return new TestRouteResponse { StatusCode = statusCode, Message = $"General error: {routeType}" };
                }
            }
            catch (Exception ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public async Task<object> AddRecordToDataTableAsync(string tableName, object record)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var addMethod = dbContextType.GetMethod("Add");
            addMethod.MakeGenericMethod(table.ElementType).Invoke(_context, new[] { record });

            await _context.SaveChangesAsync();

            return record;
        }

        public async Task<object> UpdateRecordInDataTableAsync(string tableName, object record)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var updateMethod = dbContextType.GetMethod("Update");
            updateMethod.MakeGenericMethod(table.ElementType).Invoke(_context, new[] { record });

            await _context.SaveChangesAsync();

            return record;
        }

        public async Task DeleteRecordFromDataTableAsync(string tableName, object record)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var removeMethod = dbContextType.GetMethod("Remove");
            removeMethod.MakeGenericMethod(table.ElementType).Invoke(_context, new[] { record });

            await _context.SaveChangesAsync();
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
                var entityType = _context.Model.FindEntityType(dataTable)?.ClrType;
                if (entityType == null)
                {
                    return properties;
                }

                foreach (var prop in entityType.GetProperties())
                {
                    properties.Add(new Property { Name = prop.Name, Type = prop.PropertyType.Name });
                }
            }

            return properties;
        }




    }
}
