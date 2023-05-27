using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserInsertId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserUpdateId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserInsertId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserUpdateId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserInsertId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_UserUpdateId",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocalPhoneNumber",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecondLocalPhoneNumber",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecondPhoneNumber",
                schema: "auth",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserUpdateId",
                schema: "auth",
                table: "Users",
                newName: "ModifiedById");

            migrationBuilder.RenameColumn(
                name: "UserUpdateDate",
                schema: "auth",
                table: "Users",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UserInsertId",
                schema: "auth",
                table: "Users",
                newName: "InsertedById");

            migrationBuilder.RenameColumn(
                name: "UserInsertDate",
                schema: "auth",
                table: "Users",
                newName: "InsertedOn");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserUpdateId",
                schema: "auth",
                table: "Users",
                newName: "IX_Users_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserInsertId",
                schema: "auth",
                table: "Users",
                newName: "IX_Users_InsertedById");

            migrationBuilder.RenameColumn(
                name: "UserUpdateId",
                schema: "auth",
                table: "UserRoles",
                newName: "ModifiedById");

            migrationBuilder.RenameColumn(
                name: "UserUpdateDate",
                schema: "auth",
                table: "UserRoles",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UserInsertId",
                schema: "auth",
                table: "UserRoles",
                newName: "InsertedById");

            migrationBuilder.RenameColumn(
                name: "UserInsertDate",
                schema: "auth",
                table: "UserRoles",
                newName: "InsertedOn");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserUpdateId",
                schema: "auth",
                table: "UserRoles",
                newName: "IX_UserRoles_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_UserInsertId",
                schema: "auth",
                table: "UserRoles",
                newName: "IX_UserRoles_InsertedById");

            migrationBuilder.RenameColumn(
                name: "UserUpdateId",
                schema: "auth",
                table: "Roles",
                newName: "ModifiedById");

            migrationBuilder.RenameColumn(
                name: "UserUpdateDate",
                schema: "auth",
                table: "Roles",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UserInsertId",
                schema: "auth",
                table: "Roles",
                newName: "InsertedById");

            migrationBuilder.RenameColumn(
                name: "UserInsertDate",
                schema: "auth",
                table: "Roles",
                newName: "InsertedOn");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_UserUpdateId",
                schema: "auth",
                table: "Roles",
                newName: "IX_Roles_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_UserInsertId",
                schema: "auth",
                table: "Roles",
                newName: "IX_Roles_InsertedById");

            migrationBuilder.AddColumn<string>(
                name: "MacAdress",
                schema: "auth",
                table: "UserValidationTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "auth",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalPhoneNumber",
                schema: "auth",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalPhoneNumber2",
                schema: "auth",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber2",
                schema: "auth",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacAdress",
                schema: "auth",
                table: "UserRefreshTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "NationalPhoneNumber", "NationalPhoneNumber2", "PasswordHash", "PhoneNumber2", "SecurityStamp" },
                values: new object[] { null, null, "AQAAAAIAAYagAAAAENJB8j83aP7FIU4Vqlb4eMhQLFGHERVZPoKm+9q3ixhHHP6MTQZbBJLN3xnMjfdwMQ==", null, "b98abba0-414e-4de0-a450-c7846cc4e6bb" });

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_InsertedById",
                schema: "auth",
                table: "Roles",
                column: "InsertedById",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_ModifiedById",
                schema: "auth",
                table: "Roles",
                column: "ModifiedById",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_InsertedById",
                schema: "auth",
                table: "UserRoles",
                column: "InsertedById",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_ModifiedById",
                schema: "auth",
                table: "UserRoles",
                column: "ModifiedById",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_InsertedById",
                schema: "auth",
                table: "Users",
                column: "InsertedById",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ModifiedById",
                schema: "auth",
                table: "Users",
                column: "ModifiedById",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_InsertedById",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_ModifiedById",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_InsertedById",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_ModifiedById",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_InsertedById",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ModifiedById",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MacAdress",
                schema: "auth",
                table: "UserValidationTokens");

            migrationBuilder.DropColumn(
                name: "NationalPhoneNumber",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NationalPhoneNumber2",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber2",
                schema: "auth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MacAdress",
                schema: "auth",
                table: "UserRefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "auth",
                table: "Users",
                newName: "UserUpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                schema: "auth",
                table: "Users",
                newName: "UserUpdateId");

            migrationBuilder.RenameColumn(
                name: "InsertedOn",
                schema: "auth",
                table: "Users",
                newName: "UserInsertDate");

            migrationBuilder.RenameColumn(
                name: "InsertedById",
                schema: "auth",
                table: "Users",
                newName: "UserInsertId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ModifiedById",
                schema: "auth",
                table: "Users",
                newName: "IX_Users_UserUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_InsertedById",
                schema: "auth",
                table: "Users",
                newName: "IX_Users_UserInsertId");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "auth",
                table: "UserRoles",
                newName: "UserUpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                schema: "auth",
                table: "UserRoles",
                newName: "UserUpdateId");

            migrationBuilder.RenameColumn(
                name: "InsertedOn",
                schema: "auth",
                table: "UserRoles",
                newName: "UserInsertDate");

            migrationBuilder.RenameColumn(
                name: "InsertedById",
                schema: "auth",
                table: "UserRoles",
                newName: "UserInsertId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_ModifiedById",
                schema: "auth",
                table: "UserRoles",
                newName: "IX_UserRoles_UserUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_InsertedById",
                schema: "auth",
                table: "UserRoles",
                newName: "IX_UserRoles_UserInsertId");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                schema: "auth",
                table: "Roles",
                newName: "UserUpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedById",
                schema: "auth",
                table: "Roles",
                newName: "UserUpdateId");

            migrationBuilder.RenameColumn(
                name: "InsertedOn",
                schema: "auth",
                table: "Roles",
                newName: "UserInsertDate");

            migrationBuilder.RenameColumn(
                name: "InsertedById",
                schema: "auth",
                table: "Roles",
                newName: "UserInsertId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_ModifiedById",
                schema: "auth",
                table: "Roles",
                newName: "IX_Roles_UserUpdateId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_InsertedById",
                schema: "auth",
                table: "Roles",
                newName: "IX_Roles_UserInsertId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "auth",
                table: "Users",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "auth",
                table: "Users",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalPhoneNumber",
                schema: "auth",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondLocalPhoneNumber",
                schema: "auth",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondPhoneNumber",
                schema: "auth",
                table: "Users",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "Code", "LocalPhoneNumber", "PasswordHash", "SecondLocalPhoneNumber", "SecondPhoneNumber", "SecurityStamp" },
                values: new object[] { null, null, "AQAAAAIAAYagAAAAEO2FDolpNXGCQ15kIBfDTQBuFeGURIeTBDqzBRSkSSEqBVl3394vPvmu/cSBNSgvpQ==", null, null, "dd9aadfc-ceae-4a81-9f26-9eab18997fc5" });

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserInsertId",
                schema: "auth",
                table: "Roles",
                column: "UserInsertId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserUpdateId",
                schema: "auth",
                table: "Roles",
                column: "UserUpdateId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserInsertId",
                schema: "auth",
                table: "UserRoles",
                column: "UserInsertId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserUpdateId",
                schema: "auth",
                table: "UserRoles",
                column: "UserUpdateId",
                principalSchema: "auth",
                principalTable: "Users",
                principalColumn: "Id");

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
    }
}
