using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserUpdateId",
                schema: "auth",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserInsertId",
                schema: "auth",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAELqBHsi4Iuk26RUPyUAjWXmYY2RL8RFwZbZWNa6OvmxDMcBPs6V5GLaIfVCoBxf2Tw==", "24af33a5-5dd4-4996-9fe8-1001bed4d38f" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserInsertId",
                schema: "auth",
                table: "Users",
                column: "UserInsertId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserUpdateId",
                schema: "auth",
                table: "Users",
                column: "UserUpdateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserInsertId",
                schema: "auth",
                table: "Users",
                column: "UserInsertId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UserUpdateId",
                schema: "auth",
                table: "Users",
                column: "UserUpdateId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserInsertId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserUpdateId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserInsertId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserUpdateId",
                schema: "auth",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserUpdateId",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserInsertId",
                schema: "auth",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEPdyMBRicyX+tQqkYdtUn5J/5Wv7rz/50yhwtAA95NQCBGMcuw7f1R29pP5MI6hHag==", "4d5e3d01-3e51-4a41-a2c2-f4a68c5d8635" });
        }
    }
}
