using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailsId);
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailsId", "Count", "CreatedDate", "Price", "ProductId", "ProductName" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(2025, 6, 20, 18, 57, 25, 332, DateTimeKind.Utc).AddTicks(2434), 29.99m, "1", "Wireless Mouse" },
                    { 2, 5, new DateTime(2025, 6, 20, 18, 57, 25, 332, DateTimeKind.Utc).AddTicks(2436), 49.99m, "2", "Bluetooth Speaker" },
                    { 3, 5, new DateTime(2025, 6, 20, 18, 57, 25, 332, DateTimeKind.Utc).AddTicks(2437), 99.99m, "3", "Smart Watch" }
                });

            migrationBuilder.CreateIndex(
                name: "IDX_OrderDetailsId)",
                table: "OrderDetails",
                column: "OrderDetailsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");
        }
    }
}
