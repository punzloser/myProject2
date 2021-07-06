using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class changetypeofFileSizeinProductImagefrominttolong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 7, 6, 13, 22, 45, 945, DateTimeKind.Local).AddTicks(7461));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("70834739-9213-4c00-9936-ed75eaf822d7"),
                column: "ConcurrencyStamp",
                value: "ca2fb748-3c5c-474d-bca1-ac3874d242c1");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1cfa0087-d99f-4f3e-9cef-97a515fbfd2f", "AQAAAAEAACcQAAAAEDuFlo8x0gT587h5+tZh8TQcXSRDNsr3QJSbcRLAAZtk3F85fmkHo3hlvstpIgTGKg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 7, 5, 13, 59, 7, 791, DateTimeKind.Local).AddTicks(7880));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("70834739-9213-4c00-9936-ed75eaf822d7"),
                column: "ConcurrencyStamp",
                value: "eb9833d5-eade-413e-83e7-7aa59f95dbf7");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "41e365f5-3690-4a53-8750-56a161e1dcab", "AQAAAAEAACcQAAAAEAFEyY/icEpujQa2PW7rk7X6iOKLVTmh2uSwW+tjgtZdVJ5E/5jx8w1YJXBVH8PcXA==" });
        }
    }
}
