using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class addProductImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 6, 17, 22, 15, 24, 482, DateTimeKind.Local).AddTicks(7585));

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileSize = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 6, 17, 22, 15, 24, 482, DateTimeKind.Local).AddTicks(7585),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 17, 22, 15, 24, 497, DateTimeKind.Local).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("70834739-9213-4c00-9936-ed75eaf822d7"),
                column: "ConcurrencyStamp",
                value: "e94e1aed-b4f6-48b2-b881-972014da7d0f");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("48c2b994-33ab-439b-9d6f-a5318916aff6"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "20998432-775d-4a4f-a282-4499f8a149ca", "AQAAAAEAACcQAAAAEKRgNZpTWX01Lr8zMjkBnq/sMHzIi5T46sROtLo6Ia080b0BYLuci/1CU4lJwKo35A==" });
        }
    }
}
