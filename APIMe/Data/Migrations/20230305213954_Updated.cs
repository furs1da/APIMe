using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIMe.Data.Migrations
{
    public partial class Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "Professor",
                columns: new[] { "id", "email", "firstName", "lastName" },
                values: new object[] { 1, "bbilkhu@conestogac.on.ca", "Baljeet", "Bilkhu" });

            migrationBuilder.InsertData(
                table: "Professor",
                columns: new[] { "id", "email", "firstName", "lastName" },
                values: new object[] { 2, "apimeconestoga@gmail.com", "John", "Doe" });

            migrationBuilder.InsertData(
                table: "Section",
                columns: new[] { "id", "accessCode", "professorId", "sectionName" },
                values: new object[] { 1, "1234", 1, "SEC-1" });

            migrationBuilder.InsertData(
                table: "Section",
                columns: new[] { "id", "accessCode", "professorId", "sectionName" },
                values: new object[] { 2, "1235", 1, "SEC-2" });

            migrationBuilder.InsertData(
                table: "Section",
                columns: new[] { "id", "accessCode", "professorId", "sectionName" },
                values: new object[] { 3, "1237", 1, "SEC-3" });

            migrationBuilder.CreateIndex(
                name: "IX_BugFeature_projectId",
                table: "BugFeature",
                column: "projectId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_userId",
                table: "Project",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_routeTypeId",
                table: "Route",
                column: "routeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLog_routeId",
                table: "RouteLog",
                column: "routeId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteLog_studentId",
                table: "RouteLog",
                column: "studentId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_professorId",
                table: "Section",
                column: "professorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSection_sectionId",
                table: "StudentSection",
                column: "sectionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSection_studentId",
                table: "StudentSection",
                column: "studentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BugFeature");

            migrationBuilder.DropTable(
                name: "RouteLog");

            migrationBuilder.DropTable(
                name: "SectionStudent");

            migrationBuilder.DropTable(
                name: "StudentSection");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "RouteType");

            migrationBuilder.DropTable(
                name: "Professor");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

        }
    }
}