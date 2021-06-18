using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class seed_identity_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 17, 22, 15, 24, 482, DateTimeKind.Local).AddTicks(7585),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 6, 17, 21, 30, 41, 236, DateTimeKind.Local).AddTicks(192));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 17, 22, 15, 24, 497, DateTimeKind.Local).AddTicks(1317));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("70834739-9213-4c00-9936-ed75eaf822d7"), "e94e1aed-b4f6-48b2-b881-972014da7d0f", "administration", "administrator", "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("70834739-9213-4c00-9936-ed75eaf822d7"), new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6"), 0, "20998432-775d-4a4f-a282-4499f8a149ca", new DateTime(1995, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "punzloser@gmail.com", true, "Thanh", "Nguyen", false, null, "punzloser@gmail.com", "admin", "AQAAAAEAACcQAAAAEKRgNZpTWX01Lr8zMjkBnq/sMHzIi5T46sROtLo6Ia080b0BYLuci/1CU4lJwKo35A==", null, false, "", false, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("70834739-9213-4c00-9936-ed75eaf822d7"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("70834739-9213-4c00-9936-ed75eaf822d7"), new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 17, 21, 30, 41, 236, DateTimeKind.Local).AddTicks(192),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 6, 17, 22, 15, 24, 482, DateTimeKind.Local).AddTicks(7585));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 17, 21, 30, 41, 250, DateTimeKind.Local).AddTicks(1919));
        }
    }
}
