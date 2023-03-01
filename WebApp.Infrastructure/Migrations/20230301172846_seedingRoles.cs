using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "auth",
                table: "Roles",
                columns: new[] { "Id", "IsInactive", "UserInsertDate", "UserInsertId", "UserUpdateDate", "UserUpdateId" },
                values: new object[] { "d0b1b383-e64c-4f42-b732-7ffbe8f3666b", false, null, null, null, null });

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEAPI4PseCEuI5aNJ95q8TW8DNq36VsHP2JXE9e7BXHuR1TbvKLTDj/EK+NoXcszrpg==", "727b8949-3274-4765-89ef-2c53d5df2946" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d0b1b383-e64c-4f42-b732-7ffbe8f3666b");

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEBFH9I4Mcxx+Ll+ksZqdpmtYlbbh22EGAu1PjRh+nDh7/1A8s6+SRkjtI/0fVY68SA==", "97f9be89-0b12-4a29-bf24-9590560ce431" });
        }
    }
}
