using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIMe.Migrations
{
    public partial class routetypw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RouteType",
                columns: new[] { "id", "crudId", "name", "responseCode" },
                values: new object[,]
                {
                    { 1, 1, "GET - 200 (OK)", "200" },
                    { 2, 1, "GET - 201 (Created)", "201" },
                    { 3, 1, "GET - 204 (No Content)", "204" },
                    { 4, 1, "GET - 400 (Bad Request)", "400" },
                    { 5, 1, "GET - 404 (Not Found)", "404" },
                    { 6, 2, "POST - 200 (OK)", "200" },
                    { 7, 2, "POST - 201 (Created)", "201" },
                    { 8, 2, "POST - 204 (No Content)", "204" },
                    { 9, 2, "POST - 400 (Bad Request)", "400" },
                    { 10, 2, "POST - 409 (Conflict)", "409" },
                    { 11, 3, "PUT - 200 (OK)", "200" },
                    { 12, 3, "PUT - 204 (No Content)", "204" },
                    { 13, 3, "PUT - 400 (Bad Request)", "400" },
                    { 14, 3, "PUT - 404 (Not Found)", "404" },
                    { 15, 4, "PATCH - 200 (OK)", "200" },
                    { 16, 4, "PATCH - 204 (No Content)", "204" },
                    { 17, 4, "PATCH - 400 (Bad Request)", "400" },
                    { 18, 4, "PATCH - 404 (Not Found)", "404" },
                    { 19, 5, "DELETE - 200 (OK)", "200" },
                    { 20, 5, "DELETE - 204 (No Content)", "204" },
                    { 21, 5, "DELETE - 400 (Bad Request)", "400" },
                    { 22, 5, "DELETE - 404 (Not Found)", "404" },
                    { 23, 6, "ERROR - 400 (Bad Request)", "400" },
                    { 24, 6, "ERROR - 401 (Unauthorized)", "401" },
                    { 25, 6, "ERROR - 403 (Forbidden)", "403" },
                    { 26, 6, "ERROR - 404 (Not Found)", "404" },
                    { 27, 6, "ERROR - 405 (Method Not Allowed)", "405" },
                    { 28, 6, "ERROR - 409 (Conflict)", "409" },
                    { 29, 6, "ERROR - 429 (Too Many Requests)", "429" },
                    { 30, 6, "ERROR - 500 (Internal Server Error)", "500" },
                    { 31, 6, "ERROR - 502 (Bad Gateway)", "502" },
                    { 32, 6, "ERROR - 503 (Service Unavailable)", "503" },
                    { 33, 6, "ERROR - 504 (Gateway Timeout)", "504" }
                });

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
