using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIMe.Migrations
{
    public partial class InitialRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "48376749-5c25-445e-83a1-ee1b121463ae", "aec1ac1b-e59a-461a-8a4d-432c2462094a", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6a7b014c-9123-4558-8ee1-92b389247451", "865f3896-4715-41ca-9149-3d325d5bb014", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48376749-5c25-445e-83a1-ee1b121463ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a7b014c-9123-4558-8ee1-92b389247451");
        }
    }
}
