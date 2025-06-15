using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductDescription = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductCategory = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext", unicode: false, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "CreatedBy", "CreatedDate", "ImageUrl", "ProductCategory", "ProductDescription", "ProductName", "ProductPrice" },
                values: new object[,]
                {
                    { "1", "Seeder", new DateTime(2025, 6, 13, 13, 47, 53, 581, DateTimeKind.Utc).AddTicks(1239), "https://example.com/images/mouse.jpg", "Electronics", "Ergonomic wireless mouse with 2.4GHz connection", "Wireless Mouse", 29.99m },
                    { "2", "Seeder", new DateTime(2025, 6, 13, 13, 47, 53, 581, DateTimeKind.Utc).AddTicks(1243), "https://example.com/images/speaker.jpg", "Audio", "Portable Bluetooth speaker with HD sound", "Bluetooth Speaker", 49.99m },
                    { "3", "Seeder", new DateTime(2025, 6, 13, 13, 47, 53, 581, DateTimeKind.Utc).AddTicks(1246), "https://example.com/images/watch.jpg", "Wearables", "Fitness tracking smart watch with heart rate monitor", "Smart Watch", 99.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
