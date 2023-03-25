using APIMe.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static IdentityModel.OidcConstants;

namespace APIMe.Entities.Configuration
{
    public class RouteTypeConfiguration : IEntityTypeConfiguration<RouteType>
    {
        public void Configure(EntityTypeBuilder<RouteType> entity)
        {
            entity.HasData(
                new RouteType { Id = 1, Name = "GET - 200 (OK)", ResponseCode = "200", CrudId = 1 },
                new RouteType { Id = 2, Name = "GET - 201 (Created)", ResponseCode = "201", CrudId = 1 },
                new RouteType { Id = 3, Name = "GET - 204 (No Content)", ResponseCode = "204", CrudId = 1 },
                new RouteType { Id = 4, Name = "GET - 400 (Bad Request)", ResponseCode = "400", CrudId = 1 },
                new RouteType { Id = 5, Name = "GET - 404 (Not Found)", ResponseCode = "404", CrudId = 1 },
                new RouteType { Id = 6, Name = "POST - 200 (OK)", ResponseCode = "200", CrudId = 2 },
                new RouteType { Id = 7, Name = "POST - 201 (Created)", ResponseCode = "201", CrudId = 2 },
                new RouteType { Id = 8, Name = "POST - 204 (No Content)", ResponseCode = "204", CrudId = 2 },
                new RouteType { Id = 9, Name = "POST - 400 (Bad Request)", ResponseCode = "400", CrudId = 2 },
                new RouteType { Id = 10, Name = "POST - 409 (Conflict)", ResponseCode = "409", CrudId = 2 },
                new RouteType { Id = 11, Name = "PUT - 200 (OK)", ResponseCode = "200", CrudId = 3 },
                new RouteType { Id = 12, Name = "PUT - 204 (No Content)", ResponseCode = "204", CrudId = 3 },
                new RouteType { Id = 13, Name = "PUT - 400 (Bad Request)", ResponseCode = "400", CrudId = 3 },
                new RouteType { Id = 14, Name = "PUT - 404 (Not Found)", ResponseCode = "404", CrudId = 3 },
                new RouteType { Id = 15, Name = "PATCH - 200 (OK)", ResponseCode = "200", CrudId = 4 },
                new RouteType { Id = 16, Name = "PATCH - 204 (No Content)", ResponseCode = "204", CrudId = 4 },
                new RouteType { Id = 17, Name = "PATCH - 400 (Bad Request)", ResponseCode = "400", CrudId = 4 },
                new RouteType { Id = 18, Name = "PATCH - 404 (Not Found)", ResponseCode = "404", CrudId = 4 },
                new RouteType { Id = 19, Name = "DELETE - 200 (OK)", ResponseCode = "200", CrudId = 5 },
                new RouteType { Id = 20, Name = "DELETE - 204 (No Content)", ResponseCode = "204", CrudId = 5 },
                new RouteType { Id = 21, Name = "DELETE - 400 (Bad Request)", ResponseCode = "400", CrudId = 5 },
                new RouteType { Id = 22, Name = "DELETE - 404 (Not Found)", ResponseCode = "404", CrudId = 5 },
                new RouteType { Id = 23, Name = "ERROR - 400 (Bad Request)", ResponseCode = "400", CrudId = 6 },
                new RouteType { Id = 24, Name = "ERROR - 401 (Unauthorized)", ResponseCode = "401", CrudId = 6 },
                new RouteType { Id = 25, Name = "ERROR - 403 (Forbidden)", ResponseCode = "403", CrudId = 6 },
                new RouteType { Id = 26, Name = "ERROR - 404 (Not Found)", ResponseCode = "404", CrudId = 6 },
                new RouteType { Id = 27, Name = "ERROR - 405 (Method Not Allowed)", ResponseCode = "405", CrudId = 6 },
                new RouteType { Id = 28, Name = "ERROR - 409 (Conflict)", ResponseCode = "409", CrudId = 6 },
                new RouteType { Id = 29, Name = "ERROR - 429 (Too Many Requests)", ResponseCode = "429", CrudId = 6 },
                new RouteType { Id = 30, Name = "ERROR - 500 (Internal Server Error)", ResponseCode = "500", CrudId = 6 },
                new RouteType { Id = 31, Name = "ERROR - 502 (Bad Gateway)", ResponseCode = "502", CrudId = 6 },
                new RouteType { Id = 32, Name = "ERROR - 503 (Service Unavailable)", ResponseCode = "503", CrudId = 6 },
                new RouteType { Id = 33, Name = "ERROR - 504 (Gateway Timeout)", ResponseCode = "504", CrudId = 6 }
            );
        }
    }
}
