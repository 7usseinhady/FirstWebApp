using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                schema: "auth",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d0b1b383-e64c-4f42-b732-7ffbe8f3666b", "618fdfd2-f08b-413d-876a-04fec17f9e3f" });

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d0b1b383-e64c-4f42-b732-7ffbe8f3666b");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                schema: "auth",
                table: "Roles");

            migrationBuilder.AddColumn<bool>(
                name: "IsInactive",
                schema: "auth",
                table: "UserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserInsertDate",
                schema: "auth",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserInsertId",
                schema: "auth",
                table: "UserRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserUpdateDate",
                schema: "auth",
                table: "UserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdateId",
                schema: "auth",
                table: "UserRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInactive",
                schema: "auth",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserInsertDate",
                schema: "auth",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserInsertId",
                schema: "auth",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserUpdateDate",
                schema: "auth",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdateId",
                schema: "auth",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d0b1b383-e64c-4f42-b732-7ffbe8f3666b", "f4912daa-a439-43ea-9c5d-dbd590789948", "Admin", "admin" });

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAEBFH9I4Mcxx+Ll+ksZqdpmtYlbbh22EGAu1PjRh+nDh7/1A8s6+SRkjtI/0fVY68SA==", "97f9be89-0b12-4a29-bf24-9590560ce431" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d0b1b383-e64c-4f42-b732-7ffbe8f3666b", "618fdfd2-f08b-413d-876a-04fec17f9e3f" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserInsertId",
                schema: "auth",
                table: "UserRoles",
                column: "UserInsertId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserUpdateId",
                schema: "auth",
                table: "UserRoles",
                column: "UserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserInsertId",
                schema: "auth",
                table: "Roles",
                column: "UserInsertId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserUpdateId",
                schema: "auth",
                table: "Roles",
                column: "UserUpdateId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_AspNetRoles_RoleId",
                schema: "auth",
                table: "RoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_AspNetRoles_Id",
                schema: "auth",
                table: "Roles",
                column: "Id",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_UserRoles_AspNetRoles_RoleId",
                schema: "auth",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_AspNetUserRoles_UserId_RoleId",
                schema: "auth",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                principalTable: "AspNetUserRoles",
                principalColumns: new[] { "UserId", "RoleId" },
                onDelete: ReferentialAction.Cascade);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_AspNetRoles_RoleId",
                schema: "auth",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_AspNetRoles_Id",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserInsertId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserUpdateId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AspNetRoles_RoleId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_AspNetUserRoles_UserId_RoleId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserInsertId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserUpdateId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserInsertId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserUpdateId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserInsertId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserUpdateId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsInactive",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserInsertDate",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserInsertId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserUpdateDate",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserUpdateId",
                schema: "auth",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "IsInactive",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserInsertDate",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserInsertId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserUpdateDate",
                schema: "auth",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserUpdateId",
                schema: "auth",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "auth",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "auth",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                schema: "auth",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d0b1b383-e64c-4f42-b732-7ffbe8f3666b", "f4912daa-a439-43ea-9c5d-dbd590789948", "Admin", "admin" });

            migrationBuilder.UpdateData(
                schema: "auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: "618fdfd2-f08b-413d-876a-04fec17f9e3f",
                columns: new[] { "PasswordHash", "SecurityStamp" },
                values: new object[] { "AQAAAAIAAYagAAAAELqBHsi4Iuk26RUPyUAjWXmYY2RL8RFwZbZWNa6OvmxDMcBPs6V5GLaIfVCoBxf2Tw==", "24af33a5-5dd4-4996-9fe8-1001bed4d38f" });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d0b1b383-e64c-4f42-b732-7ffbe8f3666b", "618fdfd2-f08b-413d-876a-04fec17f9e3f" });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "auth",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                schema: "auth",
                table: "RoleClaims",
                column: "RoleId",
                principalSchema: "auth",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                schema: "auth",
                table: "UserRoles",
                column: "RoleId",
                principalSchema: "auth",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
