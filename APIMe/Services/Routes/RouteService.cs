using APIMe.Entities.Constants;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Entities.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using System;

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
        public async Task<object> GetRecordByIdFromDataTableAsync(string tableName, int id)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var records = await table.Cast<object>().ToListAsync();
            var record = records.FirstOrDefault(r => (int)r.GetType().GetProperty("Id").GetValue(r) == id);
            return record;
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
                        var addedRecord = await AddRecordToDataTableAsync(route.DataTableName, (JsonElement)data);
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
                        var updatedRecord = await UpdateRecordInDataTableAsync(route.DataTableName, (JsonElement)data);
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
                        var patchedRecord = await UpdateRecordInDataTableAsync(route.DataTableName, (JsonElement)data);
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
                        await DeleteRecordFromDataTableAsync(route.DataTableName, (JsonElement)data);
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
            catch (DbUpdateConcurrencyException ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = "The record you tried to update or delete may have already been changed or deleted by another user." };
            }
            catch (DbUpdateException ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = "An error occurred while updating the database. Please check the data you provided and try again." };
            }
            catch (InvalidOperationException ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = $"An invalid operation occurred: {ex.Message}" };
            }
            catch (ArgumentNullException ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = $"A required argument was not provided: {ex.Message}" };
            }
            catch (ArgumentException ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = $"An argument error occurred: {ex.Message}" };
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = $"An error occurred while processing JSON data: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new TestRouteResponse { StatusCode = 500, Message = $"An error occurred: {ex.Message}" };
            }
        }

        public virtual async Task<object> AddRecordToDataTableAsync(string tableName, JsonElement recordJson)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var entityType = table.ElementType;

            // Use Newtonsoft.Json for deserialization
            var recordString = recordJson.GetRawText();
            var record = JsonConvert.DeserializeObject(recordString, entityType);

            // Use the 'Add' method to add the entity to the DbContext
            var addMethod = dbContextType.GetMethods()
                .FirstOrDefault(m => m.Name == "Add" && m.IsGenericMethod && m.GetParameters().Length == 1);
            if (addMethod == null)
            {
                throw new InvalidOperationException("The 'Add' method could not be found.");
            }
            addMethod = addMethod.MakeGenericMethod(table.ElementType);


            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Enable IDENTITY_INSERT for the 'Products' table
                    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} ON");

                    // Add the record and save changes
                    addMethod.Invoke(_context, new[] { record });
                    await _context.SaveChangesAsync();

                    // Disable IDENTITY_INSERT for the 'Products' table
                    await _context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {tableName} OFF");

                    // Commit the transaction
                    await transaction.CommitAsync();
                }
                catch
                {
                    // Rollback the transaction if an error occurs
                    await transaction.RollbackAsync();
                    throw;
                }
            }


            return record;
        }

        public async Task<object> UpdateRecordInDataTableAsync(string tableName, JsonElement recordJson)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var entityType = table.ElementType;

            // Use Newtonsoft.Json for deserialization
            var recordString = recordJson.GetRawText();
            var record = JsonConvert.DeserializeObject(recordString, entityType);

            // Find and load the existing entity from the database
            var keyValues = _context.Model.FindEntityType(entityType).FindPrimaryKey().Properties
                .Select(p => p.PropertyInfo.GetValue(record)).ToArray();
            var existingEntity = await _context.FindAsync(entityType, keyValues);

            if (existingEntity == null)
            {
                throw new InvalidOperationException($"The entity with the specified key values could not be found.");
            }

            // Copy the properties from the updated record to the existing entity
            var entityProperties = entityType.GetProperties();
            foreach (var property in entityProperties)
            {
                var newValue = property.GetValue(record);
                property.SetValue(existingEntity, newValue);
            }

            // Save the changes
            await _context.SaveChangesAsync();

            return existingEntity;
        }

        public async Task<object> PatchRecordInDataTableAsync(string tableName, int id, JsonElement recordJson)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var entityType = table.ElementType;

            // Find and load the existing entity from the database
            var existingEntity = await _context.FindAsync(entityType, id);

            if (existingEntity == null)
            {
                throw new InvalidOperationException($"The entity with the specified ID could not be found.");
            }

            // Copy the properties from the updated record to the existing entity
            foreach (var property in recordJson.EnumerateObject())
            {
                var propertyName = property.Name;
                propertyName= string.Concat(propertyName.Substring(0,1).ToUpper(), propertyName.AsSpan(1));
                var newValue = property.Value;
                var entityProperty = entityType.GetProperty(propertyName);

                if (entityProperty != null && entityProperty.CanWrite)
                {
                    object convertedValue = newValue.ValueKind == JsonValueKind.Null
                        ? null
                        : JsonConvert.DeserializeObject(newValue.GetRawText(), entityProperty.PropertyType);

                    entityProperty.SetValue(existingEntity, convertedValue);
                }
            }


            // Save the changes
            await _context.SaveChangesAsync();

            return existingEntity;
        }
        public async Task DeleteRecordFromDataTableAsync(string tableName, JsonElement recordJson)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var entityType = table.ElementType;

            // Use Newtonsoft.Json for deserialization
            var recordString = recordJson.GetRawText();
            var record = JsonConvert.DeserializeObject(recordString, entityType);

            // Attach the entity to the DbContext and set its state to 'Deleted'
            _context.Attach(record);
            var entityEntry = _context.Entry(record);

            if (entityEntry.State != EntityState.Unchanged && entityEntry.State != EntityState.Detached)
            {
                throw new InvalidOperationException($"The entity is in an unexpected state: {entityEntry.State}.");
            }

            entityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteRecordFromDataTableAsync(string tableName, int id)
        {
            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var entityType = table.ElementType;

            // Find and load the existing entity from the database
            var existingEntity = await _context.FindAsync(entityType, id);

            if (existingEntity == null)
            {
                throw new InvalidOperationException($"The entity with the specified ID could not be found.");
            }

            // Attach the entity to the DbContext and set its state to 'Deleted'
            _context.Attach(existingEntity);
            var entityEntry = _context.Entry(existingEntity);

            if (entityEntry.State != EntityState.Unchanged && entityEntry.State != EntityState.Detached)
            {
                throw new InvalidOperationException($"The entity is in an unexpected state: {entityEntry.State}.");
            }

            entityEntry.State = EntityState.Deleted;

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
                // Find the entityType by comparing the name instead of using FindEntityType
                var entityType = _context.Model.GetEntityTypes()
                    .FirstOrDefault(et => et.ClrType.Name.Equals(dataTable))?.ClrType;

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

        public async Task<List<Property>> GetPropertiesByTableNameAsync(string tableName)
        {
            var properties = new List<Property>();

            var dbContextType = _context.GetType();
            var tableProperty = dbContextType.GetProperty(tableName);
            if (tableProperty == null)
            {
                throw new InvalidOperationException($"The table '{tableName}' does not exist in the DbContext.");
            }

            var table = (IQueryable)tableProperty.GetValue(_context);
            var entityType = table.ElementType;

            foreach (var prop in entityType.GetProperties())
            {
                properties.Add(new Property { Name = prop.Name, Type = prop.PropertyType.Name });
            }

            return properties;
        }
    }
}
