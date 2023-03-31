using APIMe.Entities.Constants;
using APIMe.Entities.DataTransferObjects.Admin.Route;
using APIMe.Entities.Models;
using APIMe.Services.Routes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIMeTests
{
    internal class MockRouteService : RouteService
    {
        private readonly APIMeContext _context;
        private readonly IMapper _mapper;
        public MockRouteService(APIMeContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public override async Task<object> AddRecordToDataTableAsync(string tableName, JsonElement recordJson)
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
                    switch (entityType.Name)
                    {
                        case nameof(Products):
                            _context.Products.Add((Products)record);
                            break;
                        case nameof(Customers):
                            _context.Customers.Add((Customers)record);
                            break;
                        case nameof(Supplier):
                            _context.Suppliers.Add((Supplier)record);
                            break;
                        case nameof(Payment):
                            _context.Payments.Add((Payment)record);
                            break;
                        case nameof(Employee):
                            _context.Employees.Add((Employee)record);
                            break;
                        case nameof(Inventory):
                            _context.Inventories.Add((Inventory)record);
                            break;
                        case nameof(Category):
                            _context.Categories.Add((Category)record);
                            break;
                        case nameof(Order):
                            _context.Orders.Add((Order)record);
                            break;
                            default:
                            throw new InvalidOperationException("Table Not Available For Testing");
                    };
                    
                     if (entityType.IsInstanceOfType(typeof(Supplier)))
                    {
                        
                    }
                    else if (entityType.IsInstanceOfType(typeof(Payment)))
                    {
                        
                    }
                    else if (entityType.IsInstanceOfType(typeof(Employee)))
                    {
                       
                    }
                    else if (entityType.IsInstanceOfType(typeof(Inventory)))
                    {
                       
                    }
                    else if (entityType.IsInstanceOfType(typeof(Customers)))
                    {
                      
                    }
                    else if (entityType.IsInstanceOfType(typeof(Category)))
                    {
                       
                    }
                    else if (entityType.IsInstanceOfType(typeof(Order)))
                    {
                        
                    }

                    // Add the record and save changes
                    await _context.SaveChangesAsync();

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
    }
}
